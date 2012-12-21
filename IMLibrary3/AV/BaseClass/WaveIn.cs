using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{
	/// <summary>
	/// WaveIn 的摘要说明。
	/// </summary>
	/// 
	public delegate void WaveBufferEventHandler(object sender,WAVEHDR hdr);

	public class WaveIn
	{
		bool m_running;
		int m_index;
		IntPtr m_in;
		waveProc m_incb;
		WAVEFORMATEX m_fmt;
		WAVEHDR[] m_hs;
		GCHandle[] gchs;
		 IntPtr[] datas=null ;
         public event WaveErrorEventHandler WaveInError;
		public event WaveBufferEventHandler WaveCaptured;
		public WaveIn(int index,WAVEFORMATEX format,int buffersize,int buffer_count)
		{
			this.m_fmt=format;
			this.m_index=index;
			m_incb=new waveProc(this.waveInProc);
			Debug.Assert(buffer_count>0 && buffer_count<65535);
			
			this.m_hs=new WAVEHDR[buffer_count];
			this.gchs=new GCHandle[buffer_count];
		//	this.datas=new IntPtr[buffer_count];
			for(int i=0;i<buffer_count;i++)
			{
				gchs[i]=GCHandle.Alloc(new byte[buffersize],GCHandleType.Pinned);
			//	this.datas[i]=Marshal.AllocCoTaskMem(buffer_count);
				WAVEHDR hdr=new WAVEHDR();
				hdr.dwBufferLength=buffersize;
				hdr.lpData=this.gchs[i].AddrOfPinnedObject();
				hdr.dwUser=i;
				this.m_hs[i]=hdr;
			}
			CheckError(waveInOpen(ref m_in,m_index,ref m_fmt,m_incb,0,0x00030000));
			CheckError(waveInStart(m_in));
		}
		~WaveIn()
		{
			waveInClose(this.m_in);
			if(this.datas==null)return;
            for (int i = 0; i < this.m_hs.Length; i++)
            {
                try
                {
                    Marshal.FreeCoTaskMem(this.datas[i]);
                }
                catch
                {
                }
            }
		}
		public WAVEFORMATEX WAVEFORMATEX
		{
			get{return this.m_fmt;}
		}
		public void Start()
		{
			if(this.m_running)throw(new AVException("正在录音"));
			for(int i=0;i<this.m_hs.Length;i++)
			{
				this.AddBuffer(ref this.m_hs[i]);
			}
			
			m_running=true;
		}
		public void Reset()
		{
			if(this.m_running)
			{
				m_running=false;
				CheckError(waveInReset(this.m_in));
			}
		}
		public void Stop()
		{
			m_running=false;
		/*	System.Threading.Thread.Sleep(100);
			if(this.m_in!=IntPtr.Zero)
			{
				CheckError(waveInClose(this.m_in));
			}*/
		}
		private void waveInProc(IntPtr hwi,int uMsg,int dwInstance,ref WAVEHDR hdr,int dwParam2)
		{
			switch(uMsg)
			{
				case WIM_OPEN:
					break;
				case WIM_DATA:
					try
					{
						CheckError(waveInUnprepareHeader(m_in,ref hdr,Marshal.SizeOf(typeof(WAVEHDR))));
						//	AddBuffer(ref hdr);
						if(this.m_running)
						{
							if(this.WaveCaptured!=null)this.WaveCaptured(this,hdr);
							this.AddBuffer(ref hdr);
						}
					}
					catch(AVException e)
					{
						m_running=false;
						if(this.WaveInError!=null)this.WaveInError(this,e);
					}
					catch(System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show(ex.Message+":"+ex.StackTrace);
						this.m_running=false;
					}
					break;
				case WIM_CLOSE:
					break;
			}
		}
		private void AddBuffer(ref WAVEHDR hdr)
		{
			hdr.dwBytesRecorded=0;
			hdr.dwFlags=0;
			hdr.lpNext=0;
			hdr.reserved=0;
			CheckError(waveInPrepareHeader(m_in,ref hdr,Marshal.SizeOf(hdr)));
			CheckError(waveInAddBuffer(m_in,ref hdr,System.Runtime.InteropServices.Marshal.SizeOf(typeof(WAVEHDR))));
		}

		#region
		const int sampleCoef=1;
		static double constraint(byte v)
		{
			return v * sampleCoef;
		}


		public static void Smooth(byte[] input, byte[] output, int length,
			double smoothness/* = 0.8*/, int scale /*= 100*/)
		{
			double a = 1.0 - (2.4 / scale);
			double b = smoothness;
			double acoef = a;
			double bcoef = a * b;
			double ccoef = a * b * b;
			double mastergain = 1.0 / (-1.0 / (Math.Log(a) + 2.0 * Math.Log(b)) +
				2.0 / (Math.Log(a) + Math.Log(b)) - 1.0 / Math.Log(a));
			double again = mastergain;
			double bgain = mastergain * (Math.Log(a * b * b) * (Math.Log(a) - Math.Log(a * b)) /
				((Math.Log(a * b * b) - Math.Log(a * b)) * Math.Log(a * b))
				- Math.Log(a) / Math.Log(a * b));
			double cgain = mastergain * (-(Math.Log(a) - Math.Log(a * b)) /
				(Math.Log(a * b * b) - Math.Log(a * b)));

			double areg = 0;
			double breg = 0;
			double creg = 0;

			//第一次循环，取得reg的平均值
			for (int j = 0; j < length; ++j)
			{
				double v = constraint(input[j]);

				areg = acoef * areg + v;
				breg = bcoef * breg + v;
				creg = ccoef * creg + v;
			}

			//得到基音
			double _base = again * areg + bgain * breg + cgain * creg;
			output[0] = (byte)_base;

			//用基音作为其始作循环得到其他数据
			for (int i = 1; i < length; ++i)
			{
				int v =(int)constraint(input[i - 1]);

				areg = acoef * areg + v;
				breg = bcoef * breg + v;
				creg = ccoef * creg + v;

				output[i] =(byte)( again * areg + bgain * breg + cgain * creg - _base);
			}
		}
		#endregion
		public void CheckError(int result)
		{
			if(result!=0)
			{
				StringBuilder sb=new StringBuilder(128);
				waveInGetErrorTextA(result,sb,128);
                return;
				//throw(new AVException(sb.ToString(),result));
			}
		}
		#region
		private const int MM_WIM_OPEN         =0x3BE;           /* waveform input */
		private const int MM_WIM_CLOSE        =0x3BF;
		private const int MM_WIM_DATA         =0x3C0;

		private const int MM_MIM_OPEN         =0x3C1;           /* MIDI input */
		private const int MM_MIM_CLOSE        =0x3C2;
		private const int MM_MIM_DATA         =0x3C3;
		private const int MM_MIM_LONGDATA     =0x3C4;
		private const int MM_MIM_ERROR        =0x3C5;
		private const int MM_MIM_LONGERROR    =0x3C6;

		private const int MM_MOM_OPEN         =0x3C7;           /* MIDI output */
		private const int MM_MOM_CLOSE        =0x3C8;
		private const int MM_MOM_DONE         =0x3C9;

		private const int WIM_OPEN        =MM_WIM_OPEN;
		private const int WIM_CLOSE       =MM_WIM_CLOSE;
		private const int WIM_DATA        =MM_WIM_DATA;

		[DllImport("winmm.dll")]
		private extern static int waveInGetDevCapsA(int uDeviceID,ref WAVEINCAPSA pwic,int cbwic);
		[DllImport("winmm.dll")]
		private extern static int waveInGetDevCapsW(int uDeviceID,ref WAVEINCAPSW pwic,int cbwic);


		[DllImport("winmm.dll")]
		private extern static int waveInOpen(ref IntPtr phwi, int uDeviceID,
			ref WAVEFORMATEX pwfx,waveProc dwCallback, int dwInstance, int fdwOpen);

		[DllImport("winmm.dll")]
		private extern static int waveInClose( IntPtr hwi);
		[DllImport("winmm.dll")]
		private extern static int waveInPrepareHeader( IntPtr hwi,ref WAVEHDR pwh, int cbwh);
		[DllImport("winmm.dll")]
		private extern static int waveInUnprepareHeader( IntPtr hwi,ref WAVEHDR pwh, int cbwh);
		[DllImport("winmm.dll",PreserveSig=true)]
		private extern static int waveInAddBuffer( IntPtr hwi,ref WAVEHDR pwh, int cbwh);
		[DllImport("winmm.dll")]
		private extern static int waveInStart( IntPtr hwi);
		[DllImport("winmm.dll")]
		private extern static int waveInStop( IntPtr hwi);
		[DllImport("winmm.dll")]
		private extern static int waveInReset( IntPtr hwi);
		[DllImport("winmm.dll")]
		private extern static int waveInGetID( IntPtr hwi, int puDeviceID);
		[DllImport("winmm.dll")]
		private extern static int waveInGetErrorTextA(int mmrError,StringBuilder pszText, int cchText);
		[DllImport("winmm.dll")]
		private extern static int waveInGetErrorTextW(int mmrError,StringBuilder pszText, int cchText);
		[DllImport("winmm.dll")]
		private extern static int waveInGetNumDevs(); 
		#endregion

	}
    public delegate void WaveErrorEventHandler(object sender, AVException e);
}
