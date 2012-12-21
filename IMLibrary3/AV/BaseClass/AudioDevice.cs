using System;
using System.Runtime.InteropServices;
namespace IMLibrary.AV
{
	/// <summary>
	/// AudioDevice 的摘要说明。
	/// </summary>
	public class AudioDevice//此类获取声卡驱动信息
	{
		private short     wMid;                    /* manufacturer ID */
		private short     wPid;                    /* product ID */
		private int vDriverVersion;        /* version of the driver */
		private string    szPname;    /* product name (NULL terminated string) */
		private int   dwFormats;               /* formats supported */
		private short   wChannels;               /* number of channels supported */
		private short   wReserved1;              /* structure packing */
		public AudioDevice()
		{
		}
		public short Mid{get{return this.wMid;}}
		public short Pid{get{return this.wPid;}}
		public int Version{get{return this.vDriverVersion;}}
		public int Formats{get{return this.dwFormats;}}
		public short Channels{get{return this.wChannels;}}
		public short Reserved1{get{return this.wReserved1;}}
		public string Name{get{return this.szPname;}}
		public static AudioDeviceCollection PlayBackDevices()
		{
			AudioDeviceCollection c=new AudioDeviceCollection();
			for(int i=0;i<waveOutGetNumDevs();i++)
			{
				WAVEOUTCAPSA a=new WAVEOUTCAPSA();
				waveOutGetDevCapsA(i,ref a,System.Runtime.InteropServices.Marshal.SizeOf(typeof(WAVEINCAPSA)));
				AudioDevice b=new AudioDevice();
				b.dwFormats=a.dwFormats;
				b.szPname=a.szPname;
				b.wPid=a.wPid;
				b.wMid=a.wMid;
				b.vDriverVersion=a.vDriverVersion;
				b.wChannels=a.wChannels;
				b.wReserved1=a.wReserved1;
				c.Add(b);
			}
			return c;
		}

		public static AudioDeviceCollection InputDevices()
		{
			AudioDeviceCollection c=new AudioDeviceCollection();
			for(int i=0;i<waveInGetNumDevs();i++)
			{
				WAVEINCAPSA a=new WAVEINCAPSA();
				waveInGetDevCapsA(i,ref a,System.Runtime.InteropServices.Marshal.SizeOf(typeof(WAVEINCAPSA)));
				AudioDevice b=new AudioDevice();
				b.dwFormats=a.dwFormats;
				b.szPname=a.szPname;
				b.wPid=a.wPid;
				b.wMid=a.wMid;
				b.vDriverVersion=a.vDriverVersion;
				b.wChannels=a.wChannels;
				b.wReserved1=a.wReserved1;
				c.Add(b);
			}
			return c;
		}

		public class AudioDeviceCollection:System.Collections.ArrayList{}
		#region
		
		[DllImport("winmm.dll")]
		private extern static int waveInGetNumDevs(); 
		[DllImport("winmm.dll")]
		private extern static int waveOutGetNumDevs(); 
		[DllImport("winmm.dll")]
		private extern static int waveInGetDevCapsA(int uDeviceID,ref WAVEINCAPSA pwic,int cbwic);
		[DllImport("winmm.dll")]
		private extern static int waveOutGetDevCapsA(int uDeviceID,ref WAVEOUTCAPSA pwoc,int cbwoc);
		#endregion
	}
}
