using System;
using System.Collections.Generic;
using System.Text;
using IMLibrary3.Net.TCP;


namespace IMLibrary3.Net
{
    /// <summary>
    /// TCP客户端
    /// </summary>
   public  class TCPClient:TCP.TCP_Client 
    {
      /// <summary>
      /// 构造
      /// </summary>
       public TCPClient()
       {
       }

       /// <summary>
       /// 发送数据
       /// </summary>
       /// <param name="e">IMLibrary3.Protocol.Element</param>
       public void Write(IMLibrary3.Protocol.Element e)
       {
           this.Write(IMLibrary3.Protocol.Factory.CreateXMLMsg(e));
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
