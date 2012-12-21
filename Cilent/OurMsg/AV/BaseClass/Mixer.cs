using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{
	public class Mixer
	{
		IntPtr m_hmx;
		public event System.EventHandler MixerControlChange;
		private event  System.EventHandler PreMixerControlChange;
		private void OnMixerControlChange(int controlid)
		{
			if(this.PreMixerControlChange!=null)this.PreMixerControlChange(this,System.EventArgs.Empty);
			if(this.MixerControlChange!=null)this.MixerControlChange(this,System.EventArgs.Empty);
		}
		MixerCallbackListener listener;
		public Mixer(Control c)
		{
			this.listener=new MixerCallbackListener(this);
			this.listener.AssignHandle(c.Handle);
			Mixer.mixerOpen(ref m_hmx,0,(int)c.Handle,0,Mixer.MIXER_OBJECTF_MIXER|0x00010000);
		}
		public IntPtr Handle
		{
			get{return this.m_hmx;}
		}
		public void Close()
		{
			Mixer.mixerClose(this.m_hmx);
		}
	
		private class MixerCallbackListener:NativeWindow
		{
			private Mixer mixer;
			public MixerCallbackListener(Mixer mixer)
			{
				this.mixer=mixer;
			}
			protected override void WndProc(ref Message m)
			{
				if(m.Msg==Mixer.MM_MIXM_CONTROL_CHANGE)
				{
					this.mixer.OnMixerControlChange((int)m.LParam);
				}
				base.WndProc (ref m);
			}

		}

		public class MixerControlDetail
		{
			private int component_type;
			Mixer mixer;
			uint lineid;
			int mute_controlid,volume_controlid;
			string name;
			int max,min;
			bool mute;int volume;
			public string Name
			{
				get{return this.name;}
			}
			public int Volume
			{
				get{return this.volume;}
				set{this.volume=value;this.SetValue(volume_controlid,value);}
			}
			public bool Mute
			{
				get{return this.mute;}
				set
				{
					if(this.mute!=value)
					{
						this.mute=value;
						if(value)
							this.SetValue(mute_controlid,1);
						else
							this.SetValue(mute_controlid,0);
					}
				}
			}
			public int Max
			{
				get{return this.max;}
			}
			public int Min
			{
				get{return this.min;}
			}
			public MixerControlDetail(Mixer mixer,int component_type)
			{
				this.component_type=component_type;
				this.mixer=mixer;
				this.Initialize();
				mixer.PreMixerControlChange+=new EventHandler(mixer_PreMixerControlChange);
			}
			private MIXERLINE GetLine()
			{
				MIXERLINE ml=new MIXERLINE();
				if(this.component_type==MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE)
				{
					MIXERLINE inLine=new MIXERLINE();
					inLine.cbStruct=Marshal.SizeOf(inLine);
					inLine.dwComponentType=Mixer.MIXERLINE_COMPONENTTYPE_DST_WAVEIN;
					Mixer.mixerGetLineInfo((int)mixer.Handle,ref inLine,Mixer.MIXER_GETLINEINFOF_COMPONENTTYPE);
					for(int i=0;i<inLine.cConnections;i++)
					{
						ml=new MIXERLINE();
						ml.cbStruct=Marshal.SizeOf(ml);
						ml.dwSource=(uint)i;
						ml.dwDestination=inLine.dwDestination;
						Mixer.mixerGetLineInfo((int)mixer.Handle,ref ml,Mixer.MIXER_GETLINEINFOF_SOURCE);
						if(ml.dwComponentType==this.component_type)break;
					}
				}
				else
				{
					ml=new MIXERLINE();
					ml.cbStruct=Marshal.SizeOf(ml);
					ml.dwComponentType=this.component_type;
					Mixer.mixerGetLineInfo((int)mixer.Handle,ref ml, Mixer.MIXER_OBJECTF_HMIXER|Mixer.MIXER_GETLINEINFOF_COMPONENTTYPE);
				}
				return ml;
			}
			private void Initialize()
			{
				MIXERLINE ml=this.GetLine();
				this.lineid=ml.dwLineID;
				this.name=ml.szName;
				MIXERCONTROL ctl=this.GetLineControl(Mixer.MIXERCONTROL_CONTROLTYPE_VOLUME);
				this.min=ctl.Bounds.dwMinimum;
				this.max=ctl.Bounds.dwMaximum;
				this.volume_controlid=ctl.dwControlID;
				ctl=this.GetLineControl(Mixer.MIXERCONTROL_CONTROLTYPE_MUTE);
				this.mute_controlid=ctl.dwControlID;

				this.volume=this.GetValue(this.volume_controlid);
				this.mute=this.GetValue(this.mute_controlid)==0;
			}
			private int GetValue(int controlid)
			{
				IntPtr volumes=Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int))*2);
				Marshal.WriteInt32(volumes,0);
				IMLibrary.AV.MIXERCONTROLDETAILS details=new MIXERCONTROLDETAILS();
				details.cbStruct=Marshal.SizeOf(details);
				details.cChannels=1;
				details.cbDetails=4;
				details.cMultipleItems=0;
				details.dwControlID=controlid;
				details.paDetails=(int)volumes;
				Mixer.mixerGetControlDetails(mixer.Handle,ref details,0);
				int ret=Marshal.ReadInt32(volumes);
				Marshal.FreeCoTaskMem(volumes);
				return ret;
			}
			private void SetValue(int controlid,int value)
			{
				IntPtr volumes=Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int))*2);
				Marshal.WriteInt32(volumes,value);
				IMLibrary.AV.MIXERCONTROLDETAILS details=new MIXERCONTROLDETAILS();
				details.cbStruct=Marshal.SizeOf(details);
				details.cChannels=1;
				details.cbDetails=4;
				details.cMultipleItems=0;
				details.dwControlID=controlid;
				details.paDetails=(int)volumes;
				int r=Mixer.mixerSetControlDetails(mixer.Handle,ref details,0);
				System.Diagnostics.Trace.WriteLine(r.ToString());
				int ret=Marshal.ReadInt32(volumes);
				System.Diagnostics.Trace.WriteLine(ret.ToString());
				Marshal.FreeCoTaskMem(volumes);
			}
			private MIXERCONTROL GetLineControl(int controlType)
			{
				MIXERCONTROL mxc=new MIXERCONTROL();mxc.Bounds=new Volume();
				MIXERLINECONTROLS mxlc=new MIXERLINECONTROLS();

				IntPtr p=Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(MIXERCONTROL)));
				Marshal.StructureToPtr(mxc,p,true);

				mxlc.cbStruct = Marshal.SizeOf(typeof(MIXERLINECONTROLS));
				mxlc.dwLineID =this.lineid;
				mxlc.dwControlType =controlType;
				mxlc.cControls = 1;
				mxlc.cbmxctrl = Marshal.SizeOf(typeof(MIXERCONTROL));
				mxlc.pamxctrl = p;
				Mixer.mixerGetLineControls((int)mixer.Handle,ref mxlc,Mixer.MIXER_OBJECTF_HMIXER |Mixer.MIXER_GETLINECONTROLSF_ONEBYTYPE);
				mxc=(MIXERCONTROL)Marshal.PtrToStructure(mxlc.pamxctrl,typeof(MIXERCONTROL));
				return mxc;
			}

			private void mixer_PreMixerControlChange(object sender, EventArgs e)
			{
				this.Initialize();
			}
		}

		#region API
		[DllImport("winmm.dll", EntryPoint="mixerClose")]
		public static extern int mixerClose (
			int hmx
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetControlDetails")]
		public static extern int mixerGetControlDetails (
			int hmxobj,
			ref MIXERCONTROLDETAILS pmxcd,
			int fdwDetails
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetDevCaps")]
		public static extern int mixerGetDevCaps (
			int uMxId,
			MIXERCAPS pmxcaps,
			int cbmxcaps
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetID")]
		public static extern int mixerGetID (
			int hmxobj,
			ref int pumxID,
			int fdwId
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetLineControls")]
		public static extern int mixerGetLineControls (
			int hmxobj,
			ref MIXERLINECONTROLS pmxlc,
			int fdwControls
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetLineInfo")]
		public static extern int mixerGetLineInfo (
			int hmxobj,
			ref MIXERLINE pmxl,
			int fdwInfo
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetNumDevs")]
		public static extern int mixerGetNumDevs ();
		[DllImport("winmm.dll", EntryPoint="mixerMessage")]
		public static extern int mixerMessage (
			int hmx,
			int uMsg,
			int dwParam1,
			int dwParam2
			);
		[DllImport("winmm.dll", EntryPoint="mixerOpen")]
		public static extern int mixerOpen (
			ref int phmx,
			int uMxId,
			int dwCallback,
			int dwInstance,
			int fdwOpen
			);
		[DllImport("winmm.dll", EntryPoint="mixerSetControlDetails")]
		public static extern int mixerSetControlDetails (
			int hmxobj,
			ref MIXERCONTROLDETAILS pmxcd,
			int fdwDetails
			);
		[DllImport("winmm.dll", EntryPoint="mixerClose")]
		public static extern int mixerClose (
			IntPtr hmx
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetControlDetails")]
		public static extern int mixerGetControlDetails (
			IntPtr hmxobj,
			ref MIXERCONTROLDETAILS pmxcd,
			uint fdwDetails
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetDevCaps")]
		public static extern int mixerGetDevCaps (
			IntPtr uMxId,
			ref MIXERCAPS pmxcaps,
			int cbmxcaps
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetID")]
		public static extern int mixerGetID (
			IntPtr hmxobj,
			ref int pumxID,
			uint fdwId
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetLineControls")]
		public static extern int mixerGetLineControls (
			int hmxobj,
			ref MIXERLINECONTROLS pmxlc,
			uint fdwControls
			);
		[DllImport("winmm.dll", EntryPoint="mixerGetLineInfo")]
		public static extern int mixerGetLineInfo (
			int hmxobj,
			ref MIXERLINE pmxl,
			uint fdwInfo
			);
		[DllImport("winmm.dll", EntryPoint="mixerMessage")]
		public static extern int mixerMessage (
			IntPtr hmx,
			int uMsg,
			int dwParam1,
			int dwParam2
			);
		[DllImport("winmm.dll", EntryPoint="mixerOpen")]
		public static extern int mixerOpen (
			ref IntPtr phmx,
			int uMxId,
			int dwCallback,
			int dwInstance,
			uint fdwOpen
			);
		[DllImport("winmm.dll", EntryPoint="mixerSetControlDetails")]
		public static extern int mixerSetControlDetails (
			IntPtr hmxobj,
			ref MIXERCONTROLDETAILS pmxcd,
			uint fdwDetails
			);
		public const uint MIXER_OBJECTF_HANDLE    =0x80000000;
		public const int MIXER_OBJECTF_MIXER     =0x00000000;
		public const uint MIXER_OBJECTF_HMIXER    =(MIXER_OBJECTF_HANDLE|MIXER_OBJECTF_MIXER);
		public const int MIXER_OBJECTF_WAVEOUT   =0x10000000;
		//public const int MIXER_OBJECTF_HWAVEOUT  =(MIXER_OBJECTF_HANDLE|MIXER_OBJECTF_WAVEOUT);
		public const int MIXER_OBJECTF_WAVEIN    =0x20000000;
		//public const int MIXER_OBJECTF_HWAVEIN   =(MIXER_OBJECTF_HANDLE|MIXER_OBJECTF_WAVEIN);
		public const int MIXER_OBJECTF_MIDIOUT   =0x30000000;
		//public const int MIXER_OBJECTF_HMIDIOUT  =(MIXER_OBJECTF_HANDLE|MIXER_OBJECTF_MIDIOUT);
		public const int MIXER_OBJECTF_MIDIIN    =0x40000000;
		//public const int MIXER_OBJECTF_HMIDIIN   =(MIXER_OBJECTF_HANDLE|MIXER_OBJECTF_MIDIIN);
		public const int MIXER_OBJECTF_AUX       =0x50000000;

		public const int MIXER_GETLINEINFOF_DESTINATION     = 0x00000000;
		public const int MIXER_GETLINEINFOF_SOURCE           =0x00000001;
		public const int  MIXER_GETLINEINFOF_LINEID           =0x00000002;
		public const int  MIXER_GETLINEINFOF_COMPONENTTYPE    =0x00000003;

		public const int MIXERLINE_COMPONENTTYPE_DST_FIRST       =0x00000000;
		public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS    =(MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
		public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST       =0x00001000;
		public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE  =(MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
		public const int MIXERLINE_COMPONENTTYPE_DST_WAVEIN      =(MIXERLINE_COMPONENTTYPE_DST_FIRST + 7);

		public const int MIXERCONTROL_CT_CLASS_FADER         =0x50000000;
		public const int MIXERCONTROL_CT_UNITS_UNSIGNED      =0x00030000;
        public const int MIXERCONTROL_CT_CLASS_SWITCH        =0x20000000;
        public const int MIXERCONTROL_CT_SC_SWITCH_BOOLEAN   =0x00000000;
        public const int MIXERCONTROL_CT_UNITS_BOOLEAN      = 0x00010000;
        public const int MIXERCONTROL_CONTROLTYPE_BOOLEAN        =(MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BOOLEAN | MIXERCONTROL_CT_UNITS_BOOLEAN);
		public const int MIXERCONTROL_CONTROLTYPE_FADER          =(MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
		public const int MIXERCONTROL_CONTROLTYPE_VOLUME         =(MIXERCONTROL_CONTROLTYPE_FADER + 1);
		public const int MIXERCONTROL_CONTROLTYPE_MUTE          =(MIXERCONTROL_CONTROLTYPE_BOOLEAN + 2);
		public const int MIXER_GETLINECONTROLSF_ONEBYTYPE        =0x00000002;
		public const int MIXER_GETCONTROLDETAILSF_VALUE     = 0x00000000;

        public const int MM_MIXM_LINE_CHANGE     =0x3D0;       /* mixer line change notify */
        public const int MM_MIXM_CONTROL_CHANGE  =0x3D1;       /* mixer control change notify */
		#endregion
	}
}
