using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IMLibrary3
{
    /// <summary>
    /// 图片文件服务类
    /// </summary>
    public class ServerImage
    {
         /// <summary>
         /// 构造
         /// </summary>
         /// <param name="MD5">文件MD5值</param>
        public ServerImage(string MD5)
        {
            this.MD5 = MD5;
        }
        
        /// <summary>
        /// 文件上传者 
        /// </summary>
        public string userID = "";

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name="";

        /// <summary>
        /// 文件MD5
        /// </summary>
        public string MD5 = "";

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension = "";

        /// <summary>
        /// 已上传长度
        /// </summary>
        public long uploadLength = 0;

        /// <summary>
        /// 文件长度
        /// </summary>
        public long Length = 0;

        /// <summary>
        /// 文件二进制数据
        /// </summary>
        public byte[] Data = new byte[0];

        /// <summary>
        /// 最后一次激活时间
        /// </summary>
        public DateTime LastActivity = DateTime.Now;
    }
     
}
