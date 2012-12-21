using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using IMLibrary3.Net.UDP;

namespace  IMLibrary3.Net
{
    /// <summary>
    /// 高性能UDP服务（采用完成端口设计）
    /// </summary>
    public class UDPServer:UDP_Server
    {
        /// <summary>
        /// 
        /// </summary>
        public UDPServer()
        {

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="remoteEP"></param>
        public void SendPacket(byte[] packet , IPEndPoint remoteEP)
        {
            SendPacket(packet, 0, packet.Length, remoteEP );
        }

    }
}
