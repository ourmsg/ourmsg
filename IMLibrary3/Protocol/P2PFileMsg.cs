using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// P2P文件传输协商协议
    /// </summary>
    public class P2PFileMsg:Element 
    {
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 传输的文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 传输文件的大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 对端远程IP
        /// </summary>
        public IPAddress remoteIP{ get; set; }

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
    }
}
