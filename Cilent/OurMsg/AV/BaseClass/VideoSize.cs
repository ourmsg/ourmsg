using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.AV
{
    /// <summary>
    /// 视频模式
    /// </summary>
    public enum VideoSizeModel
    {
        /// <summary>
        /// 捕获视频大小宽为160，高120像素
        /// </summary>
        W160_H120,
        /// <summary>
        /// 捕获视频大小宽为176，高144像素
        /// </summary>
        W176_H144,
        /// <summary>
        /// 捕获视频大小宽为320，高240像素
        /// </summary>
        W320_H240,
        /// <summary>
        /// 捕获视频大小宽为352，高288像素
        /// </summary>
        W352_H288,
        /// <summary>
        /// 捕获视频大小宽为640，高480像素
        /// </summary>
        W640_H480,
        /// <summary>
        /// 捕获视频大小宽为800，高600像素
        /// </summary>
        W800_H600,
    }


    /// <summary>
    /// 视频显示尺寸大小 
    /// </summary>
    public sealed class VideoSize
    {
        /// <summary>
        ///  视频显示高度
        /// </summary>
        public static int Width=160;

        /// <summary>
        ///  视频显示宽度
        /// </summary>
        public static int Height = 120;

        /// <summary>
        /// 设置大小模式
        /// </summary>
        /// <param name="Model">视频显示尺寸大小</param>
        public static void SetModel(VideoSizeModel Model)
        {
            switch (Model)
            {
                case VideoSizeModel.W160_H120 :
                    Width = 160;
                    Height = 120;
                    break;
                case VideoSizeModel.W176_H144:
                    Width = 176;
                    Height = 144;
                    break;
                case VideoSizeModel.W320_H240:
                    Width = 320;
                    Height = 240;
                    break;
                case VideoSizeModel.W352_H288:
                    Width = 352;
                    Height = 288;
                    break;
                case VideoSizeModel.W640_H480:
                    Width = 640;
                    Height = 480;
                    break;
                case VideoSizeModel.W800_H600:
                    Width = 800;
                    Height = 600;
                    break;
            }
        }
    }



    /// <summary>
    /// 视频显示尺寸大小 
    /// </summary>
    public class VideoCapturerSize
    {
        /// <summary>
        ///  视频显示高度
        /// </summary>
        public int Width = 160;

        /// <summary>
        ///  视频显示宽度
        /// </summary>
        public int Height = 120;

        /// <summary>
        /// 设置大小模式
        /// </summary>
        /// <param name="Model">视频显示尺寸大小</param>
        public VideoCapturerSize(VideoSizeModel Model)
        {
            SetModel(Model);
        }


        /// <summary>
        /// 设置大小模式
        /// </summary>
        /// <param name="Model"></param>
        public void SetModel(VideoSizeModel Model)
        {
            switch (Model)
            {
                case VideoSizeModel.W160_H120:
                    Width = 160;
                    Height = 120;
                    break;
                case VideoSizeModel.W176_H144:
                    Width = 176;
                    Height = 144;
                    break;
                case VideoSizeModel.W320_H240:
                    Width = 320;
                    Height = 240;
                    break;
                case VideoSizeModel.W352_H288:
                    Width = 352;
                    Height = 288;
                    break;
                case VideoSizeModel.W640_H480:
                    Width = 640;
                    Height = 480;
                    break;
                case VideoSizeModel.W800_H600:
                    Width = 800;
                    Height = 600;
                    break;
            }
        }
    }
}
