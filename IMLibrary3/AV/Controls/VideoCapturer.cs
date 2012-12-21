using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IMLibrary.AV.Controls
{
    /// <summary>
    /// 视频捕获组件
    /// </summary>
    public class VideoCapturer
    {
        #region 初始化视频组件
        /// <summary>
        /// 初始化视频组件
        /// </summary>
        /// <param name="control"></param>
        public VideoCapturer(Control control)
        {
            this.controlVideo = control;
        }

        /// <summary>
        /// 初始化视频组件
        /// </summary>
        public VideoCapturer()
        { }
        #endregion

        #region 变量
        /// <summary>
        /// 视频捕获尺寸
        /// </summary>
        private VideoCapturerSize CapturerSize = new VideoCapturerSize(VideoSizeModel.W160_H120);

        /// <summary>
        /// 本地视频画刷
        /// </summary>
        private DrawDib drawDib = new DrawDib();

        /// <summary>
        /// 捕获视频的控件句柄
        /// </summary>
        private Control panel = new  Control();

        /// <summary>
        /// 用户自定义视频显示控件
        /// </summary>
        public  Control controlVideo = new Control();

        /// <summary>
        /// 视频显示 
        /// </summary>
        public IMLibrary.AV.VideoCapturer vCapturer;

        #endregion

        #region 事件

      /// <summary>
      /// 捕获到视频数据事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        public delegate void VideoCaptureredEventHandler(object sender, VideoCapturedEventArgs e);
       
        /// <summary>
        /// 捕获到视频数据事件
        /// </summary>
        public event VideoCaptureredEventHandler VideoDataCapturered;


        /// <summary>
        /// 视频捕获前事件
        /// </summary>
        public event VideoCaptureredEventHandler VideoCapturerBefore;

        #endregion

        #region 属性
        private VideoSizeModel _VideoCapturerSize = VideoSizeModel.W160_H120;
        /// <summary>
        /// 视频捕获尺寸
        /// </summary>
        public VideoSizeModel VideoCapturerSize
        {
            set
            {
                _VideoCapturerSize = value;
                CapturerSize.SetModel(value);
            }

            get { return _VideoCapturerSize; }
        }

        /// <summary>
        /// 视频图像信息
        /// </summary>
        public BITMAPINFO bitmapInfo
        {
            private set;
            get;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 开始视频捕获
        /// </summary>
        /// <param name="videoSizeModel"></param>
        public void StartVideoCapture(VideoSizeModel videoSizeModel)
        {
            CapturerSize.SetModel(videoSizeModel);
            StartVideoCapture();
        }
        /// <summary>
        /// 开始视频捕获
        /// </summary>
        private  void StartVideoCapture()
        {
            ///先将本地视频初始化为CapturerSize的高与宽
            this.vCapturer = new IMLibrary.AV.VideoCapturer(this.panel, 0);
            this.vCapturer.ConnectDevice();
            IMLibrary.AV.CaptureParms cp = this.vCapturer.CaptureParms;
            cp.fAbortLeftMouse = cp.fAbortRightMouse = false;
            cp.fYield = true;
            this.vCapturer.CaptureParms = cp;
            IMLibrary.AV.BITMAPINFO h = this.vCapturer.BITMAPINFO;

            //如果当前摄像头不为CapturerSize,先将本地视频初始化为CapturerSize
            if (!(h.bmiHeader.biWidth == this.CapturerSize.Width && h.bmiHeader.biHeight == this.CapturerSize.Height))
            {
                h.bmiHeader.biWidth =   this.CapturerSize.Width;
                h.bmiHeader.biHeight =  this.CapturerSize.Height;
                this.vCapturer.BITMAPINFO = h; 
            } 
            this.vCapturer.VideoCaptured += new VideoCaptureEventHandler(vc_VideoCaptured);
            this.vCapturer.SetPreviewRate(25);
            this.vCapturer.Preview = true;
            this.vCapturer.CaptureWithOutFile();

            this.bitmapInfo = vCapturer.BITMAPINFO;
            if (this.VideoCapturerBefore != null)
                this.VideoCapturerBefore(this, new VideoCapturedEventArgs(this.bitmapInfo));//触发视频捕获前事件

            //Console.WriteLine("biSize:" + bitmapInfo.bmiHeader.biSize.ToString() + " biSizeImage:" + bitmapInfo.bmiHeader.biSizeImage.ToString()
            //                  + " biBitCount:" + bitmapInfo.bmiHeader.biBitCount.ToString() + " biWidth:" + bitmapInfo.bmiHeader.biWidth.ToString()
            //                  + " biHeight:" + bitmapInfo.bmiHeader.biHeight.ToString()
            //                  + " biClrUsed:" + bitmapInfo.bmiHeader.biClrUsed.ToString() + "  biClrImportant:" + bitmapInfo.bmiHeader.biClrImportant.ToString()
            //                  + " biXPelsPerMeter:" + bitmapInfo.bmiHeader.biXPelsPerMeter.ToString() + " biYPelsPerMeter:" + bitmapInfo.bmiHeader.biYPelsPerMeter.ToString()
            //                  + " biPlanes:" + bitmapInfo.bmiHeader.biPlanes.ToString() + " biCompression:" + bitmapInfo.bmiHeader.biCompression.ToString());

            IniVideoRender();
        }
        #endregion

        #region 捕获视频事件
        /// <summary>
        /// 捕获视频事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hdr"></param>
        private void vc_VideoCaptured(object sender,VIDEOHDR hdr)
        {
            try
            {
                byte[] data = new byte[hdr.dwBytesUsed];
                Marshal.Copy(hdr.lpData, data, 0, hdr.dwBytesUsed);

                this.drawDib.Draw(data, this.controlVideo);

                if (this.VideoDataCapturered != null)
                    this.VideoDataCapturered(this, new VideoCapturedEventArgs(this.bitmapInfo, data));

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
        }

        //private delegate void delegateDrawControl(byte[] data);
        //private void drawControl(byte[] data)
        //{
        //    this.drawDib.Draw(data, this.controlVideo);
        //    if (this.VideoDataCapturered != null)
        //        this.VideoDataCapturered(this, new VideoCapturedEventArgs(this.bitmapInfo, data));
        //}
        #endregion

        #region 初始化视频画刷及显示控件
        /// <summary>
        /// 初始化视频画刷及显示控件
        /// </summary>
        private void IniVideoRender()
        {
            drawDib = new DrawDib(this.controlVideo);
            drawDib.BITMAPINFOHEADER = bitmapInfo.bmiHeader;
            drawDib.Open();
        }
        #endregion

        #region 关闭视频捕捉
        /// <summary>
        /// 关闭视频捕捉
        /// </summary>
        public void Close()
        {
            if (this.vCapturer != null)
            {
                this.vCapturer.Stop();
                this.vCapturer.Close();
                this.vCapturer.Disconnect();
                this.vCapturer = null;
                this.drawDib = null;
                this.panel = null;
                this.controlVideo = null;
            }
        }
        #endregion
    }

    #region 视频捕获事件参数
    /// <summary>
    /// 视频捕获事件参数
    /// </summary>
    public class VideoCapturedEventArgs : System.EventArgs
    {
        /// <summary>
        /// 视频图像信息
        /// </summary>
        public BITMAPINFO BITMAPINFO;

        /// <summary>
        /// 捕获到的数据
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 初始化事件参数
        /// </summary>
        public VideoCapturedEventArgs()
        {
        }

        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="BITMAPINFO">捕获的图像头信息</param>
        /// <param name="data">捕获的数据</param>
        public VideoCapturedEventArgs(BITMAPINFO BITMAPINFO, byte[] data)
        {
            this.BITMAPINFO = BITMAPINFO;
            this.Data = data;
        }

        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="BITMAPINFO">捕获的图像头信息</param>
        public VideoCapturedEventArgs(BITMAPINFO BITMAPINFO)
        {
            this.BITMAPINFO = BITMAPINFO;
        }
    }
    #endregion
}
