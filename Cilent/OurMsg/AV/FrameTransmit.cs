using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using IMLibrary3;
using IMLibrary3.Net;
using IMLibrary3.Net.UDP ;
using IMLibrary3.Protocol;

namespace IMLibrary.AV
{
    /// <summary>
    /// 侦传输
    /// </summary>
    public class FrameTransmit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverEP">服务器主机信息</param>
        public FrameTransmit(IPEndPoint serverEP)
        {
            ServerEP = serverEP;
        }


        public void Dispose()
        {
            System.Threading.Thread st = new System.Threading.Thread(new System.Threading.ThreadStart(ShutdownThread));
            st.Start();
        }

        private void ShutdownThread()
        {
            if (sockUDP != null && sockUDP.Listened)
            {
                sockUDP.CloseSock();
                sockUDP = null;
            }
            isRunning = false;
            videoThread.Join();
        }


        private void VideoDecoderThread()
        {
            //isRunning = true;
            while (isRunning)
            {
                CheckCompleteFrame();
                //byte[]data = CheckCompleteFrame();
                //if (data == null)
                //{
                //    System.Threading.Thread.Sleep(0);
                //    continue;
                //}
              
            }
        }


        #region 变量
        /// <summary>
        /// 服务器主机
        /// </summary>
        private IPEndPoint ServerEP = null;

        /// <summary>
        /// 对方主机
        /// </summary>
        private IPEndPoint toEP = null;

        /// <summary>
        /// 本地主机
        /// </summary>
        private IPEndPoint myRemoteEP = null;
        /// <summary>
        ///  UDP套接字
        /// </summary>
        private SockUDP sockUDP = null;
        /// <summary>
        /// mtu
        /// </summary>
        private int mtu = 1400;
        /// <summary>
        /// 联接类型
        /// </summary>
        private ConnectedType connectedType = ConnectedType.None;

        /// <summary>
        /// 发送时间戳
        /// </summary>
        private long SendTimestamp = 0;

        /// <summary>
        /// 接收时间戳
        /// </summary>
        private long ReceiverTimestamp = 0;

        /// <summary>
        /// 收到的侦集合
        /// </summary>
        SortedDictionary<long, frame> ReceiverFrames = new SortedDictionary<long, frame>();
        /// <summary>
        /// 是否运行
        /// </summary>
        private bool isRunning = false;

        //获取帧线程
        System.Threading.Thread videoThread;

        #endregion

        #region 事件

        public delegate void RecDataEventHandler(object sender,byte[] data);
        public event RecDataEventHandler RecVideoData;
        protected virtual void OnRecVideoData(object sender,byte[] data)
        {
            if (RecVideoData != null)
                RecVideoData(this, data);
        }
        public event RecDataEventHandler RecAudioData;
        protected virtual void OnRecAudioData(object sender, byte[] data)
        {
            if (RecAudioData != null)
                RecAudioData(this, data);
        }
        /// <summary>
        /// 获得IPEndPoint事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="local">本地主机信息</param>
        /// <param name="remote">远程主机信息</param>
        public delegate void GetIPEndPointEventHandler(object sender, IPEndPoint local, IPEndPoint remote);
        public event GetIPEndPointEventHandler GetIPEndPoint;
        protected virtual void OnGetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            if (GetIPEndPoint != null)
                GetIPEndPoint(this, local, remote);//触发获取本机主机事件
        }

        public delegate void TransmitEventHandler(object sender, ConnectedType connectedType);
        /// <summary>
        /// 文件传输成功联接到服务器事件
        /// </summary>
        public event TransmitEventHandler TransmitConnected;
        /// <summary>
        /// 触发文件传输成功联接到服务器事件
        /// </summary>
        protected virtual void OnTransmitConnected()
        {
            if (TransmitConnected != null)
                TransmitConnected(this, this.connectedType);
        }
        #endregion

        #region sockUDP_DataArrival
        private void sockUDP_DataArrival(object sender, SockEventArgs e)
        {
            if (e.Data.Length < 29) return;
            UDPFramePacket packet = new UDPFramePacket(e.Data);//获得一侦数据的一个包

            if (packet.type == (byte)TransmitType.Video || packet.type == (byte)TransmitType.Audio)//收到音频视频数据包 
            {
                if (packet.type == (byte)TransmitType.Video)//触发视频数据到达事件
                    OnRecVideoData(this, packet.Block );
                else if (packet.type == (byte)TransmitType.Audio)//触发音频数据到达事件
                    OnRecAudioData(this, packet.Block);

                return;

                if (packet.PacketCount >0 && packet.PacketCount <33)//最大32个数据包
                {
                    lock (ReceiverFrames)
                    {
                        if (ReceiverFrames.ContainsKey(packet.Timestamp))//如果侦已经存在
                        {
                            frame f = ReceiverFrames[packet.Timestamp];
                            f.Add(packet);//将包添加到侦
                        }
                        else
                        {
                            frame f = new frame();//创建侦
                            f.Add(packet);//将包添加到侦
                            f.dateTime = DateTime.Now;
                            f.type = packet.type;//设置侦类型
                            f.Timestamp = packet.Timestamp;//设置时间戳
                            f.PacketCount = packet.PacketCount;//设置侦的包总数
                            ReceiverFrames.Add(packet.Timestamp, f);//添加侦
                        }
                    }
                }
            }
            else if (packet.type == (byte)TransmitType.Penetrate)//收到另一客户端请求打洞 
            {
                toEP = e.RemoteIPEndPoint;
                if (packet.Block.Length > mtu)//确定MTU值
                    mtu = packet.Block.Length;
                if (connectedType == ConnectedType.None)//如果还未连接，继续打洞
                    sockUDP.Send(toEP, packet.BaseData);//告之对方收到打洞包并向对方打洞 
            }
            else if (packet.type == (byte)TransmitType.getRemoteEP)//收到自己的远程主机信息 
            {
                if (myRemoteEP == null)
                    myRemoteEP = new IPEndPoint(packet.RemoteIP, packet.Port);//设置自己的远程主机信息
            }
        }
        #endregion

        #region 发送数据包
        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void sendData(TransmitType type, byte[] data)
        {
            if (data == null || data.Length == 0) return;
            SendTimestamp++;
            UDPFramePacket packet = new UDPFramePacket();
            packet.type = (byte)type;
            packet.Timestamp = SendTimestamp;
            packet.Block = data;
            send(packet.ToBytes());
            return;
            if (data.Length <= mtu)
            {
                packet.PacketCount = 1;
                packet.Block = data;
                send(packet.ToBytes());
                Console.WriteLine(packet.type.ToString() + ":" + packet.ToBytes().Length.ToString());
            }
            else
            {
                int count = data.Length / mtu;
                if (data.Length % mtu != 0)
                    count += 1;
                int offSet = 0;
                byte[] block = new byte[mtu];
                for (int i = 0; i < count; i++)
                {
                    packet.PacketCount = count;
                    packet.PacketIndex = i;
                    if (offSet + mtu > data.Length)
                        block = new byte[data.Length - offSet];//要发送的缓冲区

                    Buffer.BlockCopy(data, offSet, block, 0, block.Length);
                    packet.Block = block;
                    send(packet.ToBytes());
                    offSet += mtu;
                    Console.WriteLine(  packet.type.ToString() +":"+ packet.ToBytes().Length.ToString());
                }
            }
        }


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        private void send(byte[] data)
        {
            if (toEP != null && sockUDP != null && sockUDP.Listened)
                sockUDP.Send(toEP, data);
        }
        #endregion

        #region 获取一侦数据
        /// <summary>
        /// 获取一侦数据
        /// </summary>
        /// <returns></returns>
        private byte[] CheckCompleteFrame()
        {
            byte[] data=null;
            lock (ReceiverFrames)
            {
                foreach (frame f in ReceiverFrames.Values)
                { 
                    if (f.Timestamp > ReceiverTimestamp && f.FrameComplete == true) //处理接收一侦数据
                    {
                        ReceiverTimestamp = f.Timestamp;//设置收到的新的时间戳
                        data = f.GetFrame();//返回侦数据 
                        ReceiverFrames.Remove(f.Timestamp);//删除已处理侦
                        if (f.type == (byte)TransmitType.Video)//触发视频数据到达事件
                            OnRecVideoData(this, data);
                        else if (f.type == (byte)TransmitType.Audio)//触发音频数据到达事件
                            OnRecAudioData(this, data);
                        f.Dispose();
                    }
                    else if (DateTime.Now > f.dateTime.AddSeconds(1) || f.Timestamp < ReceiverTimestamp)//接收时间大于1秒或超时的数据全部抛弃
                    {
                        ReceiverFrames.Remove(f.Timestamp);//删除已处理侦
                        f.Dispose();//清空数据
                    }
                    return data;
                }
            }
            return data;
        }
        #endregion

        #region 公共方法

        #region UDP服务
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (sockUDP != null) return;//如果已经初始化套接字 ，则退出

            if (sockUDP == null)
            {
                sockUDP = new SockUDP();
                sockUDP.DataArrival += new SockUDP.UDPEventHandler(sockUDP_DataArrival);
                sockUDP.Listen(0);//随机侦听
            }

            IPEndPoint myLocalEP = null;
            System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());//获得本机局域网IPV4地址
            for (int i = ips.Length - 1; i >= 0; i--)
            {
                if (ips[i].GetAddressBytes().Length == 4)
                {
                    myLocalEP = new IPEndPoint(ips[i], sockUDP.ListenPort);//获得本机局域网地址
                    break;
                }
            }

            UDPFramePacket packet = new UDPFramePacket();
            packet.type = (byte)TransmitType.getRemoteEP;//获得公网主机信息
            packet.PacketCount = 1;
            sockUDP.Send(ServerEP, packet.BaseData);

            #region 主机信息获取 时间计数器
            int TimeCount = 0;
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Interval = 500;
            timer1.Enabled = true;
            timer1.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                if (myRemoteEP == null && TimeCount < 10)//如果还未获得本机远程主机信息
                {
                    sockUDP.Send(ServerEP, packet.BaseData);//每隔500毫秒发送一次请求
                }
                else//如果三秒内已经获得主机信息或三秒钟后未获得主机信息
                {
                    //终止发送，并触发获得主机事件
                    timer1.Enabled = false;
                    timer1.Dispose();
                    timer1 = null;
                    if (myRemoteEP == null)//如果超时未获得本机远程主机信息
                        myRemoteEP = myLocalEP;//假设本机的远程IP等于本地IP
                    OnGetIPEndPoint(this, myLocalEP, myRemoteEP);//触发获取本机主机事件
                }
                TimeCount++;
            };
            #endregion


            ///开始组包线程
            videoThread = new System.Threading.Thread(new System.Threading.ThreadStart(VideoDecoderThread));
            videoThread.Start();
        }


        #endregion

        #region 设置远程主机信息
        /// <summary>
        /// 设置远程主机信息
        /// </summary>
        /// <param name="remoteIP">远程主机（广域网）</param>
        /// <param name="remoteLocalIP">远程主机（局域网）</param>
        public void setRemoteIP(IPEndPoint remoteLocalIP, IPEndPoint remoteIP)
        {

            #region 网络打洞互联 时间计数器
            int TimeCount = 0;
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Interval = 500;
            timer1.Enabled = true;

            timer1.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                TimeCount++;

                if (toEP == null && TimeCount <= 20)//如果10秒后还未与对方建立联接
                {
                    UDPFramePacket packet = new UDPFramePacket();
                    packet.type = (byte)TransmitType.Penetrate;//打洞数据包
                    packet.Block = new byte[1400];
                    if (sockUDP != null && sockUDP.Listened)
                        sockUDP.Send(remoteLocalIP, packet.ToBytes());//发送一次局域网正常打洞请求

                    UDPFramePacket packet1 = new UDPFramePacket();
                    packet1.type = (byte)TransmitType.Penetrate;//打洞数据包
                    packet1.Block = new byte[512];
                    if (sockUDP != null && sockUDP.Listened)
                        sockUDP.Send(remoteLocalIP, packet1.ToBytes());//发送一次局域网小包打洞请求

                    UDPFramePacket packet2 = new UDPFramePacket();
                    packet2.type = (byte)TransmitType.Penetrate;//打洞数据包
                    packet2.Block = new byte[1400];
                    if (sockUDP != null && sockUDP.Listened)
                        sockUDP.Send(remoteIP, packet2.ToBytes());//发送一次广域网打洞请求

                    UDPFramePacket packet3 = new UDPFramePacket();
                    packet3.type = (byte)TransmitType.Penetrate;//打洞数据包
                    packet3.Block = new byte[512];
                    if (sockUDP != null && sockUDP.Listened)
                        sockUDP.Send(remoteIP, packet3.ToBytes());//发送一次广域网小包打洞请求
                }
                else
                {
                    //终止发送，并触发获得主机事件
                    timer1.Enabled = false;
                    timer1.Dispose();
                    timer1 = null;
                    if (toEP != null)//如果已与对方打洞成功并建立连接
                    {
                        if (TimeCount <= 10)
                        {
                            connectedType = ConnectedType.UDPLocal;//标明是局域网连接
                            Console.WriteLine("局域网打洞成功，MTU=" + mtu.ToString() + ",打洞次数：" + TimeCount.ToString());
                        }
                        else
                        {
                            connectedType = ConnectedType.UDPRemote;//标明是广域网连接
                            Console.WriteLine("广域网打洞成功，MTU=" + mtu.ToString() + "，打洞次数：" + TimeCount.ToString());
                        }
                    }
                    else//打洞超时，数据只能通过服务器中转
                    {
                        Console.WriteLine("局域网打洞不成功，打洞次数：：" + TimeCount.ToString());
                        mtu = 1400;
                        toEP = remoteIP;//将对方的广域网远程主机信息记录下来
                    }

                    OnTransmitConnected();//触发连接建立事件
                }
            };
            #endregion
        }
        #endregion

        #region 取消传输
        /// <summary>
        /// 取消传输
        /// </summary>
        public void CancelTransmit()
        {

        }
        #endregion

        #endregion

    }

    #region 一侦数据
    /// <summary>
    /// 一侦数据
    /// </summary>
    class frame
    {
        /// <summary>
        /// 
        /// </summary>
        public frame() { }

        public byte type = 0;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime dateTime = DateTime.Now;

        /// <summary>
        /// 包数量
        /// </summary>
        public int PacketCount = 0;

        /// <summary>
        /// 是否组包结束
        /// </summary>
        public bool FrameComplete = false;

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsExpired = false;

        /// <summary>
        /// 侦数据包集合
        /// </summary>
        private SortedList<int, UDPFramePacket>  Frame  = new SortedList<int, UDPFramePacket>();

        public void Dispose()
        {
            Frame.Clear();
        }


        /// <summary>
        /// 添加数据包到侦
        /// </summary>
        /// <param name="frame"></param>
        public void Add(UDPFramePacket packet)
        {
            if (!FrameComplete)
            {
                Frame.Add(packet.PacketIndex, packet);
                if (Frame.Count >= PacketCount)
                    FrameComplete = true;
            }
        }

        /// <summary>
        /// 获取一侦数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetFrame()
        {
            if (FrameComplete)
            {
                List<byte> data = new List<byte>();
                foreach (UDPFramePacket p in Frame.Values)
                    data.AddRange(p.Block);
                return data.ToArray();
            }
            else
                return null;
        }
    }
    #endregion

}
