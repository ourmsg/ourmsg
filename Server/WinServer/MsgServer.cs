using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using IMLibrary3;
using IMLibrary3.IO;
using IMLibrary3.Net;
using IMLibrary3.Organization;
using IMLibrary3.Protocol;
using IMLibrary3.Security;
using IMLibrary3.Operation;
using IMLibrary3.Net.TCP;
using IMLibrary3.Net.UDP;

namespace OurMsgServer
{
    /// <summary>
    /// 消息服务器
    /// </summary>
    public class MsgServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Port"></param>
        public MsgServer(int Port) {
            port = Port;
        }

        #region 变量

        int port = 2;

        /// <summary>
        /// TCP服务(消息服务)
        /// </summary>
        private TCPServer tcpMessageServer = null;

        #endregion

        #region 事件
        /// <summary>
        /// 消息服务事件代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MsgServerEventHandler(object sender, MsgServerEventArgs e);

        /// <summary>
        /// 用户登录事件
        /// </summary>
        public event MsgServerEventHandler Login;
        /// <summary>
        /// 消息到达事件
        /// </summary>
        public event MsgServerEventHandler Message;
        /// <summary>
        /// 在线状态更改事件
        /// </summary>
        public event MsgServerEventHandler Presence;
        /// <summary>
        /// 用户请求获得联系人在线状的事件
        /// </summary>
        public event MsgServerEventHandler RequestUsersPresence;
        /// <summary>
        /// 用户请求下载分组信息
        /// </summary>
        public event MsgServerEventHandler RequestOrgGroups;
        /// <summary>
        /// 用户请求下载用户信息
        /// </summary>
        public event MsgServerEventHandler RequestOrgUsers;
        /// <summary>
        /// 用户请求下载群信息
        /// </summary>
        public event MsgServerEventHandler RequestOrgRooms;
        /// <summary>
        /// 用户请求创建、更新群信息
        /// </summary>
        public event MsgServerEventHandler RequestChangeRoom;
        /// <summary>
        /// 转发图片文件下载消息
        /// </summary>
        public event MsgServerEventHandler RouteDownLoadImageFile;
        /// <summary>
        /// 转发P2P文件传输消息
        /// </summary>
        public event MsgServerEventHandler RouteP2PFileTransmit;
        /// <summary>
        /// 转发MPG4编解码的视频对话消息
        /// </summary>
        public event MsgServerEventHandler RouteAVMsg;
        /// <summary>
        /// 管理员请求创建、更新用户信息
        /// </summary>
        public event MsgServerEventHandler RequestChangeUserVcard;
        /// <summary>
        /// 管理员请求创建、更新分组信息
        /// </summary>
        public event MsgServerEventHandler RequestChangeGroupVcard;
        /// <summary>
        /// 管理员请求修改密码
        /// </summary>
        public event MsgServerEventHandler RequestChangePassword;
        /// <summary>
        /// 用户下线事件
        /// </summary>
        public event MsgServerEventHandler UserOffline;
        #endregion

        #region 公共方法

        #region 开始服务
        /// <summary>
        /// 开始服务
        /// </summary>
        public void Start()
        {
            if (tcpMessageServer == null)
            {
                tcpMessageServer = new TCPServer();
                tcpMessageServer.SessionCreated += new EventHandler<TCP_ServerSessionEventArgs<TCPServerSession>>(tcpMessageServer_SessionCreated);
                tcpMessageServer.Bindings = new IPBindInfo[] { new IPBindInfo("127.0.0.1", BindInfoProtocol.TCP, IPAddress.Any, port) };
            }
            tcpMessageServer.Start();
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (tcpMessageServer != null && tcpMessageServer.IsRunning)
            {
                tcpMessageServer.Stop();
                tcpMessageServer.Dispose();
                tcpMessageServer = null;
            }
        }
        #endregion

        #region 停止服务
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (tcpMessageServer != null && tcpMessageServer.IsRunning)
                tcpMessageServer.Stop(); 
        }
        #endregion

        #region 获得客户端连接套接字集合
        public TCP_ServerSession [] GetTcpServerSession()
        {
            return tcpMessageServer.Sessions.ToArray();
        }
        #endregion

        #region TCP发送消息
        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="e">Element</param>
        public void BroadcastingMessage(object e)
        {
            tcpMessageServer.BroadcastingMessage(e);
        }

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="XMLMsg"></param>
        public void BroadcastingMessage(string XMLMsg)
        {
            tcpMessageServer.BroadcastingMessage(XMLMsg);
        }


        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="Session">用户</param>
        /// <param name="Message">消息文本</param>
        public void SendMessage(TCPServerSession Session, string XMLMsg)
        {
            tcpMessageServer.SendMessageToSession(Session, XMLMsg);
        }

        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="Session">TCP客户端</param>
        /// <param name="e">object</param>
        public void SendMessage(TCPServerSession Session, object e)
        {
            tcpMessageServer.SendMessageToSession(Session, Factory.CreateXMLMsg(e));
        }
        #endregion

        #endregion

        #region TCP创建连接
        private void tcpMessageServer_SessionCreated(object sender, TCP_ServerSessionEventArgs<TCPServerSession> e)
        {
            e.Session.PacketReceived += new TCP_ServerSession.PacketReceivedEventHandler(messageSession_PacketReceived);
            e.Session.Disonnected += new EventHandler(Session_Disonnected);
        }
        #endregion 

        #region TCP连接断开事件
        /// <summary>
        /// TCP连接断开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Session_Disonnected(object sender, EventArgs e)
        {
            TCPServerSession session = sender as TCPServerSession;
            if (session != null && session.IsAuthenticated)//如果该用户已经成功验证登录
            {
                UserVcard user = session.Tag as UserVcard;
                if (user != null)
                {
                    if(UserOffline !=null )
                        UserOffline(this, new MsgServerEventArgs(session, "", null, user));//触发用户离线事件
                }
            } 
        }
        #endregion

        #region TCP连接数据包到达事件
        /// <summary>
        /// TCP连接数据包到达事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageSession_PacketReceived(object sender, TcpSessionEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (e.Data.Length < 2) return;

            TCPServerSession Session = sender as TCPServerSession;

            object obj = Factory.CreateInstanceObject(e.Data);
            if (obj != null)//如果收到的消息对像不为空
            {
                if (obj is Auth)//登录请求
                {
                    if (Login != null)
                        Login(this, new MsgServerEventArgs(Session, e.Data, obj, null));
                }
                else if (Session.IsAuthenticated)//如果提供其他服务客户端必须是已经进行验证后
                {
                    UserVcard RequestUser = Session.Tag as UserVcard;//获得请求用户
                    if (RequestUser == null) return;//如果用户为空，退出

                    if (obj is Message)//请求转发消息
                    {
                        if (Message != null)
                            Message(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is Presence)//请求在线状态
                    {
                        if ((obj as Presence).type == type.set)//设置在线状态
                        {
                            if (Presence != null)
                                Presence(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                        }
                        else if ((obj as Presence).type == type.get)//获得联系人在线状态
                        {
                            if (RequestUsersPresence != null)
                                RequestUsersPresence(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                        }
                    }
                    else if (obj is DownloadGroups)//请求下载的分组信息
                    {
                        if (RequestOrgGroups != null)
                            RequestOrgGroups(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is DownloadUsers)//请求下载用户信息
                    {
                        if (RequestOrgUsers != null)
                            RequestOrgUsers(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is DownloadRooms)//请求下载群信息
                    {
                        if (RequestOrgRooms != null)
                            RequestOrgRooms(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is ChangeRoom)//更新群信息
                    {
                        if (RequestChangeRoom != null)
                            RequestChangeRoom(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is ImageFileMsg )//通知客户端到文件服务器下载已上传完成的文件
                    {
                        if (RouteDownLoadImageFile != null)
                            RouteDownLoadImageFile(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is P2PFileMsg )//转发文件传输消息
                    {
                        if (RouteP2PFileTransmit != null)
                            RouteP2PFileTransmit(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if(obj is AVMsg)//转发音视频对话的消息
                        if (RouteAVMsg != null)
                            RouteAVMsg(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));


                    #region 组织架构信息管理
                    if (obj is UserVcard)//如果是管理员变更用户信息资料
                    {
                        if (RequestChangeUserVcard != null)
                            RequestChangeUserVcard(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is GroupVcard)//如果是管理员变更分组信息资料
                    {
                        if (RequestChangeGroupVcard != null)
                            RequestChangeGroupVcard(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    else if (obj is ChangePassword)//如果是要求修改密码
                    {
                        if (RequestChangePassword != null)
                            RequestChangePassword(this, new MsgServerEventArgs(Session, e.Data, obj, RequestUser));
                    }
                    #endregion

                }
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
         
    }

    #region 事件参数
    public class MsgServerEventArgs : EventArgs 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Session">TCP客户端</param>
        /// <param name="XMLMsg">XML消息字符串</param>
        /// <param name="obj">消息转换的对像</param>
        /// <param name="RequestUser">请求用户</param>
        public MsgServerEventArgs(TCPServerSession Session, string XMLMsg, object obj, UserVcard RequestUser)
        {
            this.Session = Session;
            this.XMLMsg = XMLMsg;
            this.obj = obj;
            this.RequestUser = RequestUser;
        }

        /// <summary>
        /// TCP客户端
        /// </summary>
        public TCPServerSession Session { get; set; }

        /// <summary>
        /// XML消息字符串
        /// </summary>
        public string XMLMsg { get; set; }

        /// <summary>
        /// 消息转换的对像
        /// </summary>
        public object obj { get; set; }

        /// <summary>
        /// 请求用户
        /// </summary>
        public UserVcard RequestUser { get; set; }
    }
    #endregion 
}
