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
    #region 视频编解码器基类
    /// <summary>
	/// Compress 的摘要说明。
	/// </summary>
	public class ICBase
	{
		protected int hic;
		protected ICMODE mode;
		protected BITMAPINFO _in;
		protected BITMAPINFO _out;
		protected COMPVARS pp;
		protected int fourcc;
		protected COMPVARS Compvars
		{
			get{return this.pp;}
		}
		public ICBase(COMPVARS cp,BITMAPINFO biIn,ICMODE mode,int fourcc)
		{
			this.pp=cp;
			this._in=biIn;
			this.mode=mode;
			this.fourcc=fourcc;
			_out=new BITMAPINFO();

        }

		#region
		public int HIC
		{
			get{return this.hic;}
		}
		public BITMAPINFO InFormat
		{
			get{return this._in;}
		}
		public BITMAPINFO OutFormat
		{
			get{return this._out;}
		}
		public ICMODE Mode
		{
			get{return this.mode;}
		}
		public int Fourcc
		{
			get{return this.fourcc;}
		}
		#endregion

		public virtual void Open()
		{
            this.hic=ICOpen(FOURCC.ICTYPE_VIDEO,this.fourcc,this.mode); 
            this.Compvars.hic = this.hic;
		}

        public virtual byte[] Process(byte[] data)
        {
            return null;
        }

		public virtual void Close()
		{
			ICClose(hic);
		}
		
		#region  API 申明 
		
		[DllImport("MSVFW32.dll")]
		public static extern void ICSeqCompressFrameEnd(ref COMPVARS pc);
		[DllImport("MSVFW32.dll")]
		public static extern int ICCompressGetFormatSize(
			int hic,       
			ref BITMAPINFO lpbiInput  
			);
		
		[DllImport("MSVFW32.dll")]
		public static extern bool ICSeqCompressFrameStart(
			COMPVARS pc,        
			ref BITMAPINFO lpbiIn  
			);
		[DllImport("MSVFW32.dll")]
		public static extern int ICSeqCompressFrame(
			COMPVARS pc,  
			int uiFlags,  
			byte[] lpBits, 
			ref bool pfKey,  
			ref long plSize  
			);
		[DllImport("MSVFW32.dll",CharSet=CharSet.Ansi)]
		public static extern int ICGetInfo(
			int hic,            
			ICINFO lpicinfo,  
			int cb            
			);
		[DllImport("MSVFW32.dll")]
		public static extern bool ICCompressorChoose(
			IntPtr hwnd,      
			int uiFlags,   
			//ref BITMAPINFO pvIn,   
			int pvIn,
			int lpData,  
			COMPVARS pc,   
			string lpszTitle 
			);
		[DllImport("MSVFW32.dll")]
		public static extern int ICLocate(
			int fccType,              
			int fccHandler,           
			ref BITMAPINFOHEADER lpbiIn,  
			ref BITMAPINFOHEADER lpbiOut,  
			short wFlags                 
			);
		[DllImport("MSVFW32.dll"),PreserveSig]
		public static extern int ICOpen(int fccType,int fccHandler,ICMODE wMode); 
		[DllImport("MSVFW32.dll")]
		public static extern int ICClose(int hic);
		[DllImport("MSVFW32.dll")]
		public static extern int ICCompress(
			int hic,
			int dwFlags,        // flags
			ref BITMAPINFOHEADER lpbiOutput,     // output format
			IntPtr lpData,         // output data
			ref BITMAPINFOHEADER  lpbiInput,      // format of frame to compress
			IntPtr lpBits,         // frame data to compress
			int lpckid,         // ckid for data in AVI file
			int lpdwFlags,      // flags in the AVI index.
			int lFrameNum,      // frame number of seq.
			int dwFrameSize,    // reqested size in bytes. (if non zero)
			int dwQuality,      // quality within one frame
			//	BITMAPINFOHEADER  lpbiPrev,       // format of previous frame
			int  lpbiPrev,       // format of previous frame
			int lpPrev          // previous frame
			);
		[DllImport("MSVFW32.dll")]
		public static extern int ICDecompress(
			int hic,                        
			int dwFlags,                  
			ref BITMAPINFOHEADER lpbiFormat,  
			byte[] lpData,                  
			ref BITMAPINFOHEADER lpbi    ,    
			byte[] lpBits                   
			);
		[DllImport("MSVFW32.dll")]
		public static extern int ICSendMessage(int hic,int msg,ref BITMAPINFO dw1,ref BITMAPINFO dw2);
		[DllImport("MSVFW32.dll")]
		public static extern int ICSendMessage(int hic,int msg,int dw1,int dw2);
		[DllImport("MSVFW32.dll")]
		public static extern int ICSendMessage(int hic,int msg,ICINFO dw1,int dw2);
		public static readonly int DRV_USER                =0x4000;
		public static readonly int ICM_USER          =(DRV_USER+0x0000);
		public static readonly int ICM_COMPRESS_BEGIN          =(ICM_USER+7);    // begin a series of compress calls.
		public static readonly int ICM_COMPRESS                =(ICM_USER+8) ;   // compress a frame
		public static readonly int ICM_COMPRESS_END            =(ICM_USER+9) ;   // end of a series of compress calls.
		public static readonly int ICM_COMPRESS_GET_FORMAT     =(ICM_USER+4);
		public static readonly int ICM_DECOMPRESS_BEGIN        =(ICM_USER+12);   // start a series of decompress calls
		public static readonly int ICM_DECOMPRESS              =(ICM_USER+13);   // decompress a frame
		public static readonly int ICM_DECOMPRESS_END          =(ICM_USER+14);
		#endregion
    }
    #endregion

    #region 视频解编码器

    /// <summary>
    /// 视频编码器(压缩)
    /// </summary>
    public class ICCompressor:ICBase
	{
        /// <summary>
        /// 初始化视频编码器
        /// </summary>
        /// <param name="cp">压缩对像</param>
        /// <param name="biIn">图像信息</param>
        /// <param name="fourcc">编码类型</param>
        public ICCompressor(COMPVARS cp, BITMAPINFO biIn, int fourcc)
            : base(cp, biIn, ICMODE.ICMODE_COMPRESS, fourcc)
        {

        }

        /// <summary>
        /// 打开视频编码器，准备解码（压缩）
        /// </summary>
		public override void Open()
		{
			base.Open ();
			int r=ICSendMessage(hic,ICM_COMPRESS_GET_FORMAT,ref this._in,ref this._out);
			bool s=ICSeqCompressFrameStart(this.Compvars,ref this._in);
		}

        /// <summary>
        /// 视频编码（压缩）
        /// </summary>
        /// <param name="data">视频数据</param>
        /// <returns>返回已编码（压缩）数据</returns>
        public override byte[] Process(byte[] data)
        {
            if (this.hic == 0) 
                return data;

            try
            {
                bool key = false; long size = 0;

                lock (data)
                {
                    IntPtr r = (IntPtr)ICSeqCompressFrame(this.pp, 0, data,ref key, ref size);
                    byte[] b = new byte[size];
                    Marshal.Copy(r, b, 0, (int)size);
                    return b;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 关闭编码器
        /// </summary>
		public override void Close()
		{
			if(this.hic!=0)
			{
				try
				{
					ICSeqCompressFrameEnd(ref this.pp);
				}
				catch
				{
				}
			}
			base.Close();
		}
	}

    /// <summary>
    /// 视频解码器
    /// </summary>
	public class ICDecompressor:ICBase
	{
        /// <summary>
        /// 初始化视频解码器
        /// </summary>
        /// <param name="cp">压缩对像</param>
        /// <param name="biIn">图像信息</param>
        /// <param name="fourcc">编码类型</param>
        public ICDecompressor(COMPVARS cp, BITMAPINFO biIn, int fourcc)
            : base(cp, biIn, ICMODE.ICMODE_DECOMPRESS, fourcc)//ICMODE_DECOMPRESS
        {
        }

        /// <summary>
        /// 打开解码器，准备解码
        /// </summary>
		public override void Open()
		{
			base.Open ();
			int r=ICSendMessage(hic,ICM_USER+10,ref this._in,ref this._out);//get the output bitmapinfo
		    r=ICSendMessage(hic,ICM_DECOMPRESS_BEGIN,ref this._in,ref this._out);
		}

        /// <summary>
        /// 解码（解压缩数据）
        /// </summary>
        /// <param name="data">视频数据</param>
        /// <returns>返回已解码的数据</returns>
        public override byte[] Process(byte[] data)
        {
            if (this.hic == 0) return data;

            byte[] b = new byte[this._out.bmiHeader.biSizeImage];  
            try
            {
                int i = ICDecompress(this.hic, 0, ref this._in.bmiHeader,data, ref this._out.bmiHeader,b);
            }
            catch
            {
            }
            return b;
        }

        /// <summary>
        /// 关闭视频解码器
        /// </summary>
		public override void Close()
		{
			if(this.hic!=0)
			{
				ICSendMessage(hic,ICM_USER+14,0,0);//get the output bitmapinfo
			}
			base.Close ();
		}
    }

    #endregion
}
