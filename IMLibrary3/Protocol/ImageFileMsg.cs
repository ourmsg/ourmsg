using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// tcp图片文件
    /// </summary>
    public class ImageFileMsg:Element 
    {
         
        /// <summary>
        /// 文件发送接收类型（是发送给群还是发送给用户）
        /// </summary>
        public IMLibrary3.Enmu.MessageType MessageType { get; set; }

        /// <summary>
        /// 传输的文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 传输文件的大小
        /// </summary>
        public long  Length { get; set; }
  
        /// <summary>
        /// 上次发送的文件位置
        /// </summary>
        public long LastLength { get; set; }

        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string MD5{ get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件包数据
        /// </summary>
        public byte[] fileBlock { get; set; }
    }
}
