using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{
	/// <summary>
	/// WaveOut 的摘要说明。
	/// </summary>
	/// 
	public class SoundPlayer
	{
		[DllImport("Winmm.dll")]
		static extern bool  sndPlaySound(string wfname, int fuSound);
		public SoundPlayer()
		{
		}

		public void Play(string file)
		{

			bool b=sndPlaySound(file,1);
		}

        [DllImport("winmm.dll")]
         static extern int sndPlaySoundA(byte[] lpszSoundName, int uFlags);
        public void play(System.IO.Stream stream)
        {
            byte[] buf = new byte[stream.Length];
            int SND_MEMORY = 0x4;
            stream.Read(buf, 0, buf.Length);
            stream.Close();
            sndPlaySoundA(buf, SND_MEMORY);
        }
	}

	public class WaveOut
	{
		bool m_running;
		int m_outdex;
		IntPtr m_out;
		waveProc m_outcb;
		WAVEFORMATEX m_fmt;
		WAVEHDR[] m_hs;
		GCHandle[] gchs;
		int m_pos=0;
		int m_buffersize;
		public event WaveErrorEventHandler WaveOutError;
		public WaveOut(int index,WAVEFORMATEX format,int buffersize,int buffer_count)//波形输出
		{
			this.m_fmt=format;
			this.m_outdex=index;
			m_buffersize=buffersize;
			m_outcb=new waveProc(this.waveOutProc);
			Debug.Assert(buffer_count>0 && buffer_count<65535);
			this.m_hs=new WAVEHDR[buffer_count];
			this.gchs=new GCHandle[buffer_count];
			for(int i=0;i<buffer_count;i++)
			{
				gchs[i]=GCHandle.Alloc(new byte[buffersize],GCHandleType.Pinned);
				WAVEHDR hdr=new WAVEHDR();
				hdr.dwBufferLength=buffersize;
				hdr.lpData=this.gchs[i].AddrOfPinnedObject();
				hdr.dwUser=i;
				this.m_hs[i]=hdr;
			}
			CheckError(waveOutOpen(ref m_out,m_outdex,ref m_fmt,m_outcb,0,0x00030000));
		}
		~WaveOut()//波形输出
		 {
			 for(int i=0;i<this.m_hs.Length;i++)
			 {
				 //	Marshal.FreeCoTaskMem(this.m_hs[i].lpData);
				 this.gchs[i].Free();
			 }
			if(this.m_out!=IntPtr.Zero)
			{
				try
				{
					CheckError(waveOutClose(this.m_out));
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
			if(this.m_running)throw(new AVException("正在播放"));
			m_running=true;
		}
		public void Reset()
		{
			this.m_pos=0;
			CheckError(waveOutReset(this.m_out));
			m_running=false;
		}
		public void Stop()
		{
			m_running=false;
		//	this.m_pos=0;
		}
		private void waveOutProc(IntPtr hwi,int uMsg,int dwInstance,ref WAVEHDR hdr,int dwParam2)
		{
			switch(uMsg)
			{
				case WOM_OPEN:
					break;
				case WOM_DONE:
					try
					{
						this.CheckError(waveOutUnprepareHeader(m_out,ref hdr,Marshal.SizeOf(typeof(WAVEHDR))));
						m_pos=hdr.dwUser;
						hdr.dwBytesRecorded=0;
					}
					catch(AVException e)
					{
						m_running=false;
						if(this.WaveOutError!=null)this.WaveOutError(this,e);
					}
					break;
				case WOM_CLOSE:
					break;
			}
		}

		public void Write(byte[] data)
		{
            try
            {
                if (!this.m_running) return;
                for (int i = this.m_pos; i < this.m_hs.Length + this.m_pos; i++)
                {
                    int newpos = i % this.m_hs.Length;
                    if (this.m_hs[newpos].dwBytesRecorded == 0)
                    {
                        int cnt = data.Length > this.m_buffersize ? this.m_buffersize : data.Length;
                        this.m_hs[newpos].dwBytesRecorded = cnt;
                        Marshal.Copy(data, 0, this.m_hs[newpos].lpData, cnt);
                        CheckError(waveOutPrepareHeader(m_out, ref this.m_hs[newpos], Marshal.SizeOf(typeof(WAVEHDR))));
                        CheckError((waveOutWrite(m_out, ref this.m_hs[newpos], System.Runtime.InteropServices.Marshal.SizeOf(typeof(WAVEHDR)))));
                        break;
                    }
                }
            }
            catch { }
		}

		public void CheckError(int result)
		{
			if(result!=0)
			{
				this.m_pos=0;
				StringBuilder sb=new StringBuilder(128);
				waveOutGetErrorTextA(result,sb,128);
				throw(new AVException(sb.ToString(),result));
			}
		}




		#region
		private const int MM_WOM_OPEN         =0x3BB;           /* waveform output */
		private const int MM_WOM_CLOSE        =0x3BC;
		private const int MM_WOM_DONE         =0x3BD;
		private const int MM_MOM_OPEN         =0x3C7;           /* MIDI output */
		private const int MM_MOM_CLOSE        =0x3C8;
		private const int MM_MOM_DONE         =0x3C9;

		private const int WOM_OPEN        =MM_WOM_OPEN;
		private const int WOM_CLOSE       =MM_WOM_CLOSE;
		private const int WOM_DONE        =MM_WOM_DONE;

		private const int WAVE_INVALIDFORMAT     =0x00000000;       /* invalid format */


		[DllImport("winmm.dll")]
		private extern static int waveInGetDevCapsA(int uDeviceID,ref WAVEINCAPSA pwic,int cbwic);
		[DllImport("winmm.dll")]
		private extern static int waveInGetDevCapsW(int uDeviceID,ref WAVEINCAPSW pwic,int cbwic);


		[DllImport("winmm.dll")]
		private extern static int waveInOpen(ref IntPtr phwi, int uDeviceID,
			ref WAVEFORMATEX pwfx,waveProc dwCallback, int dwInstance, int fdwOpen);

		[DllImport("winmm.dll")]
		private extern static int waveOutGetDevCapsA(int uDeviceID,ref WAVEOUTCAPSA pwoc,int cbwoc);
		[DllImport("winmm.dll")]
		private extern static int waveOutGetDevCapsW(int uDeviceID,ref WAVEOUTCAPSW pwoc,int cbwoc);
		[DllImport("winmm.dll")]
		private extern static int waveOutGetVolume(IntPtr hwo,ref int pdwVolume);
		[DllImport("winmm.dll")]
		private extern static int waveOutSetVolume(IntPtr hwo,int dwVolume);

		[DllImport("winmm.dll")]
		private extern static int waveOutOpen(ref IntPtr phwo,int uDeviceID,
			ref WAVEFORMATEX pwfx, waveProc dwCallback,int dwInstance, int fdwOpen);
		[DllImport("winmm.dll")]
		private extern static int waveOutClose(IntPtr hwo);
		[DllImport("winmm.dll")]
		private extern static int waveOutPrepareHeader(IntPtr hwo,ref WAVEHDR pwh,int cbwh);
		[DllImport("winmm.dll")]
		private extern static int waveOutUnprepareHeader(IntPtr hwo,ref WAVEHDR pwh,int cbwh);
		[DllImport("winmm.dll")]
		private extern static int waveOutWrite(IntPtr hwo,ref WAVEHDR pwh,int cbwh);
		[DllImport("winmm.dll")]
		private extern static int waveOutPause(IntPtr hwo);
		[DllImport("winmm.dll")]
		private extern static int waveOutRestart(IntPtr hwo);
		[DllImport("winmm.dll")]
		private extern static int waveOutReset(IntPtr hwo);
		[DllImport("winmm.dll")]
		private extern static int waveOutGetErrorTextA(int mmrError,StringBuilder pszText,int cchText);
		[DllImport("winmm.dll")]
		private extern static int  waveOutGetErrorTextW(int mmrError,StringBuilder pszText,int cchText);
		[DllImport("winmm.dll")]
		private extern static int waveOutGetNumDevs(); 
		#endregion
	}
}
