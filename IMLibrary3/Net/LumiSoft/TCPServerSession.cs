using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using IMLibrary3.Net.IO;
using System.IO;
using IMLibrary3.Net;
using IMLibrary3.Net.TCP;

namespace IMLibrary3.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class TCPServerSession : TCP_ServerSession
    {
        /// <summary>
        /// 
        /// </summary>
        public TCPServerSession() 
        {
             
        }

        /// <summary>
        /// 发送对像数据
        /// </summary>
        /// <param name="e"></param>
        public void Write(object e)
        {
            this.TcpStream.WriteLine(IMLibrary3.Protocol.Factory.CreateXMLMsg(e));
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Message">数据</param>
        public void Write(string Message)
        {
            this.TcpStream.WriteLine(Message);
        }
         
    }

 
}
