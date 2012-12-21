using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IMLibrary3.Protocol
{


    /// <summary>
    /// MPG4 音视频对话消息
    /// </summary>
    public class AVMsg : Element
    {
        /// <summary>
        /// 对端远程IP
        /// </summary>
        public IPAddress remoteIP { get; set; }

        /// <summary>
        /// 对端远程端口
        /// </summary>
        public int remotePort { get; set; }

        /// <summary>
        /// 对端局域网IP
        /// </summary>
        public IPAddress LocalIP { get; set; }

        /// <summary>
        /// 对端局域网端口
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// MPG4编码信息
        /// </summary>
        public int biSize { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biWidth { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biHeight { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public short biPlanes { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public short biBitCount { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biCompression { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biSizeImage { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biXPelsPerMeter { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biYPelsPerMeter { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biClrUsed { get; set; }
        /// <summary>
        ///  MPG4编码信息
        /// </summary>
        public int biClrImportant { get; set; }
    }
}
