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
    /// <summary>
    /// DrawDib 的摘要说明。
    /// </summary>
    public class DrawDib
    {
        /// <summary>
        /// 初始化视频画刷
        /// </summary>
        public DrawDib()
        {
        }

        /// <summary>
        /// 初始化画刷控件
        /// </summary>
        /// <param name="Control">绘制控件</param>
        public DrawDib(Control Control)
        {
            this.Control = Control;
        }

        #region 变量

        /// <summary>
        /// 非托管指针
        /// </summary>
        private IntPtr hdd;

        /// <summary>
        /// 图像信息
        /// </summary>
        public BITMAPINFOHEADER  BITMAPINFOHEADER = new BITMAPINFOHEADER();

        /// <summary>
        /// 要显示图像的控件
        /// </summary>
        public Control Control=new Control();
        
        #endregion

        #region 属性
        ///// <summary>
        ///// 图像信息
        ///// </summary>
        //public BITMAPINFOHEADER BITMAPINFOHEADER
        //{
        //    set { _BITMAPINFOHEADER = value;}
        //    get { return _BITMAPINFOHEADER; }
        //}

        /// <summary>
        /// 显示图像的控件句柄
        /// </summary>
        public IntPtr Handle
        {
            get { return this.hdd; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOpened
        {
            get { return this.hdd != IntPtr.Zero; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开图像画刷句柄
        /// </summary>
        public void Open()
        {
            this.hdd = DrawDibOpen();
            Debug.Assert(hdd != IntPtr.Zero);
            DrawDibBegin(hdd, IntPtr.Zero, this.Control.Width, this.Control.Height, ref  BITMAPINFOHEADER,  BITMAPINFOHEADER.biWidth, BITMAPINFOHEADER.biHeight, 0);
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="data">图像数据</param>
        /// <param name="control">要显示图像控件</param>
        public void Draw(byte[] data, Control control)
        {
            try
            {
                using (Graphics g = control.CreateGraphics())
                {
                    IntPtr hdc = g.GetHdc();
                    bool b = DrawDibDraw(
                        hdd,
                        hdc,
                        0,
                        0,
                        Control.Width,
                        Control.Height,
                        ref  BITMAPINFOHEADER,
                        data,
                        0,
                        0,
                        BITMAPINFOHEADER.biWidth,
                        BITMAPINFOHEADER.biHeight,
                        0);
                    g.ReleaseHdc(hdc);
                }
            }
            catch { }
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="data">图像数据</param>
        public void Draw(byte[] data)
        {
            try
            {
                using (Graphics g = Control.CreateGraphics())
                {
                    IntPtr hdc = g.GetHdc();
                    bool b = DrawDibDraw(
                        hdd,
                        hdc,
                        0,
                        0,
                        Control.Width,
                        Control.Height,
                        ref BITMAPINFOHEADER,
                        data,
                        0,
                        0,
                        BITMAPINFOHEADER.biWidth,
                        BITMAPINFOHEADER.biHeight,
                        0);
                    g.ReleaseHdc(hdc);
                }
            }
            catch { }
        }

        /// <summary>
        /// 关闭画刷
        /// </summary>
        public void Close()
        {
            if (hdd != IntPtr.Zero)
            {
                DrawDibEnd(hdd);
                DrawDibClose(hdd);
            }
        }
        #endregion

        #region API申明
        /*
		**  DrawDibOpen()
		**
		*/
        [DllImport("MSVFW32.dll")]
        public static extern IntPtr DrawDibOpen();

        /*
        **  DrawDibClose()
        **
        */
        [DllImport("MSVFW32.dll")]
        public static extern bool DrawDibClose(IntPtr hdd);

        [DllImport("MSVFW32.dll")]
        public static extern bool DrawDibBegin(
            IntPtr hdd,
            IntPtr hdc,
            int dxDst,
            int dyDst,
            ref BITMAPINFOHEADER lpbi,
            int dxSrc,
            int dySrc,
            int wFlags
            );
        [DllImport("MSVFW32.dll")]
        public static extern bool DrawDibEnd(IntPtr hdd);
        /*
        **  DrawDibDraw()
        **
        **  actualy draw a DIB to the screen.
        **
        */
        [DllImport("MSVFW32.dll")]
        public static extern bool DrawDibDraw(
            IntPtr hdd,
            IntPtr hdc,
            int xDst,
            int yDst,
            int dxDst,
            int dyDst,
            ref BITMAPINFOHEADER lpbi,
            byte[] lpBits,
            int xSrc,
            int ySrc,
            int dxSrc,
            int dySrc,
            uint wFlags
            );
        #endregion
    }
}