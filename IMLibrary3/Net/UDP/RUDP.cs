using System;
using System.Collections.Generic;
using System.Text;
using Helper.Net.RUDP;
using System.Net;
using System.Threading;

namespace IMLibrary3.Net 
{
    public class RUDP
    {

        private RUDPSocket rUDP = null;
        private System.Threading.Thread thdUdp;

        public RUDP()
        {

        }

         
        #region 事件
        /// <summary>
        /// 数据到达事件
        /// </summary>
        /// <param name="sender">套接字对像</param>
        /// <param name="e">事件相关参数</param>
        public  delegate void DataArrivalEventHandler(object sender, SockEventArgs e);
        /// <summary>
        /// 数据到达事件
        /// </summary>
        public  event DataArrivalEventHandler DataArrival;
        /// <summary>
        /// 套接字错误事件
        /// </summary>
        /// <param name="sender">套接字对像</param>
        /// <param name="e">事件相关参数</param>
        public delegate void ErrorEventHandler(object sender, SockEventArgs e);
        /// <summary>
        /// 套接字错误事件
        /// </summary>
        public event ErrorEventHandler Sock_Error;
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
                byte[] RData = rUDP.EndReceive(ar);
                if (DataArrival != null)
                    DataArrival(this, new SockEventArgs(RData, ipend));

                rUDP.BeginReceive(new AsyncCallback(ReadCallback), null);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
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
                    byte[] RData = rUDP.Receive( );
                    if (DataArrival != null)
                        DataArrival(this, new SockEventArgs(RData, ipend));
                }
                catch (Exception e)
                {
                    if (Sock_Error != null)
                        Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
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
                rUDP.EndSend(ar);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs( e.Source + "," + e.Message));
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
            RUDPSocketError err= RUDPSocketError.HostDown ;
            try
            {
                if (IsAsync)
                    rUDP.BeginSend(Data, 0, Data.Length, out err, new AsyncCallback(SendCallback), null);
                else
                    rUDP.Send(Data,0, Data.Length );
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
            }
        }

        /// <summary>
        /// 侦听
        /// </summary>
        /// <param name="Port">侦听端口</param>
        public void Listen(int Port)
        {
            try
            {
                if (Port < 1 || Port >65534)
                {
                    System.Random r = new System.Random();
                resetPort:
                    Port = r.Next(2000, 65534);
                    try
                    {
                        rUDP  = new  RUDPSocket( );
                    }
                    catch
                    {
                        goto resetPort;
                    }
                }
                else
                {
                    rUDP = new RUDPSocket();
                }

                
                 Listened = true;//侦听
                 ListenPort = Port;

                 rUDP.Listen(Port);

                if (IsAsync)//如果采用异步通信
                {
                    rUDP.BeginReceive(new AsyncCallback(ReadCallback), null);
                }
                else//如果是同步通信
                {
                    thdUdp = new Thread(new ThreadStart(GetUDPData));
                    thdUdp.Start();
                }
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
            }
            //LocalEndPoint = (IPEndPoint)rUDP.lClient.LocalEndPoint;

        }

        /// <summary>
        /// 侦听
        /// </summary>
        /// <param name="LocalIp">本地IP</param>
        /// <param name="Port">本地端口</param>
        public void Listen(IPAddress LocalIp, int Port)
        {
            try
            {
                IPEndPoint ipEnd = new IPEndPoint(LocalIp, Port);
                rUDP  = new  RUDPSocket( );

               
                this.Listened = true;//侦听
                this.ListenPort = Port;

                rUDP.Listen(Port);
                if (IsAsync)//如果采用异步通信
                {
                    rUDP.BeginReceive(new AsyncCallback(ReadCallback), null);
                }
                else//如果是同步通信
                {
                    thdUdp = new Thread(new ThreadStart(GetUDPData));
                    thdUdp.Start();
                }
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
            }
        }

        /// <summary>
        /// 关闭套接字
        /// </summary>
        public void CloseSock()
        {
            try
            {
                thdUdp.Abort();

                Thread.Sleep(0);

                rUDP.Close();

                Thread.Sleep(0);

            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(this, new SockEventArgs(  e.Source + "," + e.Message));
            }
        }
        #endregion
    

    }
}
