using System;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{
	public class VideoCapturer
	{
        public capVideoStreamCallback m_streamCallBack;
        public capErrorCallback m_errorCallBack;
        CaptureParms m_parms;
		BITMAPINFO m_BITMAPINFO;
		IntPtr m_hwnd;
		bool m_IsCapturing;
		bool m_HasPreview;
		bool m_Connected;
      
		private int index;
		public event VideoCaptureEventHandler VideoCaptured;
		public event VideoErrorEventHandler VideoError;

        /// <summary>
        /// 捕捉视频
        /// </summary>
        /// <param name="control"></param>
        /// <param name="index"></param>
        public VideoCapturer(Control control, int index)
        {
            this.index = index;
            m_hwnd = capCreateCaptureWindow("", WS_VISIBLE | WS_CHILD, 0, 0, control.Width, control.Height, control.Handle, 0);

            if (m_hwnd == IntPtr.Zero) throw new AVException();
            m_parms = new CaptureParms();

            m_streamCallBack = new  capVideoStreamCallback(this.streamCallBack);
            m_errorCallBack = new capErrorCallback(this.capErrorCallback);

            SendMessage(m_hwnd, WM_CAP_SET_CALLBACK_VIDEOSTREAM, 0, m_streamCallBack);
            SendMessage(m_hwnd, WM_CAP_SET_CALLBACK_ERRORA, 0, m_errorCallBack);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public int ShowVideoDisplay()
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_DLG_VIDEODISPLAY,0,0);
		}

        /// <summary>
        /// 显示视频压缩
        /// </summary>
        /// <returns></returns>
		public int ShowVideoCompression()
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_DLG_VIDEOCOMPRESSION,0,0);
		}

        /// <summary>
        /// 显示视频格式 
        /// </summary>
        /// <returns></returns>
		public int ShowVideoFormat()
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_DLG_VIDEOFORMAT,0,0);
		}

        /// <summary>
        /// 显示视频资源
        /// </summary>
        /// <returns></returns>
		public int ShowVideoSource()
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_DLG_VIDEOSOURCE,0,0);
		}

        /// <summary>
        /// 设置驱动索引
        /// </summary>
        /// <returns></returns>
		public int ConnectDevice()
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_DRIVER_CONNECT,this.index,0);
		}

        /// <summary>
        /// 设置速率
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
		public int SetPreviewRate(int rate)
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_SET_PREVIEWRATE,rate,0);
		}

        /// <summary>
        /// 是否在图片框中预览图像
        /// </summary>
		public bool Preview
		{
			get{return this.m_HasPreview;}
			set
			{
				int ret=SendMessageClass.SendMessage(m_hwnd,WM_CAP_SET_PREVIEW,value,0);
				if(ret==1)this.m_HasPreview=value;
			}
		}


        /// <summary>
        /// 捕获视频数据到IO设备或内存
        /// </summary>
        /// <returns></returns>
		public int CaptureWithOutFile()
		{
	//		Debug.Assert(this.Connected);
			int ret=SendMessageClass.SendMessage(m_hwnd,WM_CAP_SEQUENCE_NOFILE,0,0);
			return ret;
		}

        /// <summary>
        /// 连接到摄像头驱动
        /// </summary>
		public void  Disconnect()
		{
			SendMessageClass.SendMessage(m_hwnd,WM_CAP_DRIVER_DISCONNECT,0,0);
		}

        /// <summary>
        /// 关闭
        /// </summary>
		public void Close()
		{
			SendMessageClass.SendMessage(m_hwnd,0x10,0,0);
		}

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
		public int Stop()
		{
			int ret=SendMessageClass.SendMessage(m_hwnd,WM_CAP_STOP,0,0);
			return ret;
		}

        /// <summary>
        /// 终止图像捕获
        /// </summary>
        /// <returns></returns>
		public int Abort()
		{
			int ret=SendMessageClass.SendMessage(m_hwnd,WM_CAP_ABORT,0,0);
			return ret;
		}

        /// <summary>
        /// 将当前图像保存为图片文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
		public int SavePicture(string file)
		{
			return SendMessage(m_hwnd,WM_CAP_FILE_SAVEDIBW,0,file);
		}

        /// <summary>
        /// 设置为相关参数
        /// </summary>
		public CaptureParms CaptureParms
		{
			get
			{
				SendMessage(m_hwnd,WM_CAP_GET_SEQUENCE_SETUP,Marshal.SizeOf(m_parms),ref m_parms);
				return this.m_parms;
			}
			set
			{
				m_parms=value;
				SendMessage(m_hwnd,WM_CAP_SET_SEQUENCE_SETUP,Marshal.SizeOf(m_parms),ref m_parms);
			}
		}

        /// <summary>
        /// 改变图像大小等信息
        /// </summary>
		public BITMAPINFO BITMAPINFO 
		{
			get
			{
				SendMessage(m_hwnd,WM_CAP_GET_VIDEOFORMAT,Marshal.SizeOf(m_BITMAPINFO),ref m_BITMAPINFO);
				return this.m_BITMAPINFO;
			}
			set
			{
				this.m_BITMAPINFO=value;
				SendMessage(m_hwnd,WM_CAP_SET_VIDEOFORMAT,Marshal.SizeOf(m_BITMAPINFO),ref m_BITMAPINFO);
			}
		}

		public int SetScale(bool value)
		{
			return SendMessageClass.SendMessage(m_hwnd,WM_CAP_SET_SCALE,value,0);
		}
		public CAPSTATUS CAPSTATUS
		{
			get
			{
				CAPSTATUS status=new CAPSTATUS();
				SendMessage(m_hwnd,WM_CAP_GET_STATUS,Marshal.SizeOf(CAPSTATUS),ref status);
				return status;
			}
		}
		public bool Connected
		{
			get{return this.m_Connected;}
		}
		public IntPtr Handler
		{
			get{return this.Handler;}
		}
		public bool IsCapturing
		{
			get{return this.m_IsCapturing;}
		}
         
		private int  streamCallBack(IntPtr hWnd,ref  VIDEOHDR lpVHdr)
		{
			if(this.VideoCaptured!=null)
                this.VideoCaptured(this, lpVHdr);
             return 0;
		}
		private int capErrorCallback(IntPtr hWnd,int nID,string lpsz)
		{
			this.m_IsCapturing=false;
			this.m_HasPreview=false;
			if(this.m_Connected)
			{
				this.Disconnect();
				this.m_Connected=false;
			}
			if(this.VideoError!=null)this.VideoError(this,lpsz);
			return 0;
		}

		public class VideoCaptureDeviceCollection:System.Collections.ArrayList{}
		public static VideoCaptureDeviceCollection GetDevices()
		{
			VideoCaptureDeviceCollection c=new VideoCaptureDeviceCollection();
			for(int i=0;i<10;i++)
			{
				StringBuilder name,version;
				name=new StringBuilder( 128);
				version=new StringBuilder(128);
				if(capGetDriverDescription(i,name,128,version,128))
				{
					c.Add(new VideoCaptureDevice(i,name.ToString(),version.ToString()));
				}
			}
			return c;
		}


		#region

		private static int WS_CHILD = 1073741824;
		private static int WS_VISIBLE = 268435456;


		private const int WM_USER                        = 0x0400;
		private const int WM_CAP_START                   =WM_USER;
		private const int WM_CAP_UNICODE_START            =WM_USER+100;
		private const int WM_CAP_SET_CALLBACK_ERRORW     =(WM_CAP_UNICODE_START+  2);
		private const int WM_CAP_SET_CALLBACK_ERRORA     =(WM_CAP_START+  2);
		private const int WM_CAP_SET_CALLBACK_FRAME      = (WM_CAP_START+  5);
		private const int  WM_CAP_SET_CALLBACK_VIDEOSTREAM =(WM_CAP_START+  6);


		public static int  WM_CAP_DRIVER_CONNECT           =(WM_CAP_START+  10);
		public static int  WM_CAP_DRIVER_DISCONNECT        =(WM_CAP_START+  11);

		public static int  WM_CAP_DRIVER_GET_NAMEA        =(WM_CAP_START+  12);
		public static int  WM_CAP_DRIVER_GET_VERSIONA     =(WM_CAP_START+  13);
		public static int  WM_CAP_DRIVER_GET_NAMEW        =(WM_CAP_UNICODE_START+  12);
		public static int  WM_CAP_DRIVER_GET_VERSIONW     =(WM_CAP_UNICODE_START+  13);
		public static int  WM_CAP_DRIVER_GET_NAME          =WM_CAP_DRIVER_GET_NAMEW;
		public static int  WM_CAP_DRIVER_GET_VERSION       =WM_CAP_DRIVER_GET_VERSIONW;

		public static int  WM_CAP_DRIVER_GET_CAPS          =(WM_CAP_START+  14);

		public static int  WM_CAP_FILE_SET_CAPTURE_FILEA  =(WM_CAP_START+  20);
		public static int  WM_CAP_FILE_GET_CAPTURE_FILEA  =(WM_CAP_START+  21);
		public static int  WM_CAP_FILE_SAVEASA            =(WM_CAP_START+  23);
		public static int  WM_CAP_FILE_SAVEDIBA           =(WM_CAP_START+  25);
		public static int  WM_CAP_FILE_SET_CAPTURE_FILEW  =(WM_CAP_UNICODE_START+  20);
		public static int  WM_CAP_FILE_GET_CAPTURE_FILEW  =(WM_CAP_UNICODE_START+  21);
		public static int  WM_CAP_FILE_SAVEASW            =(WM_CAP_UNICODE_START+  23);
		public static int  WM_CAP_FILE_SAVEDIBW           =(WM_CAP_UNICODE_START+  25);
		public static int  WM_CAP_FILE_SET_CAPTURE_FILE    =WM_CAP_FILE_SET_CAPTURE_FILEW;
		public static int  WM_CAP_FILE_GET_CAPTURE_FILE    =WM_CAP_FILE_GET_CAPTURE_FILEW;
		public static int  WM_CAP_FILE_SAVEAS              =WM_CAP_FILE_SAVEASW;
		public static int  WM_CAP_FILE_SAVEDIB             =WM_CAP_FILE_SAVEDIBW;

		// out of order to save on ifdefs
		public static int  WM_CAP_FILE_ALLOCATE            =(WM_CAP_START+  22);
		public static int  WM_CAP_FILE_SET_INFOCHUNK       =(WM_CAP_START+  24);

		public static int  WM_CAP_EDIT_COPY                =(WM_CAP_START+  30);

		public static int  WM_CAP_SET_AUDIOFORMAT          =(WM_CAP_START+  35);
		public static int  WM_CAP_GET_AUDIOFORMAT          =(WM_CAP_START+  36);

		public static int  WM_CAP_DLG_VIDEOFORMAT          =(WM_CAP_START+  41);
		public static int  WM_CAP_DLG_VIDEOSOURCE          =(WM_CAP_START+  42);
		public static int  WM_CAP_DLG_VIDEODISPLAY         =(WM_CAP_START+  43);
		public static int  WM_CAP_GET_VIDEOFORMAT          =(WM_CAP_START+  44);
		public static int  WM_CAP_SET_VIDEOFORMAT          =(WM_CAP_START+  45);
		public static int  WM_CAP_DLG_VIDEOCOMPRESSION     =(WM_CAP_START+  46);

		public static int  WM_CAP_SET_PREVIEW              =(WM_CAP_START+  50);
		public static int  WM_CAP_SET_OVERLAY              =(WM_CAP_START+  51);
		public static int  WM_CAP_SET_PREVIEWRATE          =(WM_CAP_START+  52);
		public static int  WM_CAP_SET_SCALE                =(WM_CAP_START+  53);
		public static int  WM_CAP_GET_STATUS               =(WM_CAP_START+  54);
		public static int  WM_CAP_SET_SCROLL               =(WM_CAP_START+  55);

		public static int  WM_CAP_GRAB_FRAME               =(WM_CAP_START+  60);
		public static int  WM_CAP_GRAB_FRAME_NOSTOP        =(WM_CAP_START+  61);

		public static int  WM_CAP_SEQUENCE                 =(WM_CAP_START+  62);
		public static int  WM_CAP_SEQUENCE_NOFILE          =(WM_CAP_START+  63);
		public static int  WM_CAP_SET_SEQUENCE_SETUP       =(WM_CAP_START+  64);
		public static int  WM_CAP_GET_SEQUENCE_SETUP       =(WM_CAP_START+  65);

		public static int  WM_CAP_SET_MCI_DEVICEA         =(WM_CAP_START+  66);
		public static int  WM_CAP_GET_MCI_DEVICEA         =(WM_CAP_START+  67);
		public static int  WM_CAP_SET_MCI_DEVICEW         =(WM_CAP_UNICODE_START+  66);
		public static int  WM_CAP_GET_MCI_DEVICEW         =(WM_CAP_UNICODE_START+  67);
		public static int  WM_CAP_SET_MCI_DEVICE           =WM_CAP_SET_MCI_DEVICEW;
		public static int  WM_CAP_GET_MCI_DEVICE           =WM_CAP_GET_MCI_DEVICEW;
		public static int  WM_CAP_STOP                     =(WM_CAP_START+  68);
		public static int  WM_CAP_ABORT                    =(WM_CAP_START+  69);

		public static int  WM_CAP_SINGLE_FRAME_OPEN        =(WM_CAP_START+  70);
		public static int  WM_CAP_SINGLE_FRAME_CLOSE       =(WM_CAP_START+  71);
		public static int  WM_CAP_SINGLE_FRAME             =(WM_CAP_START+  72);

		public static int  WM_CAP_PAL_OPENA               =(WM_CAP_START+  80);
		public static int  WM_CAP_PAL_SAVEA               =(WM_CAP_START+  81);
		public static int  WM_CAP_PAL_OPENW               =(WM_CAP_UNICODE_START+  80);
		public static int  WM_CAP_PAL_SAVEW               =(WM_CAP_UNICODE_START+  81);
		public static int  WM_CAP_PAL_OPEN                = WM_CAP_PAL_OPENW;
		public static int  WM_CAP_PAL_SAVE                 =WM_CAP_PAL_SAVEW;

		public static int  WM_CAP_PAL_PASTE                =(WM_CAP_START+  82);
		public static int  WM_CAP_PAL_AUTOCREATE           =(WM_CAP_START+  83);
		public static int  WM_CAP_PAL_MANUALCREATE         =(WM_CAP_START+  84);

		// Following added post VFW 1.1
		public static int  WM_CAP_SET_CALLBACK_CAPCONTROL  =(WM_CAP_START+  85);


		[DllImport("AVICAP32.dll")]
		private static extern IntPtr capCreateCaptureWindow(
			string lpszWindowName,
			int dwStyle,
			int x, int y, int nWidth, int nHeight,
			IntPtr hwndParent, int nID);
		[DllImport("AVICAP32.dll",CharSet=CharSet.Unicode)]
		private static extern bool capGetDriverDescription(int wDriverIndex,
			StringBuilder lpszName, int cbName,
			StringBuilder lpszVer, int cbVer);


		[DllImport("USER32.DLL")]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			capErrorCallback lParam
			);	
		[DllImport("USER32.DLL")]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			capVideoStreamCallback lParam
			);	
		[DllImport("USER32.DLL")]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			ref CaptureParms lParam
			);	
		[DllImport("USER32.DLL")]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			ref BITMAPINFO lParam
			);	
		[DllImport("USER32.DLL")]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			ref CAPSTATUS lParam
			);	
		[DllImport("USER32.DLL",CharSet=CharSet.Auto)]
		static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			string lParam
			);	
		#endregion
	}
	public class VideoCaptureDevice
	{
		private int index;private string name,version;
		public VideoCaptureDevice(int index,string name,string version)
		{
			this.index=index;this.name=name;this.version=version;
		}
		public int Index{get{return this.index;}}
		public string Name{get{return this.name;}}
		public string Version{get{return this.version;}}
	}
}
