using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

using IMLibrary3.Net;
using IMLibrary3.Protocol;
using IMLibrary3.Operation;

namespace IMLibrary3
{
    /// <summary>
    /// P2P文件传输
    /// </summary>
    public class p2pFileClient :FileTransmitBase 
    {

        #region 构造函数
        /// <summary>
        /// 发送文件方使用此构造函数
        /// </summary>
        /// <param name="serverEP">代理服务器主机信息</param>
        /// <param name="fullFileName">文件路径</param>
        public p2pFileClient(IPEndPoint serverEP, string fullFileName)
            : base(serverEP, fullFileName)
        {
 
        }

        /// <summary>
        /// 接收文件方使用此构造函数
        /// </summary>
        /// <param name="serverEP">代理服务器主机信息</param>
        /// <param name="tFileInfo">接收文件的信息</param>
        public p2pFileClient(IPEndPoint serverEP, TFileInfo tFileInfo)
            : base(serverEP, tFileInfo)
        {
            CacheFile = System.Windows.Forms.Application.StartupPath + "\\FileCache\\" + TFileInfo.MD5;
        }

      

        #endregion

        /// <summary>
        /// 发送文件包消息
        /// </summary>
        UDPPacket FilePackageMsg = new UDPPacket();

        //UDPFileMsg RequestSendFilePakMsg = new UDPFileMsg();

        #region 私有方法

        #region sockUDP_DataArrival
        private void sockUDP_DataArrival(object sender, SockEventArgs e)
        {
            if (e.Data.Length < 21) return;
            UDPPacket fileMsg = new UDPPacket(e.Data);

            if (fileMsg.type == (byte)TransmitType.getFilePackage)//收到文件数据包 
                if (IsSend)  //如果是文件发送者，则按指定位置发送文件
                    sendFile(fileMsg.LastPos);
                else//如果是文件接收者，处理接收文件数据包
                    ReceivedFileBlock(fileMsg);
            else if (fileMsg.type == (byte)TransmitType.Penetrate)//收到另一客户端请求打洞 
            {
                toEP = e.RemoteIPEndPoint;
                if (fileMsg.Block.Length > mtu)//确定MTU值
                    mtu = fileMsg.Block.Length;

                if (TFileInfo.connectedType == ConnectedType.None)//如果还未连接，继续打洞
                    sockUDP.Send(toEP, fileMsg.BaseData);//告之对方收到打洞包并向对方打洞 
            }
            else if (fileMsg.type == (byte)TransmitType.getRemoteEP)//收到自己的远程主机信息 
            {
                if (myRemoteEP == null)
                    myRemoteEP = new IPEndPoint(fileMsg.RemoteIP, fileMsg.Port);//设置自己的远程主机信息
             }
            else if (fileMsg.type == (byte)TransmitType.over)
            {
                if (IsConnected == false)//如果文件传输结束标识为false
                {
                    IsConnected = true;//文件传输结束标识为真
                    if (FS != null)
                    {
                        FS.Close(); FS.Dispose(); FS = null;
                    }
                    OnFileTransmitted();//触发文件传输完成
                }
            }
        }
        #endregion

        #region 发送文件包
        /// <summary>
        /// 发送文件包
        /// </summary>
        /// <param name="fileMsg"></param>
        private void sendFilePak(UDPPacket fileMsg)
        {
            if (toEP != null)
            {
                if (TFileInfo.connectedType == ConnectedType.None || TFileInfo.connectedType == ConnectedType.UDPServer)//如果通过服务器中转
                {
                    fileMsg.RemoteIP = toEP.Address;//告诉服务器接收方的IP和端口
                    fileMsg.Port = toEP.Port;
                    sockUDP.Send(ServerEP, fileMsg.ToBytes());
                }
                else//如果直连，直接发送数据给对方
                {
                    sockUDP.Send(toEP, fileMsg.ToBytes());
                }
            }
        }
        #endregion
         
        #region 发送文件
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="currLength">当前已经传输完成的文件数据长度</param>
        private void sendFile(long currLength)
        {
            //如果对方要求发送的数据块起始位置大于文件尺寸则认为是非法请求退出
            if (currLength >= TFileInfo.Length)
            {
                FilePackageMsg.type = (byte)TransmitType.getFilePackage;
                FilePackageMsg.LastPos = TFileInfo.Length+1;
                for (int i = 0; i < 10; i++)
                {
                    sendFilePak(FilePackageMsg); //发送文件数据给对方
                    System.Threading.Thread.Sleep(100);
                }
                return;
            }


            if (!isTransmit)
            {
                isTransmit = true;//标记文件传输中
                OnFileTransmitBefore();//触发文件开始传输前事件
            }

            //while (currLength != TFileInfo.Length)
            {
                if (CurrRecLength % maxReadWriteFileBlock == 0)////如果需要读文件
                {
                    CurrRecLength = 0;//断点清零,重新记忆

                    if (fileBlock == null)
                        fileBlock = new byte[maxReadWriteFileBlock];

                    if ((TFileInfo.Length - currLength) < maxReadWriteFileBlock)//如果是最后一次读写文件，则将所有文件尾数据全部读入到内存
                        fileBlock = new byte[TFileInfo.Length - currLength];


                    #region 这个方法还可以
                    if (FS == null)
                        FS = new FileStream(TFileInfo.fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    FS.Seek(currLength, SeekOrigin.Begin);
                    FS.Read(fileBlock, 0, fileBlock.Length);
                    #endregion

                    #region 或这个方法也可以
                    //FileStream fw = new FileStream(TFileInfo.fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    //fw.Seek(currLength, SeekOrigin.Begin);//上次发送的位置
                    //fw.Read(fileBlock, 0, fileBlock.Length); 
                    //fw.Close();
                    //fw.Dispose();
                    #endregion
                }

                long offSet = CurrRecLength % this.maxReadWriteFileBlock;// 获得要发送的绝对位置

                if (offSet + mtu > fileBlock.Length)
                    buffer = new byte[fileBlock.Length - offSet];//要发送的缓冲区


                Buffer.BlockCopy(fileBlock, (int)offSet, buffer, 0, buffer.Length);//将其保存于Buffer字节数组

                currLength += buffer.Length;
                CurrRecLength += buffer.Length;//断点设置


                FilePackageMsg.type = (byte)TransmitType.getFilePackage;
                FilePackageMsg.LastPos = currLength;
                FilePackageMsg.Block = buffer;

                sendFilePak(FilePackageMsg); //发送文件数据给对方

                TFileInfo.CurrLength = currLength;
                OnFileTransmitting();//触发收到或发送文件数据事件 
            }
        }
        #endregion 

        #region 处理收到的文件数据块
        /// <summary>
        /// 处理对方发送文件数据块
        /// </summary>
        private void ReceivedFileBlock(UDPPacket msg)//当对方发送文件数据块过来
        {
            if (msg.LastPos > TFileInfo.Length)
            {
                currGetPos = msg.LastPos;

                if (File.Exists(TFileInfo.fullName))
                {
                    File.Delete(TFileInfo.fullName);
                    System.Threading.Thread.Sleep(100);
                }

                File.Move(CacheFile, TFileInfo.fullName);//拷贝文件

                RequestSendFilePak();//告诉对方，文件发送结束
                return;
            }

            if (msg.LastPos > currGetPos)//如果发送过来的数据大于当前获得的数据
            {
                if (!isTransmit)
                {
                    isTransmit = true;//标记文件传输中
                    OnFileTransmitBefore();//触发文件开始传输前事件
                }

                if (CurrRecLength % maxReadWriteFileBlock == 0)//如果需要写文件
                {
                    CurrRecLength = 0;//断点清零,重新记忆

                    if (fileBlock == null)
                        fileBlock = new byte[maxReadWriteFileBlock];

                    if ((TFileInfo.Length - currGetPos) < maxReadWriteFileBlock)//如果是最后一次读写文件，则将所有文件尾数据全部读入到内存
                        fileBlock = new byte[TFileInfo.Length - currGetPos];

                }

                long offSet = CurrRecLength % maxReadWriteFileBlock;// 获得要读写内存的绝对位置

                Buffer.BlockCopy(msg.Block, 0, fileBlock, (int)offSet, msg.Block.Length);//将其保存于Buffer字节数组

                CurrRecLength += msg.Block.Length;//进行断点续传的继点记忆
                currGetPos = msg.LastPos;

                if (CurrRecLength % maxReadWriteFileBlock == 0 || currGetPos == TFileInfo.Length)//如果需要写文件
                {
                    #region 文件操作

                    #region 这个方法不可取
                    //if (FS == null)
                    //    FS = new FileStream(CacheFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                    //FS.Write(fileBlock, 0, fileBlock.Length);
                    //FS.Flush();
                    #endregion

                    #region 这个方法还可以
                    FS = new FileStream(CacheFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                    FS.Write(fileBlock, 0, fileBlock.Length);
                    FS.Flush();
                    FS.Close();
                    FS.Dispose();
                    FS = null;
                    #endregion


                    if (currGetPos == TFileInfo.Length)
                    {
                        if (FS != null)
                        {
                            FS.Close(); FS.Dispose(); FS = null;
                        }

                        if (File.Exists(TFileInfo.fullName))
                        {
                            File.Delete(TFileInfo.fullName);
                            System.Threading.Thread.Sleep(100);
                        }
                        File.Move(CacheFile, TFileInfo.fullName);//拷贝文件
                    }
                    #endregion
                }

                TFileInfo.CurrLength = currGetPos;//设置当前已传输位置
                OnFileTransmitting();//触发收到或发送文件数据事件 

                RequestSendFilePak();//请求对方发送文件的下一数据包
            }

        }
        #endregion

        #region  请求对方发送文件数据包
        /// <summary>
        /// 请求对方发送文件数据包
        /// </summary>
        private void RequestSendFilePak()
        {
            UDPPacket RequestSendFilePakMsg = new UDPPacket();

            if (currGetPos == TFileInfo.Length)//如果文件传输完成，触发传输完成事件
            {
                RequestSendFilePakMsg.type = (byte)TransmitType.over;//标识完成 
                sendFilePak(RequestSendFilePakMsg);//告诉对方文件传输结束

                for (int i = 0; i < 10; i++)
                {
                    sendFilePak(RequestSendFilePakMsg); 
                    System.Threading.Thread.Sleep(100);
                }

                OnFileTransmitted();//触发文件传输结束事件
                return;//文件传输
            }
            else
            {
                RequestSendFilePakMsg.type = (byte)TransmitType.getFilePackage;//请求发送文件
                RequestSendFilePakMsg.LastPos = currGetPos;//当前要获取数据据的偏移量
                sendFilePak(RequestSendFilePakMsg); //请求对方发送文件数据  
            }

            #region 超时 时间计数器
            int TimeCount = 0;
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Interval =3000;
            timer1.Enabled = true;
            timer1.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                TimeCount++;
                if (RequestSendFilePakMsg.LastPos == currGetPos && TimeCount <= 6) 
                {
                    RequestSendFilePakMsg = new UDPPacket();
                    RequestSendFilePakMsg.type = (byte)TransmitType.getFilePackage; 
                    RequestSendFilePakMsg.LastPos = currGetPos;//当前要获取数据据的偏移量
                    sendFilePak(RequestSendFilePakMsg); //请求对方发送文件数据
                }
                else
                {
                    TimeCount = 0;
                    timer1.Enabled = false;
                    timer1.Dispose();
                    timer1 = null;
                    if (RequestSendFilePakMsg.LastPos == currGetPos)//如果超时
                    {
                        OnFileTransmitOutTime();
                        Console.WriteLine("接收文件超时");
                    }
                }
            };
            #endregion
        }
        #endregion

        #endregion

        #region 公共方法

        #region UDP服务
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="AllowResume">是否断点续传文件</param>
        public void Start(bool AllowResume)
        {
            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\FileCache");
            if (!dInfo.Exists)
                dInfo.Create();
            CacheFile = System.Windows.Forms.Application.StartupPath + "\\FileCache\\" + TFileInfo.MD5;

            if (AllowResume)//如果断点续传
            {
                FileInfo finfo = new FileInfo(CacheFile);
                if (finfo.Exists)//如果缓存文件存在，则触发可断点续传事件
                    currGetPos = finfo.Length;//设置断点续传接收文件位置
            }
            else
            {
                File.Delete(CacheFile);//删除缓存文件
                System.Threading.Thread.Sleep(100);
            }


            if (sockUDP != null) return;//如果已经初始化套接字 ，则退出

            if (sockUDP == null)
            {
                sockUDP = new SockUDP();
                sockUDP.IsAsync = true;//异步通信
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

            UDPPacket ftMsg = new UDPPacket();
            ftMsg.type = (byte)TransmitType.getRemoteEP;//获得公网主机信息

            sockUDP.Send(ServerEP, ftMsg.BaseData);

            #region 与服务器保持通信
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval =30000;
            //timer.Enabled = true;
            //timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            //{
            //    if (sockUDP != null)
            //    { 
            //      sockUDP.Send(ServerEP, ftMsg.BaseData);
            //    }
            //};
            #endregion

            #region 主机信息获取 时间计数器
            int TimeCount = 0;
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Interval = 500;
            timer1.Enabled = true;
            timer1.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                if (myRemoteEP == null && TimeCount < 10)//如果还未获得本机远程主机信息
                {
                    sockUDP.Send(ServerEP, ftMsg.BaseData);//每隔500毫秒发送一次请求
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
        }
         
       
        #endregion

        #region 设置远程主机信息
        /// <summary>
        /// 设置远程主机信息
        /// </summary>
        /// <param name="remoteIP">远程主机（广域网）</param>
        /// <param name="remoteLocalIP">远程主机（局域网）</param>
        public void setRemoteIP(IPEndPoint remoteLocalIP,IPEndPoint remoteIP)
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
                    UDPPacket ftMsg = new UDPPacket();
                    ftMsg.type = (byte)TransmitType.Penetrate;//打洞数据包
                    ftMsg.Block = new byte[1400];
                    sockUDP.Send(remoteLocalIP, ftMsg.ToBytes());//发送一次局域网正常打洞请求

                    UDPPacket ftMsg1 = new UDPPacket();
                    ftMsg1.type = (byte)TransmitType.Penetrate;//打洞数据包
                    ftMsg1.Block = new byte[512];
                    sockUDP.Send(remoteLocalIP, ftMsg1.ToBytes());//发送一次局域网小包打洞请求


                    UDPPacket ftMsg2 = new UDPPacket();
                    ftMsg2.type = (byte)TransmitType.Penetrate;//打洞数据包
                    ftMsg2.Block = new byte[1400];
                    sockUDP.Send(remoteIP, ftMsg2.ToBytes());//发送一次广域网打洞请求

                    UDPPacket ftMsg3 = new UDPPacket();
                    ftMsg3.type = (byte)TransmitType.Penetrate;//打洞数据包
                    ftMsg3.Block = new byte[512];
                    sockUDP.Send(remoteIP, ftMsg3.ToBytes());//发送一次广域网小包打洞请求
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
                            TFileInfo.connectedType = ConnectedType.UDPLocal;//标明是局域网连接
                            Console.WriteLine("局域网打洞成功，MTU=" + mtu.ToString() + ",打洞次数：" + TimeCount.ToString());
                        }
                        else
                        {
                            TFileInfo.connectedType = ConnectedType.UDPRemote;//标明是广域网连接
                            Console.WriteLine("广域网打洞成功，MTU=" + mtu.ToString() + "，打洞次数：" + TimeCount.ToString());
                        }
                    }
                    else//打洞超时，数据只能通过服务器中转
                    {
                        Console.WriteLine("局域网打洞不成功，打洞次数：：" + TimeCount.ToString());
                        mtu = 1400;
                        toEP = remoteIP;//将对方的广域网远程主机信息记录下来
                    }

                    if (!IsSend)//如果是文件接收端
                        RequestSendFilePak();//开始获取文件数据包

                    OnFileTransmitConnected();//触发连接建立事件
                }
            };
            #endregion
        }
        #endregion

        #region 取消文件传输
        /// <summary>
        /// 取消文件传输
        /// </summary>
        public void CancelTransmit()
        {
            if(isTransmit)
            {
                if (CurrRecLength > 0 && !IsSend)//如果传输文件存在断点并且是文件接收者,保存断点
                {
                    FS = new FileStream(CacheFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                    FS.Write(fileBlock, 0, CurrRecLength);
                    FS.Flush();
                    FS.Close(); FS.Dispose(); FS = null;
                    CurrRecLength = 0;
                }
            }
        }
        #endregion

        #endregion

    } 
}
