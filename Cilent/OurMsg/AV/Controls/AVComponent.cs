using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using IMLibrary3;

using StreamCoders.Decoder;
using StreamCoders.Encoder;
using StreamCoders.Devices;


namespace IMLibrary.AV.Controls
{
    /// <summary>
    /// AV组件
    /// </summary>
    public partial class AVComponent : Component
    {
        /// <summary>
        /// 构造
        /// </summary>
        public AVComponent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="container"></param>
        public AVComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #region 事件
        /// <summary>
        /// 获得IPEndPoint事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="local">本地主机信息</param>
        /// <param name="remote">远程主机信息</param>
        public delegate void GetIPEndPointEventHandler(object sender, IPEndPoint local, IPEndPoint remote);
        public event GetIPEndPointEventHandler GetIPEndPoint;
        protected virtual void OnGetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            if (GetIPEndPoint != null)
                GetIPEndPoint(this, local, remote);//触发获取本机主机事件
        }

        public delegate void TransmitEventHandler(object sender, ConnectedType connectedType);
        /// <summary>
        /// 文件传输成功联接到服务器事件
        /// </summary>
        public event TransmitEventHandler TransmitConnected;
        /// <summary>
        /// 触发文件传输成功联接到服务器事件
        /// </summary>
        protected virtual void OnTransmitConnected(ConnectedType connectedType)
        {
            if (TransmitConnected != null)
                TransmitConnected(this, connectedType);
        }

        /// <summary>
        /// 捕获到视频数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void VideoCaptureredEventHandler(object sender, IMLibrary.AV.BITMAPINFO bitmapinfo);

        /// <summary>
        /// 视频捕获前事件
        /// </summary>
        public event VideoCaptureredEventHandler VideoCapturerBefore;
        #endregion

        #region 变量
        /// <summary>
        /// 标识是否已经设置过AV组件
        /// </summary>
        private bool IsIni = false;

        /// <summary>
        /// 初始化本地视频控件
        /// </summary>
        Control cLocal=new Control();
        /// <summary>
        /// 初始化远程视频控件
        /// </summary>
        Control cRemote = new Control();
        /// <summary>
        /// 初始化麦克风控制控件
        /// </summary>
        TrackBar trackBarIn = new TrackBar();
        /// <summary>
        /// 初始化声卡声音控制控件
        /// </summary>
        TrackBar trackBarOut = new TrackBar();

        /// <summary>
        /// 视频捕捉组件
        /// </summary>
        VideoCapturer VC = null;
        /// <summary>
        /// 视频回显组件
        /// </summary>
        VideoRender VR = null;
        /// <summary>
        /// 视频编码器
        /// </summary>
        VideoEncoder VE  = null;
        /// <summary>
        /// 视频解码器
        /// </summary>
        VideoEncoder VD = null;

        /// <summary>
        /// 音频捕捉组件
        /// </summary>
        AudioCapturer AC = null;
        /// <summary>
        /// 音频回显组件
        /// </summary>
        AudioRender AR = null;
        /// <summary>
        /// 音频编解码器
        /// </summary>
        AudioEncoder AE = null;

        /// <summary>
        /// 侦通信对像
        /// </summary>
        public IMLibrary.AV.FrameTransmit frameTransmit = null;

        #region 视频相关
        /// <summary>
        /// 位图 
        /// </summary>
        Bitmap canvasBitmap;
        /// <summary>
        /// 绘图操作
        /// </summary>
        Graphics gfx = null;
        /// <summary>
        /// 摄像头
        /// </summary>
        CamCapture cam;
        /// <summary>
        /// 摄像信息
        /// </summary>
        CamMetrics metrics;
        /// <summary>
        /// 驱动信息
        /// </summary>
        List<CamMetrics> selectedDeviceMetrics;
        /// <summary>
        /// 编码器
        /// </summary>
        StreamCoders.Encoder.VideoEncoderBase encoder;
        /// <summary>
        /// 解码器
        /// </summary>
        StreamCoders.Decoder.VideoDecoderBase decoder;
        #endregion

        
        #endregion

        #region 公共方法

        #region 设置AV基本控制单元
        /// <summary>
        /// 设置AV基本控制单元
        /// </summary>
        /// <param name="local">本地视频控件</param>
        /// <param name="remote">远程视频控件</param>
        /// <param name="trackBarOut">声音音量调制器</param>
        /// <param name="trackBarIn">话筒音量调制器</param>
        public void SetControls(Control local, Control remote, TrackBar trackBarOut, TrackBar trackBarIn)
        {
            this.cLocal = local;//初始化本地视频控件
            this.cRemote = remote;//初始化远程视频控件
            this.trackBarIn = trackBarIn;//初始化麦克风控制控件
            this.trackBarOut = trackBarOut;//初始化声卡声音控制控件
        }
        /// <summary>
        /// 设置AV基本控制单元
        /// </summary>
        /// <param name="local">本地视频控件</param>
        /// <param name="remote">远程视频控件</param>
        public void SetControls(Control local, Control remote )
        {
            this.cLocal = local;//初始化本地视频控件
            this.cRemote = remote;//初始化远程视频控件
        }
        #endregion

        #region 初始化音视频捕获资源
        /// <summary>
        /// 初始化音视频通信组件
        /// </summary>
        /// <param name="Model">视频显示大小模式</param>
        public void iniAV(VideoSizeModel Model, System.Net.IPEndPoint ServerEP)
        {
            if (!IsIni)
                IsIni = true;//标识已经初始化
            else
                return; //如果已经初始化，则退出

            VideoSize.SetModel(Model);//设置视频编码尺寸

            if (cam == null)
                iniVideoCapturer();

            #region //创建新的视频捕捉组件
            //if (VC == null)
            //{
            //    VC = new  VideoCapturer(this.cLocal);
            //    VC.VideoCapturerBefore += new VideoCapturer.VideoCaptureredEventHandler(VC_VideoCapturerBefore);
            //    VC.VideoDataCapturered += new VideoCapturer.VideoCaptureredEventHandler(VC_VideoDataCapturered);
            //    VC.StartVideoCapture(Model);//开始捕捉视频
            //}
            #endregion

            if (AE == null)
            {
                AE = new AudioEncoder();//创建G711音频编解码器
            }

            if (AC == null)
            {
                AC = new AudioCapturer(this.trackBarOut, this.trackBarIn);//创建新的音频捕捉组件
                AC.AudioDataCapturered += new AudioCapturer.AudioDataCaptureredEventHandler(AC_AudioDataCapturered);
            }

            if (AR == null)
            {
                AR = new AudioRender();//创建G711音频回放组件
            }

            if (frameTransmit == null)
            {
                frameTransmit = new FrameTransmit(ServerEP);
                frameTransmit.GetIPEndPoint += new FrameTransmit.GetIPEndPointEventHandler(frameTransmit_GetIPEndPoint);
                frameTransmit.RecAudioData += new FrameTransmit.RecDataEventHandler(frameTransmit_RecAudioData);
                frameTransmit.RecVideoData += new FrameTransmit.RecDataEventHandler(frameTransmit_RecVideoData);
                frameTransmit.TransmitConnected += new FrameTransmit.TransmitEventHandler(frameTransmit_TransmitConnected);
                frameTransmit.Start();
            }
        }
        #endregion

        #region 设置对方视频格式事件
        /// <summary>
        /// 设置对方视频格式事件
        /// </summary>
        /// <param name="BITMAPINFO"></param>
        public void SetRemoteBITMAPINFOHEADER(BITMAPINFO BITMAPINFO)
        {
            if (VD == null)
                VD = new VideoEncoder(BITMAPINFO, false);//创建视频解码器

            if (VR == null)
            {
                VR = new VideoRender(this.cRemote);//创建视频回显组件
                VR.IniVideoRender(BITMAPINFO.bmiHeader);
            }
        }
        #endregion

        #endregion

        #region 关闭
        /// <summary>
        /// 关闭 
        /// </summary>
        public void Close()
        {
            if (frameTransmit != null)
            {
                frameTransmit.Dispose();
                frameTransmit = null;
            }
            if (VC != null)
            {
                VC.Close();
                VC = null;
            }
            if (VE != null)
            {
                VE.Close();
                VE = null;
            }
            if (VD != null)
            {
                VD.Close();
                VD = null;
            }
            if (VR != null)
            {
                VR = null;
            }
            if (AC != null)
            {
                AC.Close();
                AC = null;
            }
            if (AE != null)
            {
                AE.Close(); AE = null;
            }
            if (AR != null)
            {
                AR.Close(); AE = null;
            }
            if (cam != null)
            {
                cam.Stop(); cam.Dispose();
                cam = null;
                timer1.Stop();
            }
            //cLocal.Dispose(); cLocal = null;
            //cRemote.Dispose(); cRemote = null;
            //trackBarIn.Dispose(); trackBarIn = null;
            //trackBarOut.Dispose(); trackBarOut = null;
        }
        #endregion

        #region 音视频捕捉事件
        /// <summary>
        /// 捕捉到音频数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AC_AudioDataCapturered(object sender, AudioCapturedEventArgs e)
        {
            if (AE != null)
                frameTransmit.sendData(IMLibrary3.Protocol.TransmitType.Audio, AE.Encode(e.Data));//将音频数据编码后发送给对方
        }

        /// <summary>
        /// 视频捕捉前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VC_VideoCapturerBefore(object sender, VideoCapturedEventArgs e)
        {
            //Console.WriteLine("biSize:" + e.BITMAPINFO.bmiHeader.biSize.ToString() + " biSizeImage:" + e.BITMAPINFO.bmiHeader.biSizeImage.ToString()
            //                  + " biBitCount:" + e.BITMAPINFO.bmiHeader.biBitCount.ToString() + " biWidth:" + e.BITMAPINFO.bmiHeader.biWidth.ToString()
            //                  + " biHeight:" + e.BITMAPINFO.bmiHeader.biHeight.ToString()
            //                  + " biClrUsed:" + e.BITMAPINFO.bmiHeader.biClrUsed.ToString() + "  biClrImportant:" + e.BITMAPINFO.bmiHeader.biClrImportant.ToString()
            //                  + " biXPelsPerMeter:" + e.BITMAPINFO.bmiHeader.biXPelsPerMeter.ToString() + " biYPelsPerMeter:" + e.BITMAPINFO.bmiHeader.biYPelsPerMeter.ToString()
            //                  + " biPlanes:" + e.BITMAPINFO.bmiHeader.biPlanes.ToString() + " biCompression:" + e.BITMAPINFO.bmiHeader.biCompression.ToString());
            if (VideoCapturerBefore != null)
                VideoCapturerBefore(this, e.BITMAPINFO);

            if (VE == null)
                VE = new VideoEncoder(e.BITMAPINFO, true);//根据摄像头采集数据的格式，创建新的视频编码器
        } 

        /// <summary>
        /// 捕捉到视频数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VC_VideoDataCapturered(object sender, VideoCapturedEventArgs e)
        {
            if (VE != null) 
                frameTransmit.sendData(IMLibrary3.Protocol.TransmitType.Video, VE.Encode(e.Data));//将视频数据编码后发送给对方
        }
        #endregion

        #region 通信事件

        //联接事件
        private void frameTransmit_TransmitConnected(object sender, IMLibrary3.ConnectedType connectedType)
        {
            OnTransmitConnected(connectedType);//触发联系事件
        }

        //收到视频数据
        private void frameTransmit_RecVideoData(object sender, byte[] data)
        {
            Console.WriteLine("RecVideoData:" + data.Length.ToString());

            //if (this.VD != null && this.VR != null)
            //{
            //    this.VR.DrawVideo(this.VD.Decode(data));//将收到的视频数据解码后回显
            //}

            VideoSize.SetModel(VideoSizeModel.W320_H240);
            if (data.Length > 0)
            {
                Byte[] localDecodeArray = decoder.Decode(data);
                if (localDecodeArray != null)
                {
                    Console.WriteLine(localDecodeArray.Length.ToString());
                    Bitmap pBmp = StreamCoders.Tools.Visuals.ByteArrayToBitmap_RGB24(VideoSize.Width, VideoSize.Height, localDecodeArray);
                    using (Graphics pGfx = cRemote.CreateGraphics())
                    {
                        pGfx.DrawImage(pBmp, 0, 0, cRemote.Width, cRemote.Height);
                    }
                }
            }
        }

        //收到音频数据
        private void frameTransmit_RecAudioData(object sender, byte[] data)
        {
            if (this.AE != null && this.AR != null)
            {
                this.AR.play(this.AE.Decode(data));//将收到的音频数据解码后播放
            }
        }

        //获得IP事件
        private void frameTransmit_GetIPEndPoint(object sender, System.Net.IPEndPoint local, System.Net.IPEndPoint remote)
        {
            if (GetIPEndPoint != null)
                GetIPEndPoint(this, local, remote);
        }
        #endregion

        #region 初始化视频捕捉器
        /// <summary>
        /// 初始化视频捕捉器
        /// </summary>
        private void iniVideoCapturer()
        {
            canvasBitmap = new Bitmap(VideoSize.Width, VideoSize.Height);
            using (Graphics g = Graphics.FromImage(canvasBitmap))
                g.FillRectangle(Brushes.Bisque, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            gfx = cLocal.CreateGraphics();
            gfx.DrawImage(canvasBitmap, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            cam = new CamCapture();

            var deviceList = cam.GetDeviceList();//获得驱动列表
            selectedDeviceMetrics = cam.SelectDevice(deviceList[0]);//获得尺寸大小
            cam.SelectMetrics(selectedDeviceMetrics[0]);
            bool res = cam.Start();//开始捕捉视频
            if (res == false)
            {
                //IMLibrary3.Global.MsgShow("没有可用的摄像头。");
            }
            metrics = cam.Metrics;//获得尺寸

            encoder = new StreamCoders.Encoder.H264Encoder();
            encoder.Framerate = 11;
            encoder.SetInputResolution((uint)VideoSize.Width, (uint)VideoSize.Height);
            encoder.IFrameFrequency = 40;
            encoder.Bitrate = 128000;
            encoder.PayloadType = 103;
            res = encoder.Init();
            if (res == false)
            {
                //IMLibrary3.Global.MsgShow("无法初始化视频编码器。");
                return;
            } 
            byte[] zeroBuffer = new byte[VideoSize.Width * VideoSize.Height * 3];
            byte[] initData = encoder.EncodeToArray(zeroBuffer);

            decoder = new StreamCoders.Decoder.H264Decoder();
            decoder.SetInputResolution((uint)VideoSize.Width, (uint)VideoSize.Height);
            decoder.SetInputFrameRate(11);
            res = ((StreamCoders.Decoder.H264Decoder)decoder).Init(initData);
            if (res == false)
            {
                //IMLibrary3.Global.MsgShow("无法初始化视频解码器。");
                return;
            }

            timer1.Interval = (int)(1000.0 / 25);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cam != null)
            {
                byte[] buffer = cam.GetFrame();
                if (buffer == null)
                    return;
                Bitmap pBmp = StreamCoders.Tools.Visuals.ByteArrayToBitmap_RGB24((uint)metrics.Width, (uint)metrics.Height, buffer);
                //Console.WriteLine("metrics.Width:" + metrics.Width.ToString()+ "metrics.Height:" + metrics.Height.ToString());
                pBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    g.DrawImage(pBmp, 0, 0, canvasBitmap.Width, canvasBitmap.Height);
                    //g.DrawImage(pBmp, 0, 0, cLocal.Width,cLocal.Height);
                    gfx.DrawImage(canvasBitmap, new Point(0, 0));
                }
            }

            Bitmap t1 = new Bitmap(canvasBitmap, new Size(VideoSize.Width, VideoSize.Height));
            Bitmap temp = t1.Clone(new RectangleF(0, 0, VideoSize.Width, VideoSize.Height), PixelFormat.Format24bppRgb);
            Rectangle dimension = new Rectangle(0, 0, temp.Width, temp.Height);
            BitmapData picData = temp.LockBits(dimension, ImageLockMode.ReadWrite, temp.PixelFormat);
            PixelFormat format = temp.PixelFormat;
            IntPtr pixelStartAddress = picData.Scan0;
            Byte[] rawData = new Byte[picData.Stride * VideoSize.Height];
            System.Runtime.InteropServices.Marshal.Copy(pixelStartAddress, rawData, 0, rawData.Length);
            temp.UnlockBits(picData);

            Byte[] asmFrame = encoder.EncodeToArray(rawData);
            if (asmFrame != null)
                frameTransmit.sendData(IMLibrary3.Protocol.TransmitType.Video, asmFrame);//将视频数据编码后发送给对方
        }
        #endregion
    }
}
