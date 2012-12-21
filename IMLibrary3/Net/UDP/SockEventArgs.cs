using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IMLibrary3.Net
{
    #region 通信事件参数类
    /// <summary>
    /// 通信事件参数
    /// </summary>
    public class SockEventArgs : System.EventArgs
    { 
        /// <summary>
        /// 错误信息
        /// </summary>
        public string  Message;

        /// <summary>
        /// 收到的数据
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 远程主机
        /// </summary>
        public IPEndPoint RemoteIPEndPoint;

        /// <summary>
        /// 套接事件相关参数
        /// </summary>
        public SockEventArgs()
        {

        }

        /// <summary>
        /// UDP通信事件参数
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="消息"></param>
        public SockEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// UDP通信事件参数
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="remoteIPEndPoint">远程主机</param>
        public SockEventArgs(byte[] data, IPEndPoint remoteIPEndPoint)
        {
             Data = data;
             RemoteIPEndPoint = remoteIPEndPoint;
        }
    }

    #endregion
}
