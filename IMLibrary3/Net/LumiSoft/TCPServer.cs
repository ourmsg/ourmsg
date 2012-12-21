using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using IMLibrary3.Net.TCP;

namespace IMLibrary3.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class TCPServer : TCP_Server<TCPServerSession>
    {
        /// <summary>
        /// 
        /// </summary>
        public TCPServer() 
        {
           
        }

        /// <summary>
        /// 发送消息给一个认证的TCP客户端
        /// </summary>
        /// <param name="session">TCP session</param>
        /// <param name="e"></param>
        public void SendMessageToSession(TCPServerSession session, object e)
        {
            if (session != null && !session.IsDisposed && session.IsConnected)
                session.Write(e);//发送消息给TCP客户端
        }


        /// <summary>
        /// 发送消息给一个认证的TCP客户端
        /// </summary>
        /// <param name="session">TCP session</param>
        /// <param name="Message">XML文本消息</param>
        public void SendMessageToSession(TCPServerSession session, string Message)
        {
            if (session != null && !session.IsDisposed && session.IsConnected)
                session.Write(Message);//发送消息给TCP客户端
        }


        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="e"></param>
        public void BroadcastingMessage(object e)
        {
            string xmlstr = IMLibrary3.Protocol.Factory.CreateXMLMsg(e);
            foreach (TCPServerSession session in Sessions.ToArray())
                if (!session.IsDisposed && session.IsAuthenticated)//如果用户已经登录
                    session.Write(xmlstr);
        }

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="Message"></param>
        public void BroadcastingMessage(string Message)
        {
            foreach (TCPServerSession session in Sessions.ToArray())
                if (!session.IsDisposed && session.IsAuthenticated)//如果用户已经登录
                    session.Write(Message);
        }

    }
}
