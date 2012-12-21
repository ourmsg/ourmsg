using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

#region IMLibrary3
using IMLibrary3.Enmu;
using IMLibrary3.Security;
using IMLibrary3.Organization;
using IMLibrary3.Operation;
using IMLibrary3.Protocol;

using IMLibrary3.Net;
using IMLibrary3.Net.TCP;
#endregion


namespace IMLibrary3
{
    /// <summary>
    /// TCP文件传输
    /// </summary>
    public class ImageFileClient :FileTransmitBase 
    {
        #region 构造
        /// <summary>
        /// 构造(上传文件)
        /// </summary>
        /// <param name="serverEP">服务器主机信息</param>
        /// <param name="fullFileName">上传文件（含路径）</param>
        public ImageFileClient(IPEndPoint serverEP, string fullFileName)
            :base(serverEP,fullFileName)
        {
            mtu = 10240;//1次上传10K
            Start();
        }

        /// <summary>
        /// 构造(下载文件)
        /// </summary>
        /// <param name="serverEP">服务器主机信息</param>
        /// <param name="tFileInfo">下载文件信息</param>
        public ImageFileClient(IPEndPoint serverEP, TFileInfo tFileInfo)
            : base(serverEP, tFileInfo)
        {
            mtu = 10240;//1次下载10K
            Start();
        }
        #endregion

        #region TCP操作

        #region strat
        private void Start()
        {
            if (tcpClient == null)
            {
                tcpClient = new TCPClient();
                tcpClient.PacketReceived +=new TCP_Client.PacketReceivedEventHandler(tcpClient_PacketReceived);
                tcpClient.Connected += new EventHandler(tcpClient_Connected);
                tcpClient.Disonnected += new EventHandler(tcpClient_Disonnected);
            }
            tcpClient.Connect(ServerEP.Address.ToString(), ServerEP.Port);
            System.Timers.Timer timer = new System.Timers.Timer();
            if (timer == null)
            {
                int count = 0;
                timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
                {
                    if (IsConnected)//如果连接到服务器
                    {
                        timer.Enabled = false;
                        timer = null;
                    }
                    else  if (count < 3)//如果9秒以后还未连接到服务器
                    {
                        tcpClient.Connect(ServerEP.Address.ToString(), ServerEP.Port);
                        Console.WriteLine("try connecte!"+ count.ToString());
                    }
                    else if (count >= 3)//如果3秒后还未连接，则退出连接操作
                    {
                        Console.WriteLine("connecte filad!" );
                        timer.Enabled = false;
                        timer = null;
                    }
                    count++;
                };
            }
        }
        #endregion

        #region Disonnected
        private void tcpClient_Disonnected(object sender, EventArgs e)
        {
            Console.WriteLine("断开连接!");
        }
        #endregion

        #region PacketReceived
        private void tcpClient_PacketReceived(object sender, TcpSessionEventArgs e)
        {
            object obj = Factory.CreateInstanceObject(e.Data);
            ImageFileMsg fileMsg = obj as ImageFileMsg;
            if (fileMsg!=null )//如果收到的消息对像不为空
            {
                if (fileMsg.type == type.New)//服务器允许上传文件
                { OnFileTransmitBefore(); }//触发文件传输前事件
                else if (fileMsg.type == type.set)
                { TCPSendFile(fileMsg); }//发送文件到服务器
                else if (fileMsg.type == type.get)
                { ReceivedFileBlock(fileMsg); }
                else if (fileMsg.type == type.over)//文件传输结束
                {
                    Console.WriteLine("over!");
                    OnFileTransmitted();//触发文件传输完成事件
                }
            }
        }
        #endregion

        #region Connected
        private void tcpClient_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("tcpClient_Connected!");

            IsConnected = true;//标记已经连接到服务器
 
            OnFileTransmitConnected();//触发文件传输TCP连接成功事件

            if (IsSend)//如果是上传文件
            {
                ImageFileMsg fileMsg = new ImageFileMsg();
                fileMsg.Name = TFileInfo.Name;
                fileMsg.Length = TFileInfo.Length;
                fileMsg.Extension = TFileInfo.Extension;

                fileMsg.type = type.New;
                TCPSendMsg(fileMsg);
            }
            else
            {
                RequestSendFilePak();//如果是下载文件，则开始下载
            }
        }
        #endregion

        #endregion

        #region 方法

        #region TCP发送消息到文件代理服务器
        /// <summary>
        /// TCP发送消息到文件代理服务器
        /// </summary>
        /// <param name="e"></param>
        protected void TCPSendMsg(Element e)
        {
            if (ServerEP != null && tcpClient.IsConnected)
            {
                if (e is ImageFileMsg)
                {
                    ImageFileMsg finfo = e as ImageFileMsg;
                    finfo.MD5 = TFileInfo.MD5;
                    finfo.Extension = TFileInfo.Extension;
                }
                tcpClient.TcpStream.WriteLine(Factory.CreateXMLMsg(e));
            }
        }
        #endregion

        #region TCP发送文件
        /// <summary>
        /// TCP发送文件
        /// </summary>
        /// <param name="fileMsg"></param>
        protected void TCPSendFile(ImageFileMsg fileMsg)
        {
            long currLength = fileMsg.LastLength;

            if (currLength >= TFileInfo.Length)
            {
                OnFileTransmitted();//触发文件传输结束事件
                return;//如果对方要求发送的数据块起始位置大于文件尺寸则认为是非法请求退出
            }

            if (!isTransmit)
            {
                isTransmit = true;//标记文件传输中
                OnFileTransmitBefore();//触发文件开始传输前事件
            }

            #region 如果当前是需要读写文件
            if (CurrRecLength % maxReadWriteFileBlock == 0)
            {
                CurrRecLength = 0;//断点清零,重新记忆

                if (fileBlock == null)
                    fileBlock = new byte[maxReadWriteFileBlock]; 

                //读文件到内存过程
                if ((TFileInfo.Length - currLength) < maxReadWriteFileBlock)//如果是最后一次读写文件，则将所有文件尾数据全部读入到内存
                    fileBlock = new byte[TFileInfo.Length - currLength];
                   
                ////////////////////////文件操作
                FS = new FileStream(TFileInfo.fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                FS.Seek(currLength, SeekOrigin.Begin);//上次发送的位置
                FS.Read(fileBlock, 0, fileBlock.Length);
                FS.Close();
                FS.Dispose();
                /////////////////////////// 
            }
            #endregion

            long offSet = CurrRecLength % this.maxReadWriteFileBlock;// 获得要发送的绝对位置

            //byte[] buffer; 

            if (offSet + mtu > fileBlock.Length)
                buffer = new byte[fileBlock.Length - offSet];//要发送的缓冲区
            //else
            //    buffer = new byte[mtu];//要发送的缓冲区

            Buffer.BlockCopy(fileBlock, (int)offSet, buffer, 0, buffer.Length);//将其保存于Buffer字节数组

            currLength += buffer.Length;
            CurrRecLength += buffer.Length;//断点设置

            fileMsg.type = type.set;//上传标记
            fileMsg.LastLength = currLength;
            fileMsg.fileBlock = buffer;// 
            TCPSendMsg(fileMsg);//发送文件到服务器

            TFileInfo.CurrLength = currLength;
            OnFileTransmitting();//触发收到或发送文件数据事件 

        }
        #endregion

        #region 处理收到的文件数据块
        /// <summary>
        /// 处理对方发送文件数据块
        /// </summary>
        private void ReceivedFileBlock(ImageFileMsg fileMsg)//当对方发送文件数据块过来
        {
            if (fileMsg.LastLength > currGetPos)//如果发送过来的数据大于当前获得的数据
            {
                if (CurrRecLength % maxReadWriteFileBlock == 0)
                {
                    CurrRecLength = 0;//断点清零,重新记忆

                    if(fileBlock ==null)
                        fileBlock = new byte[maxReadWriteFileBlock]; 

                    if ((TFileInfo.Length - currGetPos) < maxReadWriteFileBlock)//如果是最后一次读写文件，则将所有文件尾数据全部读入到内存
                        fileBlock = new byte[TFileInfo.Length - currGetPos]; 
                }

                long offSet = CurrRecLength % maxReadWriteFileBlock;// 获得要读写内存的绝对位置
                Buffer.BlockCopy(fileMsg.fileBlock, 0, fileBlock, (int)offSet, fileMsg.fileBlock.Length);//将其保存于Buffer字节数组

                currGetPos = fileMsg.LastLength;
                CurrRecLength += fileMsg.fileBlock.Length;//进行断点续传的继点记忆

                if (CurrRecLength % maxReadWriteFileBlock == 0 || currGetPos == TFileInfo.Length)
                {
                    ////////////////////////文件操作
                    FS = new FileStream(TFileInfo.fullName, FileMode.Append, FileAccess.Write, FileShare.Read);
                    FS.Write(fileBlock, 0, fileBlock.Length);
                    FS.Flush();
                    FS.Close();
                    FS.Dispose();
                    ///////////////////////////
                }

                TFileInfo.CurrLength = currGetPos;//设置当前已传输位置
                OnFileTransmitting();//触发收到或发送文件数据事件 

                if (currGetPos == TFileInfo.Length)//如果文件传输完成，触发传输完成事件
                {
                    return;//文件传输
                }

                RequestSendFilePak();//请求对方发送文件的下一数据包
            }
        }
        #endregion

        #region  请求发送文件数据包
        /// <summary>
        /// 请求发送文件数据包
        /// </summary>
        private void RequestSendFilePak()
        {
            ImageFileMsg fileMsg = new ImageFileMsg();
            fileMsg.type = type.get;//标识下载
            fileMsg.LastLength = currGetPos;
            fileMsg.MD5 = TFileInfo.MD5;//要下载的文件MD5值
            TCPSendMsg(fileMsg);//请求服务器发送文件数据  

            #region 超时 时间计数器
            int TimeCount = 0;
            if (timer1 == null)
            {
                timer1 = new System.Timers.Timer();
                timer1.Interval = 5000;
                timer1.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
                {
                    TimeCount++;

                    if (fileMsg.LastLength == currGetPos && TimeCount <= 2)//如果5秒内还未获得新数据包
                    {
                        //终止发送，并触发获得主机事件
                        timer1.Enabled = false;
                        TimeCount = 0;
                        if (fileMsg.LastLength == currGetPos)//如果超时
                        {
                            OnFileTransmitOutTime();
                            Console.WriteLine("超时：");
                        }
                    }
                    else if (TimeCount > 2)
                    {
                        timer1.Enabled = false;
                        TimeCount = 0;
                        Console.WriteLine("停止超时检测");
                    }
                };
            }
            timer1.Enabled = true;
            #endregion
        }
        #endregion

        #endregion

    }
}
