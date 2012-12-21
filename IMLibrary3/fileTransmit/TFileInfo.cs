using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3
{
    /// <summary>
    /// 传输文件信息
    /// </summary>
    public class TFileInfo
    { 
        /// <summary>
        /// 联接类型
        /// </summary>
        public ConnectedType connectedType = ConnectedType.None;

        /// <summary>
        /// 传输的文件路径
        /// </summary>
        public string fullName;

        /// <summary>
        /// 传输的文件名
        /// </summary>
        public string Name;

        /// <summary>
        /// 传输文件的大小
        /// </summary>
        public long Length;

        /// <summary>
        /// 文件尺寸中文描述
        /// </summary>
        public string LengthStr = "0";

        /// <summary>
        /// 当前传输完成的文件长度
        /// </summary>
        public long CurrLength=0;
         
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string  MD5 ="";

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension = "";

        /// <summary>
        /// 是否文件发送者
        /// </summary>
        public bool IsSend;

        /// <summary>
        /// 信息
        /// </summary>
        public string Message;

    }

    #region 文件传输联接类型
    /// <summary>
    /// 文件传输联接类型
    /// </summary>
    public enum ConnectedType
    {
        /// <summary>
        /// 未联接
        /// </summary>
        None,
        /// <summary>
        /// UDP局域网联接
        /// </summary>
        UDPLocal,
        /// <summary>
        /// UDP广域网联接
        /// </summary>
        UDPRemote,
        /// <summary>
        /// UDP服务器中转联接
        /// </summary>
        UDPServer,

    }
    #endregion

    #region 文件传输事件参数
    /// <summary>
    /// 
    /// </summary>
    public class fileTransmitEvnetArgs : System.EventArgs
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public TFileInfo fileInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        public fileTransmitEvnetArgs()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="FileInfo">文件信息</param>
        public fileTransmitEvnetArgs(TFileInfo FileInfo)
        {
            fileInfo = FileInfo;
        }
    }
    #endregion
}
