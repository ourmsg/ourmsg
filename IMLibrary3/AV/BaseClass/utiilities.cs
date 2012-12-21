using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.Serialization;


namespace IMLibrary.AV
{
	#region wave
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEHDR 
	{
		public IntPtr       lpData;                 /* pointer to locked data buffer */
		public int       dwBufferLength;         /* length of data buffer */
		public int       dwBytesRecorded;        /* used for input only */
		public int   dwUser;                 /* for client's use */
		public int       dwFlags;                /* assorted flags (see defines) */
		public int       dwLoops;                /* loop control counter */
		public int  lpNext;     /* reserved for driver */
		public int   reserved;               /* reserved for driver */
	} 
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEFORMATEX
	{
		public short  wFormatTag;
		public short   nChannels;
		public int   nSamplesPerSec;
		public int   nAvgBytesPerSec;
		public short   nBlockAlign;
		public short   wBitsPerSample;
		public short   cbSize;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct PCMWAVEFORMAT 
	{
		public WAVEFORMAT  wf;
		public short        wBitsPerSample;
	} 
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEOUTCAPSA 
	{
		public short     wMid;                  /* manufacturer ID */
		public short     wPid;                  /* product ID */
		public int vDriverVersion;      /* version of the driver */
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string    szPname;   /* product name (NULL terminated string) */
		public int   dwFormats;             /* formats supported */
		public short     wChannels;             /* number of sources supported */
		public short     wReserved1;            /* packing */
		public int   dwSupport;             /* functionality supported by driver */
	} 
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEOUTCAPSW 
	{
		public short     wMid;                  /* manufacturer ID */
		public short     wPid;                  /* product ID */
		public	int vDriverVersion;      /* version of the driver */
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string    szPname;   /* product name (NULL terminated string) */
		public int   dwFormats;             /* formats supported */
		public short     wChannels;             /* number of sources supported */
		public short     wReserved1;            /* packing */
		public int  dwSupport;             /* functionality supported by driver */
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEINCAPSW
	{
		public short    wMid;                    /* manufacturer ID */
		public short    wPid;                    /* product ID */
		public uint     vDriverVersion;        /* version of the driver */
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string    szPname;    /* product name (NULL terminated string) */
		public int     dwFormats;               /* formats supported */
		public short    wChannels;               /* number of channels supported */
		public short    wReserved1;              /* structure packing */
	} 
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEINCAPSA 
	{
		public short     wMid;                    /* manufacturer ID */
		public short     wPid;                    /* product ID */
		public int vDriverVersion;        /* version of the driver */
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string    szPname;    /* product name (NULL terminated string) */
		public int   dwFormats;               /* formats supported */
		public short    wChannels;               /* number of channels supported */
		public short    wReserved1;              /* structure packing */
	} 
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEFORMAT 
	{
		public short   wFormatTag;        /* format type */
		public short   nChannels;         /* number of channels (i.e. mono, stereo, etc.) */
		public int  nSamplesPerSec;    /* sample rate */
		public int  nAvgBytesPerSec;   /* for buffer estimation */
		public short   nBlockAlign;       /* block size of data */
	} 

	public delegate void waveProc(IntPtr hwi,int uMsg,int dwInstance,ref WAVEHDR hdr,int dwParam2);

	#endregion

	#region video
    public delegate void VideoCaptureEventHandler(object sender, VIDEOHDR hdr);
    public delegate void VideoErrorEventHandler(object sender, string desc);

	public delegate int capErrorCallback(
	IntPtr hWnd,  
	int nID,    
	string lpsz 
	);
	public delegate int capVideoStreamCallback(
	IntPtr hWnd,         
	ref VIDEOHDR lpVHdr  
	);

	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public struct VIDEOHDR 
	{
		public	IntPtr      lpData;                 /* pointer to locked data buffer */
		public	int       dwBufferLength;         /* Length of data buffer */
		public	int       dwBytesUsed;            /* Bytes actually used */
		public	int       dwTimeCaptured;         /* Milliseconds from start of stream */
		public	int   dwUser;                 /* for client's use */
		public	int       dwFlags;                /* assorted flags (see defines) */
		public	int   dwReserved;          /* reserved for driver */
	} 

	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public struct CaptureParms 
	{
		public	int       dwRequestMicroSecPerFrame;  // Requested capture rate
		public	bool        fMakeUserHitOKToCapture;    // Show "Hit OK to cap" dlg?
		public	uint        wPercentDropForError;       // Give error msg if > (10%)
		public	bool        fYield;                     // Capture via background task?
		public	int       dwIndexSize;                // Max index size in frames (32K)
		public	uint        wChunkGranularity;          // Junk chunk granularity (2K)
		public	bool        fUsingDOSMemory;            // Use DOS buffers?
		public	uint        wNumVideoRequested;         // # video buffers, If 0, autocalc
		public	bool        fCaptureAudio;              // Capture audio?
		public	uint        wNumAudioRequested;         // # audio buffers, If 0, autocalc
		public	uint        vKeyAbort;                  // Virtual key causing abort
		public	bool        fAbortLeftMouse;            // Abort on left mouse?
		public	bool        fAbortRightMouse;           // Abort on right mouse?
		public	bool        fLimitEnabled;              // Use wTimeLimit?
		public	uint        wTimeLimit;                 // Seconds to capture
		public	bool        fMCIControl;                // Use MCI video source?
		public	bool        fStepMCIDevice;             // Step MCI device?
		public	int       dwMCIStartTime;             // Time to start in MS
		public	int       dwMCIStopTime;              // Time to stop in MS
		public	bool        fStepCaptureAt2x;           // Perform spatial averaging 2x
		public	int        wStepCaptureAverageFrames;  // Temporal average n Frames
		public	int       dwAudioBufferSize;          // Size of audio bufs (0 = default)
		public	int        fDisableWriteCache;         // Attempt to disable write cache
		public	int        AVStreamMaster;             // Which stream controls length?
	} 

	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public  struct BITMAPINFOHEADER
	{
		public	int  biSize; 
		public	int   biWidth; 
		public	int   biHeight; 
		public	short   biPlanes; 
		public	short   biBitCount; 
		public	int  biCompression; 
		public	int  biSizeImage; 
		public	int   biXPelsPerMeter; 
		public	int   biYPelsPerMeter; 
		public	int  biClrUsed; 
		public	int  biClrImportant; 
	} 

	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public struct BITMAPINFO 
	{ 
		public	BITMAPINFOHEADER bmiHeader; 
		public	int          bmiColors; 
	}
	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public struct CAPSTATUS 
	{
		public	int        uiImageWidth;               // Width of the image
		public	int        uiImageHeight;              // Height of the image
		public	bool        fLiveWindow;                // Now Previewing video?
		public	bool        fOverlayWindow;             // Now Overlaying video?
		public	bool        fScale;                     // Scale image to client?
		public	Point       ptScroll;                   // Scroll position
		public	bool        fUsingDefaultPalette;       // Using default driver palette?
		public	bool        fAudioHardware;             // Audio hardware present?
		public	bool        fCapFileExists;             // Does capture file exist?
		public	int       dwCurrentVideoFrame;        // # of video frames cap'td
		public	int       dwCurrentVideoFramesDropped;// # of video frames dropped
		public	int       dwCurrentWaveSamples;       // # of wave samples cap'td
		public	int       dwCurrentTimeElapsedMS;     // Elapsed capture duration
		public	IntPtr    hPalCurrent;                // Current palette in use
		public	bool        fCapturingNow;              // Capture in progress?
		public	int       dwReturn;                   // Error value after any operation
		public	int        wNumVideoAllocated;         // Actual number of video buffers
		public	int        wNumAudioAllocated;         // Actual number of audio buffers
	} 
	#endregion

	#region mixer

	[StructLayout(LayoutKind.Sequential)]
	public struct MIXERCAPS 
	{
		public short wMid;
		public short wPid;
		public int vDriverVersion;
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
		public string szPname;
		public int fdwSupport;
		public int cDestinations;
	}
  
	[StructLayout(LayoutKind.Explicit,Size=148)]
	public struct MIXERCONTROL
	{ 
		[FieldOffset(0)]
		public int cbStruct; 
		[FieldOffset(4)]
		public int dwControlID; 
		[FieldOffset(8)]
		public int dwControlType; 
		[FieldOffset(12)]
		public int fdwControl; 
		[FieldOffset(16)]
		public int cMultipleItems; 
		[FieldOffset(20)]
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=16)]
		string  szShortName; 
		[FieldOffset(36)]
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=64)]
		string  szName; 
		[FieldOffset(100)]
		public Volume Bounds;
		[FieldOffset(124)]
		public int Metrics; 
	} 

	public struct Volume
	{
		public int dwMinimum; 
		public int dwMaximum; 
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MIXERCONTROLDETAILS 
	{
		public int cbStruct;
		public int dwControlID;
		public int cChannels;
		public int cMultipleItems;
		public int cbDetails;
		public int paDetails;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class MIXERCONTROLDETAILS_BOOLEAN 
	{
		public bool fValue;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MIXERCONTROLDETAILS_LISTTEXT 
	{
		public int dwParam1;
		public int dwParam2;
		public string szName;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class MIXERCONTROLDETAILS_SIGNED 
	{
		public int lValue;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class MIXERCONTROLDETAILS_UNSIGNED 
	{
		public int dwValue;
	}
 
	[StructLayout(LayoutKind.Sequential)]
	public struct MIXERLINE 
	{
		public int cbStruct;
		public uint dwDestination;
		public uint dwSource;
		public uint dwLineID;
		public int fdwLine;
		public int dwUser;
		public int dwComponentType;
		public int cChannels;
		public int cConnections;
		public int cControls;
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=16)]
		public string szShortName;
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=64)]
		public string szName;
		public Target tTarget;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Target
	{ 
		public int     dwType; 
		public int     dwDeviceID; 
		public short      wMid; 
		public short      wPid;
		public int vDriverVersion; 
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
		public string    szPname; 

	} 

	 
	[StructLayout(LayoutKind.Sequential)]
	public struct MIXERLINECONTROLS 
	{
		public int cbStruct;
		public uint dwLineID;   
		//union 
		//{ 
		//	DWORD dwControlID; 
		//	DWORD dwControlType; 
		//}; 
		public int dwControlType;
		public int cControls;
		public int cbmxctrl;
		public IntPtr pamxctrl;
	}
	#endregion

	#region compress
	
	/// <summary>
	/// Values for dwFlags of ICOpen() 
	/// </summary>
	public enum ICMODE
	{
		ICMODE_COMPRESS=1,
		ICMODE_DECOMPRESS=2,
		ICMODE_FASTDECOMPRESS=3,
		ICMODE_QUERY=4,
		ICMODE_FASTCOMPRESS=5,
		ICMODE_DRAW=8
	}
	public class FOURCC
	{
		public static readonly int DIVX=FOURCC.mmioFOURCC('d','i','v','x');
		public static readonly int MP42=FOURCC.mmioFOURCC('M','P','4','2');
		public static readonly int streamtypeVIDEO = mmioFOURCC('v', 'i', 'd', 's');
		public static readonly int streamtypeAUDIO = mmioFOURCC('a', 'u', 'd', 's');
		public static readonly int streamtypeMIDI = mmioFOURCC('m', 'i', 'd', 's');
		public static readonly int streamtypeTEXT = mmioFOURCC('t', 'x', 't', 's');
		public static readonly int ICTYPE_VIDEO =mmioFOURCC('v', 'i', 'd', 'c');
		public static readonly int ICTYPE_AUDIO  =mmioFOURCC('a', 'u', 'd', 'c');
		public static readonly int ICM_FRAMERATE =mmioFOURCC('F','r','m','R');
		public static readonly int ICM_KEYFRAMERATE =mmioFOURCC('K','e','y','R');

		public static Int32 mmioFOURCC(char ch0, char ch1, char ch2, char ch3) 
		{
			return ((Int32)(byte)(ch0) | ((byte)(ch1) << 8) |
				((byte)(ch2) << 16) | ((byte)(ch3) << 24));
		}
	}

	/// <summary>
	/// constants for the biCompression field
	/// </summary>
	public enum BI
	{
		BI_RGB ,
		BI_RLE8,
		BI_RLE4  ,
		BI_BITFIELDS  ,
		BI_JPEG      ,
		BI_PNG       ,
	}

	/// <summary>
	/// Flags for index
	/// </summary>
	public enum AVIIF
	{
		AVIIF_LIST         = 0x00000001, // chunk is a 'LIST'
		AVIIF_KEYFRAME     = 0x00000010, // this frame is a key frame.
		AVIIF_FIRSTPART    = 0x00000020, // this frame is the start of a partial frame.
		AVIIF_LASTPART     = 0x00000040, // this frame is the end of a partial frame.
		AVIIF_MIDPART      =0x00000040|0x00000020, //(AVIIF_LASTPART|AVIIF_FIRSTPART)
		AVIIF_NOTIME	  =  0x00000100, // this frame doesn't take any time
		AVIIF_COMPUSE      = 0x0FFF0000 // these bits are for compressor use
	}

    /// <summary>
    /// 
    /// </summary>
	[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)]
	public class ICINFO
	{ 
		public int dwSize; 
		public int fccType; 
		public int fccHandler; 
		public int dwFlags; 
		public int dwVersion; 
		public int dwVersionICM; 
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=16)]
		public string szName; 
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string szDescription; 
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
		public string szDriver; 
	}  

    /// <summary>
    /// COMPVARS对象。如果你要手动设置这个结构,你必须提供如下成员的值:cbSize, hic, lpbiOut, lKey, and lQ.还有dwFlags为ICMF_COMPVARS_VALID
    /// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class COMPVARS
	{ 
        /// <summary>
        /// 必须设置该值为一个正确的值.
        /// </summary>
		public	int         cbSize; 
        /// <summary>
        /// 如果你使用ICCompressorChoose 函数来初始化结构,请不要设置这个值.
        /// </summary>
		public	int        dwFlags; 
        /// <summary>
        /// 压缩的句柄,你可以使用ICOpen去获得一个句柄
        /// </summary>
		public	int          hic; 
        /// <summary>
        /// 可以设置为zero
        /// </summary>
		public	int        fccType; 
        /// <summary>
        /// 四个字符的压缩引擎
        /// </summary>
		public	int        fccHandler; 
        /// <summary>
        /// 保留
        /// </summary>
		public	IntPtr lpbiIn; 
        /// <summary>
        /// BITMAPINFO结构的指针,包含了输出图象格式,也可以通过使用函数
        /// </summary>
		public	IntPtr lpbiOut; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int       lpBitsOut; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int       lpBitsPrev; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int         lFrame; 
        /// <summary>
        /// 关键帧速率 ICSeqCompressFrameStart 函数使用这个值来创建关键帧
        /// </summary>
		public	int         lKey; 
        /// <summary>
        /// 数据速率,可以通过ICCompressorChoose设置
        /// </summary>
		public	int         lDataRate; 
        /// <summary>
        /// 质量设置.可以使用ICQUALITY_DEFAULT 默认. ICSeqCompressFrameStart 函数使用这个值来产生数据质量
        /// </summary>
		public	int         lQ; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int         lKeyCount; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int       lpState; 
        /// <summary>
        /// 保留
        /// </summary>
		public	int         cbState; 
	}  
	
    /// <summary>
    /// 
    /// </summary>
	[System.Flags]
	public enum ICDRAWFlag
	{ 
		ICDRAW_QUERY       = 0x00000001,   // test for support
		ICDRAW_FULLSCREEN  = 0x00000002,   // draw to full screen
		ICDRAW_HDC         = 0x00000004,   // draw to a HDC/HWND
		ICDRAW_ANIMATE	   = 0x00000008,	  // expect palette animation
		ICDRAW_CONTINUE	   = 0x00000010,	  // draw is a continuation of previous draw
		ICDRAW_MEMORYDC	   = 0x00000020,	  // DC is offscreen, by the way
		ICDRAW_UPDATING	   = 0x00000040,	  // We're updating, as opposed to playing
		ICDRAW_RENDER      = 0x00000080,   // used to render data not draw it
		ICDRAW_BUFFER      = 0x00000100   // please buffer this data offscreen, we will need to update it
	}


	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	internal struct ICDRAWBEGIN
	{
		public	ICDRAWFlag               dwFlags;        // flags

		public	IntPtr            hpal;           // palette to draw with
		public	IntPtr                hwnd;           // window to draw to
		public	IntPtr                 hdc;            // HDC to draw to

		public	int                 xDst;           // destination rectangle
		public	int                 yDst;
		public	int                 dxDst;
		public	int                 dyDst;

		public	IntPtr  lpbi;           // format of frame to draw

		public	int                 xSrc;           // source rectangle
		public	int                 ySrc;
		public	int                 dxSrc;
		public	int                 dySrc;

		public	int               dwRate;         // frames/second = (dwRate/dwScale)
		public	int               dwScale;

	} 
	#endregion

    #region SendMessageClass
    public class SendMessageClass
	{
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			int lParam
			);	
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			IntPtr lParam
			);	
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			StringBuilder lParam
			);	
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			int wParam,
			string lParam
			);	
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			bool wParam,
			string lParam
			);	
		[DllImport("USER32.DLL")]
		public static extern int SendMessage(	
			IntPtr hwnd, 
			int wMsg,
			bool wParam,
			int lParam
			);
    }
    #endregion

    #region AV异常参数类
    /// <summary>
    /// AV异常参数类
    /// </summary>
    public class AVException : System.Exception
    {
        int m_num = 0;
        public AVException() : base() { }
        public AVException(int er) : base() { this.m_num = er; }
        public AVException(string message) : base(message) { }
        public AVException(string message, int er) : base(message) { this.m_num = er; }
        public AVException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int ErrorNumber
        {
            get { return m_num; }
        }
    }
    #endregion
}
