using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IMLibrary3.Net
{
    /// <summary>
    /// UDP组件
    /// </summary>
    public partial class SockUDP  
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SockUDP()
        {

        }

        private UdpClient UDP_Server =null ;
        private System.Threading.Thread thdUdp;

        #region 事件
        /// <summary>
        /// 数据到达事件
        /// </summary>
        /// <param name="sender">套接字对像</param>
        /// <param name="e">事件相关参数</param>
        public delegate void UDPEventHandler(object sender, SockEventArgs e);
        /// <summary>
        /// 数据到达事件
        /// </summary>
        public event UDPEventHandler DataArrival;
       
        /// <summary>
        /// 套接字错误事件
        /// </summary>
        public event UDPEventHandler Sock_Error;
        #endregion

        #region 属性
        /// <summary>
        /// 标识是否侦听
        /// </summary>
        public bool Listened
        {
            get;
            private set;
        }

        /// <summary>
        /// 侦听端口
        /// </summary>
        public int ListenPort
        {
            get;
            private set;
        }

        /// <summary>
        /// 功能描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 本机主机信息
        /// </summary>
        public IPEndPoint LocalEndPoint;

        /// <summary>
        /// 是否异步通信
        /// </summary>
        public bool IsAsync
        {
            get;
            set;
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
           
            try
            {
                IPEndPoint ipend = null;
                byte[] RData = UDP_Server.EndReceive(ar, ref ipend);
                if (DataArrival != null)
                    DataArrival(this, new SockEventArgs(RData, ipend));

                UDP_Server.BeginReceive(new AsyncCallback(ReadCallback), null);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
                Console.WriteLine("ReadCallback:"+e.Source + "," + e.Message);
            }
        }

        /// <summary>
        /// 处理同步接收的数据
        /// </summary> 
        private void GetUDPData()
        {
            while (true)
            {
                try
                {
                    IPEndPoint ipend = null;
                    byte[] RData = UDP_Server.Receive(ref ipend);
                    if (DataArrival != null)
                        DataArrival(this, new SockEventArgs(RData, ipend));
                }
                catch (Exception e)
                {
                    if (Sock_Error != null)
                        Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
                    Console.WriteLine("GetUDPData:" + e.Source + "," + e.Message);
                }
            }
        }


        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                UDP_Server.EndSend(ar);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
                Console.WriteLine("SendCallback:" + e.Source + "," + e.Message);
            }
        }
        #endregion

        #region 外部方法
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Remote">对方主机信息</param>
        /// <param name="Data">要发送的数据</param>
        public void Send(IPEndPoint Remote , byte[] Data)
        {
            try
            {
                if (IsAsync)
                    UDP_Server.BeginSend(Data, Data.Length, Remote, new AsyncCallback(SendCallback), null);
                else
                    UDP_Server.Send(Data, Data.Length, Remote);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
                Console.WriteLine("Send:" + e.Source + "," + e.Message);
            }
        }

        /// <summary>
        /// 侦听
        /// </summary>
        /// <param name="Port">侦听端口</param>
        public void Listen(int Port)
        {
            IPEndPoint ipEnd = null;

            try
            {
                if (Port < 1 || Port >65534)
                {
                    System.Random r = new System.Random();
                resetPort:
                    Port = r.Next(2000, 65534);
                    try
                    {
                        ipEnd = new IPEndPoint(System.Net.IPAddress.Any, Port);
                        UDP_Server = new UdpClient();
                        //UDP_Server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        UDP_Server.Client.Bind(ipEnd);
                    }
                    catch
                    {
                        goto resetPort;
                    }
                }
                else
                {
                    ipEnd = new IPEndPoint(System.Net.IPAddress.Any, Port);
                    UDP_Server = new UdpClient( );
                    //UDP_Server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    UDP_Server.Client.Bind(ipEnd);
                }

                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                UDP_Server.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

                 Listened = true;//侦听
                 ListenPort = Port; 

                if (IsAsync)//如果采用异步通信
                {
                    UDP_Server.BeginReceive(new AsyncCallback(ReadCallback), null);
                }
                else//如果是同步通信
                {
                    thdUdp = new Thread(new ThreadStart(GetUDPData));
                    thdUdp.Start();
                }

                LocalEndPoint = (IPEndPoint)UDP_Server.Client.LocalEndPoint;

            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
                Console.WriteLine("Listen:" + e.Source + "," + e.Message);
            }
        }


        /// <summary>
        /// 关闭套接字
        /// </summary>
        public void CloseSock()
        {
            try
            { 
                if (thdUdp != null)
                    thdUdp.Abort();

                Thread.Sleep(0);

                if (UDP_Server != null )
                    UDP_Server.Close();

                Listened = false;
               
                Thread.Sleep(0);

            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
                Console.WriteLine("CloseSock:" + e.Source + "," + e.Message);
            }
        }
        #endregion
    }
}
