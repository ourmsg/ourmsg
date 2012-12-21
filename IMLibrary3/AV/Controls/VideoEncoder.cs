using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{
    /// <summary>
    /// 视频编解码器
    /// </summary>
    public class VideoEncoder
    {
        /// <summary>
        /// 视频编码器 
        /// </summary>
        private ICCompressor Compressor;

        /// <summary>
        /// 视频解码器
        /// </summary>
        private ICDecompressor Decompressor;

        /// <summary>
        /// 标识当前采用的是编码还是解码功能
        /// </summary>
        private bool IsEncode = true;

        //private COMPVARS compvars = null;

       /// <summary>
       /// 初始化视频编解码器
       /// </summary>
       /// <param name="bitmapInfoHeader">图像头信息</param>
       /// <param name="isEncode">标识完成编码还是解码功能</param>
        public VideoEncoder(BITMAPINFO bitmapInfo, bool isEncode)
        {
            //BITMAPINFO bitmapInfo = new BITMAPINFO();
            //bitmapInfo.bmiHeader = bitmapInfoHeader;
           
            
            this.IsEncode = isEncode;
            if (isEncode)
            {
                COMPVARS compvars = new COMPVARS();
                compvars.cbSize = Marshal.SizeOf(compvars);
                compvars.dwFlags = 1;
                compvars.fccHandler = FOURCC.MP42;
                compvars.fccType = FOURCC.ICTYPE_VIDEO;
                compvars.lDataRate = 780;// 780;
                compvars.lKey = 15;// 15;
                compvars.lQ = 500;// -1;
                //compvars.lQ = 500;

                this.Compressor = new ICCompressor(compvars, bitmapInfo, FOURCC.MP42);
                this.Compressor.Open();//打开编码器
            }
            else
            {
                bitmapInfo.bmiHeader.biCompression = FOURCC.MP42;
                this.Decompressor = new ICDecompressor(new COMPVARS(), bitmapInfo, FOURCC.MP42);
                this.Decompressor.Open();
            }
        }

        /// <summary>
        /// 视频编码(视频压缩)
        /// </summary>
        /// <param name="data">要压缩的数据</param>
        public byte[] Encode(byte[] data)
        {
           return this.Compressor.Process(data);
        }

        /// <summary>
        /// 视频解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decode(byte[] data)
        {
            return this.Decompressor.Process(data);
        }

        /// <summary>
        /// 关闭视频编解码器
        /// </summary>
        public void Close()
        {
            if (IsEncode)
                this.Compressor.Close();
            else
                this.Decompressor.Close();
        }
    }
}
