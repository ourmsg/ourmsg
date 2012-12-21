using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary.AV
{
    /// <summary>
    /// 视频回显组件(本组件将视频数据绘制成图像显示在指定的控件)
    /// </summary>
    public class VideoRender
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public VideoRender()
        {
            IniVideoRender();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="control">显示视频的控件</param>
        public VideoRender(Control control)
        {
            controlVideo = control;
            IniVideoRender();
        }

        #region 变量区

        /// <summary>
        /// 显示视频的控件
        /// </summary>
        private Control controlVideo = new Control();

        /// <summary>
        /// 视频画刷
        /// </summary>
        private DrawDib drawDib = new DrawDib();

        #endregion

        #region 属性
        /// <summary>
        /// 视频图像信息
        /// </summary>
        public BITMAPINFOHEADER BITMAPINFOHEADER
        {
            set
            {
                drawDib.BITMAPINFOHEADER = value;
                drawDib.BITMAPINFOHEADER.biCompression = (int)BI.BI_RGB;
                drawDib.BITMAPINFOHEADER.biBitCount = 24;
            }
            get { return drawDib.BITMAPINFOHEADER; }
        }
        #endregion

        #region 视频回显方法
        /// <summary>
        /// 视频回显（绘制视频图像）
        /// </summary>
        /// <param name="data">视频数据</param>
        public void DrawVideo(byte[] data)
        {
            this.drawDib.Draw(data, this.controlVideo);
        }
        #endregion

        #region 初始化视频画刷及显示控件
        /// <summary>
        /// 初始化视频画刷及显示控件
        /// </summary>
        public void IniVideoRender(BITMAPINFOHEADER BITMAPINFOHEADER)
        {
            drawDib = new DrawDib(this.controlVideo);
            drawDib.BITMAPINFOHEADER = BITMAPINFOHEADER;
            //drawDib.BITMAPINFOHEADER.biCompression = BITMAPINFOHEADER.biCompression;// 已修改 原(int)BI.BI_RGB;
            //drawDib.BITMAPINFOHEADER.biBitCount = BITMAPINFOHEADER.biBitCount;//已修改  原24
            drawDib.Open();
        }

        /// <summary>
        /// 初始化视频画刷及显示控件
        /// </summary>
        public void IniVideoRender()
        {
            drawDib = new DrawDib(this.controlVideo);
            drawDib.BITMAPINFOHEADER = BITMAPINFOHEADER;
            drawDib.BITMAPINFOHEADER.biCompression = (int)BI.BI_RGB;
            drawDib.BITMAPINFOHEADER.biBitCount = 24;
            drawDib.Open();
        }
        #endregion

    }
}
 
