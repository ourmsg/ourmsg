using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using IMLibrary3.Net;
using IMLibrary3.Protocol;

namespace IMLibrary3.Server
{
    /// <summary>
    /// 音视频服务器
    /// </summary>
    public class P2PAVServer
    {
        /// <summary>
        /// 文件代理服务器
        /// </summary>
        /// <param name="Port"></param>
        public P2PAVServer(int Port)
        {
            port = Port;
        }


        private int port = 0;

        /// <summary>
        /// UDP服务 
        /// </summary>
        private SockUDP udpServer = null;

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
        #endregion 

        /// <summary>
        /// 开始服务
        /// </summary>
        public void Start()
        {
            if (udpServer == null)
            {
                udpServer = new SockUDP();
                udpServer.IsAsync = true;
                udpServer.DataArrival += new SockUDP.UDPEventHandler(udpServer_DataArrival);
                udpServer.Listen(port);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (udpServer != null && udpServer.Listened)
                udpServer.CloseSock();
            udpServer = null;
        }


        void udpServer_DataArrival(object sender, SockEventArgs e)
        {
            if (e.Data.Length < 29) return;//如果收到非法数据包则退出
            //byte[] data = new byte[e.Count];
            //Buffer.BlockCopy(e.Buffer, 0, data, 0, e.Count);

            UDPFramePacket packet = new UDPFramePacket(e.Data);

            if (packet.type == (byte)TransmitType.Audio || packet.type == (byte)TransmitType.Video)
            {
                //客户端请求与另一客户端打洞或请求转发文件数据包到另一客户端
                IPEndPoint RemoteEP = new IPEndPoint(packet.RemoteIP, packet.Port);//获得消息接收者远程主机信息
                udpServer.Send(RemoteEP, packet.BaseData);//将远程主机信息发送给客户端
            }
            else if (packet.type == (byte)TransmitType.getRemoteEP)//客户端请求获取自己的远程主机信息 
            {
                packet.RemoteIP = e.RemoteIPEndPoint.Address;//设置客户端的远程IP
                packet.Port = e.RemoteIPEndPoint.Port;//设置客户端的远程UDP 端口
                udpServer.Send(e.RemoteIPEndPoint, packet.BaseData);//将远程主机信息发送给客户端
            }
             
        }
    }
}
