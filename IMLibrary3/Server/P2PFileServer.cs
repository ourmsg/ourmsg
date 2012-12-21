using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using IMLibrary3.Net;
using IMLibrary3.Protocol;

namespace IMLibrary3.Server
{
    /// <summary>
    /// UDP文件代理服务器
    /// </summary>
    public class P2PFileServer
    {
        /// <summary>
        /// 文件代理服务器
        /// </summary>
        /// <param name="Port"></param>
        public P2PFileServer(int Port)
        {
            port = Port;
        }


        private int port = 0;

        /// <summary>
        /// UDP服务(文件传输代理服务)
        /// </summary>
        private SockUDP udpFileServer = null;

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
            if (udpFileServer == null)
            {
                udpFileServer = new SockUDP();
                udpFileServer.IsAsync = true;
                udpFileServer.DataArrival += new SockUDP.UDPEventHandler(udpFileServer_DataArrival);
                udpFileServer.Listen(port);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (udpFileServer != null && udpFileServer.Listened)
                udpFileServer.CloseSock();
            udpFileServer = null;
        }

        #region UDP文件代理服务

        void udpFileServer_DataArrival(object sender, SockEventArgs e)
        {
            if (e.Data.Length < 21) return;//如果收到非法数据包则退出
            UDPPacket fileMsg = new UDPPacket(e.Data);

            if (fileMsg.type == (byte)TransmitType.getFilePackage || fileMsg.type == (byte)TransmitType.over || fileMsg.type == (byte)TransmitType.Penetrate)
            {
                //客户端请求与另一客户端打洞或请求转发文件数据包到另一客户端
                IPEndPoint RemoteEP = new IPEndPoint(fileMsg.RemoteIP, fileMsg.Port);//获得消息接收者远程主机信息
                udpFileServer.Send(RemoteEP, fileMsg.BaseData);//将远程主机信息发送给客户端
            }
            else if (fileMsg.type == (byte)TransmitType.getRemoteEP)//客户端请求获取自己的远程主机信息 
            {
                fileMsg.RemoteIP = e.RemoteIPEndPoint.Address;//设置客户端的远程IP
                fileMsg.Port = e.RemoteIPEndPoint.Port;//设置客户端的远程UDP 端口
                udpFileServer.Send(e.RemoteIPEndPoint, fileMsg.BaseData);//将远程主机信息发送给客户端
            }

            //if (DataArrival != null)
            //    DataArrival(this, new SockEventArgs(e.Data, e.RemoteIPEndPoint));

        }


        //private void udpServer_PacketReceived(object sender, UDP_e_PacketReceived e)
        //{
            //Console.WriteLine(e.Count.ToString());
            //Console.WriteLine(e.RemoteEP.Address.ToString() + "-" + e.RemoteEP.Port.ToString());
            //if (e.Count < 21) return;//如果收到非法数据包则退出
            //byte[] data = new byte[e.Count];
            //Buffer.BlockCopy(e.Buffer, 0, data, 0, e.Count);

            //UDPFileTransmit fileMsg = new UDPFileTransmit(data);

            //if (fileMsg.type == (byte)fileTransmitType.getFilePackage || fileMsg.type == (byte)fileTransmitType.Penetrate)
            //{
            //    //客户端请求与另一客户端打洞或请求转发文件数据包到另一客户端

            //    IPEndPoint RemoteEP = new IPEndPoint(fileMsg.RemoteIP, fileMsg.Port);//获得消息接收者远程主机信息
            //    //fileMsg.RemoteIP = e.RemoteEP.Address;//设置消息发送客户端的远程IP
            //    //fileMsg.Port = e.RemoteEP.Port;//设置消息发送客户端的远程UDP端口
            //    udpFileServer.SendPacket(fileMsg.BaseData, RemoteEP);//将远程主机信息发送给客户端
            //}
            //else if (fileMsg.type == (byte)fileTransmitType.getRemoteEP)//客户端请求获取自己的远程主机信息 
            //{
            //    fileMsg.RemoteIP = e.RemoteEP.Address;//设置客户端的远程IP
            //    fileMsg.Port = e.RemoteEP.Port;//设置客户端的远程UDP 端口
            //    udpFileServer.SendPacket(fileMsg.BaseData, e.RemoteEP);//将远程主机信息发送给客户端
            //}

            //if (DataArrival != null)
            //    DataArrival(this, new SockEventArgs(data, e.RemoteEP));

        //}

        #endregion 

    }
}
