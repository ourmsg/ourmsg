using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

using IMLibrary3;
using IMLibrary3.IO;
using IMLibrary3.Net;
using IMLibrary3.Net.TCP;
using IMLibrary3.Protocol;

namespace IMLibrary3.Server
{
    /// <summary>
    /// 图片文件传输服务器
    /// </summary>
    public class ImageFileServer
    {
        /// <summary>
        /// 图片文件传输服务器
        /// </summary>
        /// <param name="Port">服务端口</param>
        public ImageFileServer(int Port)
        {
            port = Port;
        }

        /// <summary>
        /// TCP服务(文件服务)
        /// </summary>
        private TCPServer tcpFileServer = null;

        /// <summary>
        /// 图片文件中转服务
        /// </summary>
        private Dictionary<string, ServerImage> Images = null;
        /// <summary>
        /// 图片文件中转服务时钟
        /// </summary>
        private System.Timers.Timer TimerImageServer = null;

        /// <summary>
        /// 服务端口
        /// </summary>
        private int port = 0;

        #region 公共方法
        /// <summary>
        /// 开始服务
        /// </summary>
        public void Start()
        {
            #region 创建图片文件夹
            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath +"\\Images");
            if (!dInfo.Exists)
                dInfo.Create();
            #endregion

            if (Images == null)
                Images = new Dictionary<string, ServerImage>();

            if (TimerImageServer == null)
            {
                TimerImageServer = new System.Timers.Timer();
                TimerImageServer.Elapsed += new System.Timers.ElapsedEventHandler(TimerImageServer_Elapsed);
            }

            TimerImageServer.Interval = 60000;//设置图片文件缓存时间 （默认1分钟）
            TimerImageServer.Enabled = true;//开始服务

            if (tcpFileServer == null)
            {
                tcpFileServer = new TCPServer();
                tcpFileServer.SessionCreated += new EventHandler<TCP_ServerSessionEventArgs<TCPServerSession>>(tcpFileServer_SessionCreated);
                tcpFileServer.Bindings = new IPBindInfo[] { new IPBindInfo("127.0.0.1", BindInfoProtocol.TCP, IPAddress.Any, port) };
            }
            tcpFileServer.Start();
        }


        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (tcpFileServer != null && tcpFileServer.IsRunning)
            {
                tcpFileServer.Stop();
                tcpFileServer.Dispose();
                tcpFileServer = null;
            }
            if (TimerImageServer != null)
            {
                TimerImageServer.Enabled = false;//停止文件服务检测
                TimerImageServer.Dispose();
                TimerImageServer = null;
            }
            if (Images != null)
            {
                Images.Clear();
                Images = null;
            }
        }
        #endregion


        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (tcpFileServer != null && tcpFileServer.IsRunning)
                tcpFileServer.Stop();
               
            if (TimerImageServer != null)
                TimerImageServer.Enabled = false;//停止文件服务检测
        }
        #endregion

        #region 从内存字典中获取服务缓冲区图片文件
        /// <summary>
        /// 从内存字典中获取服务缓冲区图片文件
        /// </summary>
        /// <param name="MD5">图片MD5值</param>
        /// <returns></returns>
        private ServerImage getServerImage(string MD5)
        {
            ServerImage image = null;
            if (Images.ContainsKey(MD5)) //如果文件存在
                Images.TryGetValue(MD5, out image);
            return image;
        }
        #endregion

        #region TCP图片文件服务

        #region 图片缓冲服务周期
        private void TimerImageServer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string fileFullName = "";

            foreach (ServerImage serverImage in Images.Values)
                if (DateTime.Now > serverImage.LastActivity.AddHours(1))
                {
                    //如果文件有一小时未用，则删除内存信息并删除文件
                    fileFullName = System.Windows.Forms.Application.StartupPath + "\\Images\\" + serverImage.Name;
                    System.IO.File.Delete(fileFullName);//删除不用的文件

                    serverImage.Data = null;
                    Images.Remove(serverImage.Name);//删除并释放内存
                }
                else if (DateTime.Now > serverImage.LastActivity.AddMinutes (10))
                {
                    //如果文件10分钟未用，则清除内存中的文件内容，释放内存
                    serverImage.Data = null;
                    Images.Remove(serverImage.Name);//删除并释放内存
                }
                else if (DateTime.Now > serverImage.LastActivity.AddMinutes(2))//如果2分钟后文件未上传完成，则视为破图
                {
                    if (serverImage.Data.Length != serverImage.Length)
                    {
                        serverImage.Data = null;
                        Images.Remove(serverImage.Name);//删除并释放内存
                    }
                }
        }
        #endregion

        #region 发送消息到文件客户端
        /// <summary>
        /// 发送消息到文件客户端
        /// </summary>
        /// <param name="e"></param>
        private void sendFileMsg(Element e, TCPServerSession se)
        {
            if (se != null && se.IsConnected)
                tcpFileServer.SendMessageToSession(se, Factory.CreateXMLMsg(e));
        }
        #endregion

        #region SessionCreated
        private void tcpFileServer_SessionCreated(object sender, TCP_ServerSessionEventArgs<TCPServerSession> e)
        {
            e.Session.PacketReceived += new TCP_ServerSession.PacketReceivedEventHandler(fileSession_PacketReceived);
            e.Session.IsAuthenticated = true;
        }
        #endregion

        #region PacketReceived
        private void fileSession_PacketReceived(object sender, TcpSessionEventArgs e)
        {
            if (e.Data.Length < 2) return;
            TCPServerSession Session = sender as TCPServerSession;
            if (DateTime.Now > Session.ConnectTime.AddSeconds(20))
            {//如果客户端连接服务器的时间大于20秒，则视为非法攻击，断开联接
                Session.Disconnect();
                Session.Dispose();
            }

            object obj = Factory.CreateInstanceObject(e.Data);
            if (obj != null)//如果收到的消息对像不为空
            {
                if (obj is ImageFileMsg)//文件传输
                    onFileTransmit(obj as ImageFileMsg, Session);
            }
            else //收到非法消息
                OnBadCommand(Session);
        }
        #endregion

        #region TCP处理非法入侵
        /// <summary>
        /// 处理非法入侵
        /// </summary>
        /// <param name="session"></param>
        private void OnBadCommand(TCPServerSession session)
        {
            session.BadCmd++;
            if (session.BadCmd > 3)//4不过3，如果收到坏消息大于3条，则视为非法攻击，断掉TCP连接
            {
                session.Disconnect();
                session.Dispose();
            }
        }
        #endregion

        #region 文件传送
        /// <summary>
        /// 文件传送
        /// </summary>
        /// <param name="FileTransmit"></param>
        /// <param name="session"></param>
        private void onFileTransmit(ImageFileMsg FileMsg, TCPServerSession session)
        {
            if (FileMsg.Length > 1024000 || FileMsg.Length < 0) return;//如果所传输的文件大于1M或无大小则为非法请求退出

            ServerImage image = getServerImage(FileMsg.MD5);
            string fullFileName = System.Windows.Forms.Application.StartupPath + "\\Images\\" + FileMsg.MD5 + FileMsg.Extension;//标记文件路径

            #region 在内存中为文件创建空间
            if (FileMsg.type == type.New)//上传文件请求
            {
                if (image == null)//如果服务器内存中无此文件
                {
                    image = new ServerImage(FileMsg.MD5);//创建内存文件
                    Images.Add(image.MD5, image);//将内存文件添加到文件下载服务区
                    if (File.Exists(fullFileName))//如果文件已在服务器硬盘中
                    {
                        image.Data = OpFile.Read(fullFileName);//将文件数据全部读取到内存
                        image.Length = image.Data.Length;//设置准确的文件长度
                        image.uploadLength = image.Length;//标记文件已经全部上传完成
                        FileMsg.type = type.over;//标识文件已经传输完成，通知客户端停止上传
                        sendFileMsg(FileMsg, session);//通知客户端文件已经上传完成
                        session.Disconnect();//断开客户端的TCP连接
                    }
                    else
                    {
                        image.Extension = FileMsg.Extension;
                        image.Length = FileMsg.Length;
                        image.Name = FileMsg.Name;
                        image.userID = FileMsg.from;
                        image.Data = new byte[image.Length];//创建内存空间

                        FileMsg.type = type.set;//通知客户端上传
                        FileMsg.LastLength = 0;//上传位置从零开始
                        sendFileMsg(FileMsg, session);//通知客户端可以上传文件
                    }
                }
                else//如果服务器内存中有此文件
                {
                    FileMsg.type = type.over;//标识文件已经传输完成，通知客户端停止上传
                    sendFileMsg(FileMsg, session);//通知客户端文件已经上传完成
                    session.Disconnect();//断开客户端的TCP连接
                }
            }
            #endregion

            #region 下载文件
            if (FileMsg.type == type.get)//下载文件请求
            {
                if (image == null)  //如果内存中无文件
                    if (File.Exists(fullFileName))//如果文件已在服务器硬盘中
                    {
                        image = new ServerImage(FileMsg.MD5);//创建内存文件 
                        image.MD5 = FileMsg.MD5;
                        image.Data = OpFile.Read(fullFileName);//将文件数据全部读取到内存
                        image.Length = image.Data.Length;//设置准确的文件长度
                        image.uploadLength = image.Length;//标记文件已经全部上传完成
                        Images.Add(image.MD5, image);//将内存文件添加到文件下载服务区
                    }

                if (image != null && FileMsg.LastLength < image.Data.Length) //如果内存中有文件
                {
                    if (FileMsg.LastLength + 10240 > image.Data.Length)
                        FileMsg.fileBlock = new byte[image.Data.Length - FileMsg.LastLength];//要发送的缓冲区
                    else
                        FileMsg.fileBlock = new byte[10240];//要发送的缓冲区

                    Buffer.BlockCopy(image.Data, (int)FileMsg.LastLength, FileMsg.fileBlock, 0, FileMsg.fileBlock.Length);//将其保存于Buffer字节数组

                    FileMsg.type = type.get;//下载标记
                    FileMsg.LastLength += FileMsg.fileBlock.Length;
                    sendFileMsg(FileMsg, session);

                    if (FileMsg.LastLength == image.Data.Length)
                    {
                        FileMsg.type = type.over;//标记下载完成
                        FileMsg.fileBlock = null;
                        sendFileMsg(FileMsg, session);
                        session.Disconnect();
                    }
                }
            }
            #endregion

            #region 上传文件
            if (FileMsg.type == type.set)//上传文件内容
            {
                if (image.uploadLength + FileMsg.fileBlock.Length > image.Length) return;

                Buffer.BlockCopy(FileMsg.fileBlock, 0, image.Data, (int)image.uploadLength, FileMsg.fileBlock.Length);//将收到的数据保存于Buffer字节数组

                image.uploadLength += FileMsg.fileBlock.Length;//设置最后一次上传文件的末尾长度

                FileMsg.LastLength = image.uploadLength;//告诉客户端已上传的文件位置
                FileMsg.fileBlock = null;

                if (image.uploadLength == image.Length)//如果文件上传完成
                {
                    OpFile.Write(image.Data, fullFileName);
                    FileMsg.type = type.over;//标识文件已经传输完成，通知客户端停止上传
                    sendFileMsg(FileMsg, session);//通知客户端文件已经上传完成
                    session.Disconnect();//断开客户端的TCP连接
                }
                else
                    sendFileMsg(FileMsg, session);//通知客户端上传文件下一数据包
            }
            #endregion

        }
        #endregion

        #endregion


    }
}
