using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net ;
 
using IMLibrary3;
using IMLibrary3.IO;
using IMLibrary3.Net;
using IMLibrary3.Server;
using IMLibrary3.Organization ;
using IMLibrary3.Protocol;
using IMLibrary3.Security;
using IMLibrary3.Operation;
using IMLibrary3.Net.TCP;
using IMLibrary3.Net.UDP;


namespace OurMsgServer
{ 
    public class Server
    {
        /// <summary>
        /// 
        /// </summary>
        public Server()
        {

        }

        #region 变量
        /// <summary>
        /// TCP服务(消息服务)
        /// </summary>
        //private TCPServer tcpMessageServer = null;

        /// <summary>
        /// 消息服务器
        /// </summary>
        private MsgServer msgServer = null;

        /// <summary>
        /// 所有用户列表
        /// </summary>
        private Dictionary<string, UserVcard> Users = null;

        /// <summary>
        /// 在线用户列表
        /// </summary>
        private Dictionary<string, UserVcard> OnlineUsers = new Dictionary<string,UserVcard>();

        /// <summary>
        /// 群组列表
        /// </summary>
        private Dictionary<string,  Room> Rooms = null;
       
        /// <summary>
        /// 组织机构版本号
        /// </summary>
        private IMLibrary3.Protocol.OrgVersion orgVersion =null ;

  


        /// <summary>
        /// UDP文件传输服务器
        /// </summary>
        private P2PFileServer p2pFileServer = null;
        /// <summary>
        /// 图片文件传输服务器
        /// </summary>
        private ImageFileServer imageFileServer = null;
        /// <summary>
        /// 音视频传输服务器
        /// </summary>
        private P2PAVServer p2pAVServer = null;
        #endregion
         
        #region 开始服务
        /// <summary>
        /// 开始服务
        /// </summary>
        /// <param name="P2PFileServerPort">P2P服务传输服务端口(UDP)</param>
        /// <param name="ImageFileServerPort">图片文件传输服务端口(TCP)</param>
        /// <param name="MessageServerPort">消息服务端口(TCP)</param>
        /// <param name="P2PAVServerPort">音视频服务端口(UDP)</param>
        public void Start(int P2PFileServerPort, int P2PAVServerPort, int ImageFileServerPort, int MessageServerPort)
        {
            #region 初始化组织架构

            ///初始化数据库连接字符串
            IMLibrary.SqlData.DataAccess.ConnectionString = DBHelper.settings.SqlConStr;


            /// 初始化用户
            if (Users == null)
                Users = DBHelper.GetUserVcards();


            /// 获取组织机构版本和信息
            {
                orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
            }

            #region  初始化群
            if (Rooms == null)
            {
                Rooms = DBHelper.GetRooms();
                foreach (Room room in Rooms.Values)
                {
                    #region 将内存中的用户添加到内存中的群
                    string[] userids = room.UserIDs.Split(';');
                    foreach (string userID in userids)
                    {
                        UserVcard user = getUser(userID);
                        if (user != null)
                        {
                            if (room.Users == null) room.Users = new Dictionary<string, User>();
                            if (!room.Users.ContainsKey(userID))
                            {
                                if (room.CreateUserID == userID) user.CreatedRoomsCount++;//标记用户已创建群数
                                room.Users.Add(user.UserID, user);
                            }

                            if (user.Rooms == null) user.Rooms = new Dictionary<string, Room>();
                            if (!user.Rooms.ContainsKey(room.RoomID))
                                user.Rooms.Add(room.RoomID, room);
                        }
                    }

                    room.UserIDs = "";//重新生成群包含的Users
                    foreach (User u in GetUsers(room))
                        room.UserIDs += u.UserID + ";";

                    #endregion
                }
            }
            #endregion

            #endregion

            //启动消息服务
            if (msgServer == null)
            {
                msgServer = new MsgServer(MessageServerPort);
                msgServer.Login += new MsgServer.MsgServerEventHandler(msgServer_Login);
                msgServer.UserOffline += new MsgServer.MsgServerEventHandler(msgServer_UserOffline);
                msgServer.Message += new MsgServer.MsgServerEventHandler(msgServer_Message);
                msgServer.Presence += new MsgServer.MsgServerEventHandler(msgServer_Presence);
                msgServer.RequestChangeGroupVcard += new MsgServer.MsgServerEventHandler(msgServer_RequestChangeGroupVcard);
                msgServer.RequestChangePassword += new MsgServer.MsgServerEventHandler(msgServer_RequestChangePassword);
                msgServer.RequestChangeRoom += new MsgServer.MsgServerEventHandler(msgServer_RequestChangeRoom);
                msgServer.RequestChangeUserVcard += new MsgServer.MsgServerEventHandler(msgServer_RequestChangeUserVcard);
                msgServer.RequestOrgGroups += new MsgServer.MsgServerEventHandler(msgServer_RequestOrgGroups);
                msgServer.RequestOrgRooms += new MsgServer.MsgServerEventHandler(msgServer_RequestOrgRooms);
                msgServer.RequestOrgUsers += new MsgServer.MsgServerEventHandler(msgServer_RequestOrgUsers);
                msgServer.RequestUsersPresence += new MsgServer.MsgServerEventHandler(msgServer_RequestUsersPresence);
                msgServer.RouteDownLoadImageFile += new MsgServer.MsgServerEventHandler(msgServer_RouteDownLoadImageFile);
                msgServer.RouteP2PFileTransmit += new MsgServer.MsgServerEventHandler(msgServer_RouteP2PFileTransmit);
                msgServer.RouteAVMsg += new MsgServer.MsgServerEventHandler(msgServer_RouteAVMsg);
            }
            msgServer.Start();

            //启动P2P文件服务
            if (p2pFileServer == null) p2pFileServer = new P2PFileServer(P2PFileServerPort);
            p2pFileServer.Start();

            //启动图片文件服务
            if (imageFileServer == null) imageFileServer = new ImageFileServer(ImageFileServerPort);
            imageFileServer.Start();

            //启动音视频传输服务 
            if (p2pAVServer == null) p2pAVServer = new P2PAVServer(P2PAVServerPort);
            p2pAVServer.Start();
        }
        #endregion 

        #region 停止服务
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            msgServer.Stop();
            imageFileServer.Stop();
            p2pFileServer.Stop();
            p2pAVServer.Stop();
        }
        #endregion
         
        #region TCP发送消息

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="e">Element</param>
        public void BroadcastingMessageToOnlineUser(Element e)
        {
            msgServer.BroadcastingMessage(e);
        }

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="XMLMsg"></param>
        public void BroadcastingMessageToOnlineUser(string XMLMsg)
        {
            msgServer.BroadcastingMessage(XMLMsg);
        }


        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="Message">消息文本</param>
        private void SendMessageToUser(string userID, string XMLMsg)
        {
            UserVcard user = getUser(userID);
            SendMessageToUser(user, XMLMsg);
        }

        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="e">Element</param>
        private void SendMessageToUser(string userID, Element e)
        {
            UserVcard user = getUser(userID);
            SendMessageToUser(user, e);
        }


        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="Message">消息文本</param>
        public void SendMessageToUser(User user, string XMLMsg)
        {
            if (user != null && user.Tag is TCPServerSession)
            {
                msgServer.SendMessage(user.Tag as TCPServerSession, XMLMsg);
            }
        }

        /// <summary>
        /// 发送消息给一个用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="e">Element</param>
        public void SendMessageToUser(User user, Element e)
        {
            if (user.ShowType == IMLibrary3.Enmu.ShowType.Offline && e is Message)
                DBHelper.addMessageToDB(e as Message);

            if (user != null && user.Tag is TCP_ServerSession)
            {
                msgServer.SendMessage(user.Tag as TCPServerSession, e);
            }
        }

        /// <summary>
        /// 发送消息到群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roomID"></param>
        /// <param name="XMLMsg"></param>
        public void SendMessageToRoom(string userID, string roomID, string XMLMsg)
        {
            Room room = getRoom(roomID);
            if (room == null) return;//如果群不存在

            SendMessageToRoom(userID, room, XMLMsg);
        }

        /// <summary>
        /// 发送消息到群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roomID"></param>
        /// <param name="msg"></param>
        public void SendMessageToRoom(string userID, string roomID, Message msg)
        {
            string xmlmsg = Factory.CreateXMLMsg(msg);
            SendMessageToRoom(userID, roomID, xmlmsg);
        }

        /// <summary>
        /// 发送消息到群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="room"></param>
        /// <param name="XMLMsg"></param>
        public void SendMessageToRoom(string userID, Room room, string XMLMsg)
        {
            if (!room.Users.ContainsKey(userID)) return;//如果群中无此用户,则不发送消息

            foreach (User user in GetUsers(room))
            {
                if (user.ShowType == IMLibrary3.Enmu.ShowType.Offline)
                    DBHelper.addMessageToDB(user.UserID, userID, room.RoomID, IMLibrary3.Enmu.MessageType.Group, XMLMsg);
                else
                    SendMessageToUser(user, XMLMsg);
            }
        }
        #endregion

        #region 消息事件

        #region 路由音视频对话消息
        void msgServer_RouteAVMsg(object sender, MsgServerEventArgs e)
        {
            IMLibrary3.Protocol.AVMsg  msg = e.obj as IMLibrary3.Protocol.AVMsg;
            SendMessageToUser(msg.to, e.XMLMsg);
        }
        #endregion

        #region 路由客户端文件传输消息
        void msgServer_RouteP2PFileTransmit(object sender, MsgServerEventArgs e)
        {
            IMLibrary3.Protocol.P2PFileMsg msg = e.obj as IMLibrary3.Protocol.P2PFileMsg;
            SendMessageToUser(msg.to, e.XMLMsg);
        }
        #endregion

        #region 通知客户端下载图片文件事件
        void msgServer_RouteDownLoadImageFile(object sender, MsgServerEventArgs e)
        {
            IMLibrary3.Protocol.ImageFileMsg msg = e.obj as IMLibrary3.Protocol.ImageFileMsg;

            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果消息发送给用户
            {
                SendMessageToUser(msg.to, e.XMLMsg);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)
            {
                SendMessageToRoom(msg.from, msg.to, e.XMLMsg);
            }
        }
        #endregion

        #region 请求下载用户在线状态事件
        void msgServer_RequestUsersPresence(object sender, MsgServerEventArgs e)
        {
            int i = 0;
            Element element =new Element();

            ///将在线用户的ShowType发送给登录用户
            foreach (TCPServerSession se in msgServer.GetTcpServerSession())
            {
                if (se.IsAuthenticated)//如果是已认证的TCP客户端
                {
                    UserVcard userTemp = se.Tag as UserVcard;//获得TCP客户端绑定的用户
                    Presence pretemp = new Presence();
                    pretemp.from = userTemp.UserID;
                    pretemp.ShowType = userTemp.ShowType;
                    pretemp.Status = pretemp.Status;

                    element.Data.Add(pretemp);
                }
                i++;

                if (i == 80)//一次发送80个用户
                {
                    SendMessageToUser(e.RequestUser, element);
                    i = 0;
                    element = new Element();
                }
            }
            if (i>0)//最一次发送在线用户状态
                SendMessageToUser(e.RequestUser, element);
                 

            ///广播登录用户上线消息 
            Presence pre = new Presence();
            pre.from = e.RequestUser.UserID;
            pre.ShowType = e.RequestUser.ShowType;
            BroadcastingMessageToOnlineUser(pre);


            //发送离线消息
            List<string> offlineMsg = DBHelper.GetOfflineMessage(e.RequestUser.UserID, 50);//获取最后50条离线消息
            foreach (string msgXML in offlineMsg)
                SendMessageToUser(e.RequestUser, msgXML);
        }
        #endregion

        #region 请求下载用户信息事件
        void msgServer_RequestOrgUsers(object sender, MsgServerEventArgs e)
        {
            #region 新算法
            foreach (string str in orgVersion.UsersXML.ToArray())
            {
                SendMessageToUser(e.RequestUser, str);//发送用户信息
            }
            #endregion
        }
        #endregion

        #region 请求下载群信息事件
        void msgServer_RequestOrgRooms(object sender, MsgServerEventArgs e)
        {
            DownloadRooms dRooms = e.obj as DownloadRooms;

            dRooms.to = e.RequestUser.UserID; dRooms.from = "";

            int i = 0;
            foreach (Room room in GetRooms(e.RequestUser))
            {
                dRooms.Data.Add(room);
                i++;
                if (i == 5)//每次发送5个群信息
                {
                    SendMessageToUser(e.RequestUser, dRooms);//发送群信息
                    dRooms.Data = new List<object>();//用户信息清零
                    i = 0;
                }
            }
            if (dRooms.Data.Count > 0)//发送剩下的群信息
            {
                SendMessageToUser(e.RequestUser, dRooms);//发送群信息
            }
        }
        #endregion

        #region 请求下载分组信息事件
        void msgServer_RequestOrgGroups(object sender, MsgServerEventArgs e)
        {
            #region 新算法
            foreach (string str in orgVersion.GroupsXML.ToArray())
            {
                SendMessageToUser(e.RequestUser, str);//发送用户信息
            }
            #endregion
        }
        #endregion

        #region 请求创建、更新用户资料事件
        void msgServer_RequestChangeUserVcard(object sender, MsgServerEventArgs e)
        {
            UserVcard card = e.obj as UserVcard;
            if (card.UserID.Trim() == "" || card.UserName.Trim() == "" || card.GroupID.Trim() == "") return;
            card.UserID = card.UserID.Trim();

            UserVcard user = getUser(card.UserID);

            if (card.type == type.New && user == null && e.RequestUser.isAdmin)//如果用户不存在，创建新用户
            {
                Users.Add(card.UserID, card);
                //设置新用户密码为123456;
                card.Password = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes("123456"));

                DBHelper.CreateUserVcard(card);//保存于数据库
                orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息

                BroadcastingMessageToOnlineUser(e.XMLMsg);//将创建的新用户信息广播给所有在线用户  
            }
            else if (card.type == type.set && e.RequestUser.isAdmin)//如果是更新用户
            {
                if (user == null) return;//如果用户不存在则退出

                string oldInfo = user.UserName + user.GroupID + user.OrderID;//老信息
                string newInfo = card.UserName.Trim() + card.GroupID.Trim() + card.OrderID; //新信息

                user.UserName = card.UserName.Trim();
                user.GroupID = card.GroupID.Trim();
                user.OrderID = card.OrderID;
                user.Job = card.Job.Trim();
                user.OfficePhone = card.OfficePhone.Trim();
                user.Mail = card.Mail.Trim();
                user.Phone = card.Phone.Trim();
                user.Post = card.Post.Trim();
                user.Remark = card.Remark.Trim();
                user.Birthday = card.Birthday.Trim();
                user.Sex = card.Sex;
                user.CreateRooms = card.CreateRooms;
                user.Disable = card.Disable;
                user.isAdmin = card.isAdmin;
                user.isSendNotice = card.isSendNotice;
                user.isSendSMS = card.isSendSMS;

                DBHelper.UpdateUserVcard(user);//更新数据库
                if (oldInfo != newInfo)//如果用户基本信息发生变更，则变更组织架构
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息

                SendMessageToUser(e.RequestUser, e.XMLMsg);//通知管理员创建新用户成功 
            }
            else if (card.type == type.get)//如果是获取用户资料
            {
                if (user == null) return;//如果用户不存在则退出
                user.type = type.result;
                SendMessageToUser(e.RequestUser, Factory.CreateXMLMsg(user));//将用户资料发送给请求者
            }
            else if (card.type == type.delete && e.RequestUser.isAdmin)//如果是删除用户
            {
                if (user == null) return;//如果用户不存在则退出
                if (e.RequestUser.UserID == user.UserID) return;//如果管理员想删除自己，退出
                TCPServerSession se = user.Tag as TCPServerSession;
                if (se != null && se.IsConnected) { se.Disconnect(); se.Dispose(); }//如果删除的用户在线，则将其踢出
                Users.Remove(card.UserID);

                DBHelper.DelUser(card.UserID);//数据库中删除
                orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息

                BroadcastingMessageToOnlineUser(e.XMLMsg);//将删除的用户广播给所有在线用户 
            }
        }
        #endregion

        #region 创建、更改群事件
        void msgServer_RequestChangeRoom(object sender, MsgServerEventArgs e)
        {
            ChangeRoom changeRoom = e.obj as ChangeRoom;

            if (changeRoom.type == type.New && e.RequestUser.CreatedRoomsCount < e.RequestUser.CreateRooms)//如果请求创建群
            {
                #region 创建群

                //10次随机产生群号(所谓摇号)，如果10次摇出的群号都已被创建，则退出并表示服务器忙 
                //10都不能摇到空号表示用户运气不好，下次再试！
                for (int i = 0; i < 10; i++)
                {
                    Random ran = new Random();
                    int RandKey = ran.Next(10000, 1000000000);//开始摇号，群号码从10000至1000000000随机产生
                    if (!Rooms.ContainsKey(RandKey.ToString()))//如果是空号，以此号创建群
                    {
                        Room room = new Room();
                        room.RoomID = RandKey.ToString();
                        room.RoomName = changeRoom.RoomName;
                        room.Notice = changeRoom.Notice;
                        room.UserIDs = changeRoom.UserIDs;
                        room.CreateUserID =e.RequestUser.UserID;//创建者为请求者

                        e.RequestUser.CreatedRoomsCount++;//标记用户创建群数

                        #region 将内存中的用户添加到内存中的群
                        string[] userids = room.UserIDs.Split(';');
                        foreach (string userID in userids)
                        {
                            UserVcard user = getUser(userID);
                            if (user != null)
                            {
                                if (room.Users == null) room.Users = new Dictionary<string, User>();
                                if (!room.Users.ContainsKey(userID))
                                    room.Users.Add(user.UserID, user);

                                if (user.Rooms == null) user.Rooms = new Dictionary<string, Room>();
                                if (!user.Rooms.ContainsKey(room.RoomID))
                                    user.Rooms.Add(room.RoomID, room);
                            }
                        }

                        room.UserIDs = "";//重新生成群包含的Users
                        foreach (User u in GetUsers(room))
                            room.UserIDs += u.UserID + ";";
                        #endregion

                        Rooms.Add(room.RoomID, room);//将创建的群添加到内存

                        DBHelper.CreateRoom(room);//将创建的群添加到数据库

                        changeRoom.RoomID = room.RoomID;
                        changeRoom.CreateUserID = room.CreateUserID;

                        SendMessageToRoom(room.CreateUserID, room, Factory.CreateXMLMsg(changeRoom));//发送消息到刚创建的群，通知群里的用户已经加入群

                        return;//创建群成功，退出
                    }
                }
                #endregion
            }
            else if (changeRoom.type == type.set)//请求更新群
            {
                #region 更新群
                Room room = getRoom(changeRoom.RoomID);
                if (room != null && room.CreateUserID ==e.RequestUser.UserID)//如果群创建者为请求用户
                {
                    string oldVersion = room.RoomName.Trim() + room.Notice.Trim() + room.UserIDs;
                    changeRoom.CreateUserID = room.CreateUserID;
                    room.Notice = changeRoom.Notice;
                    room.RoomName = changeRoom.RoomName;
                    string[] newUserids = changeRoom.UserIDs.Split(';');

                    #region 查找群中被删除的用户

                    changeRoom.type = type.delete;//标识删除群
                    changeRoom.UserIDs = null;//不要将新群包含的用户通知退出群的用户

                    string delRoomMsg = IMLibrary3.Protocol.Factory.CreateXMLMsg(changeRoom);
                    bool t = false;
                    foreach (UserVcard user in GetUsers(room))
                    {
                        t = false;
                        foreach (string userID in newUserids)
                            if (user.UserID == userID)//如果群里的用户在更新后的用户集合中存在，则表示未删除
                                t = true;

                        if (!t)//如果更新的用户集合中没有当前用户，则表示从群中删除此用户
                        {
                            room.Users.Remove(user.UserID);
                            user.Rooms.Remove(room.RoomID);
                            SendMessageToUser(user, delRoomMsg);//通知用户退出群
                        }
                    }
                    #endregion

                    #region 添加新用户记录群中新用户
                    foreach (string userID in newUserids)
                    {
                        UserVcard user = getUser(userID);//获取新用户
                        if (user != null)
                        {
                            if (!room.Users.ContainsKey(user.UserID))
                            {
                                room.Users.Add(user.UserID, user);//如果群里无此用户，则新增用户
                                if (!user.Rooms.ContainsKey(room.RoomID))
                                    user.Rooms.Add(room.RoomID, room);//如果用户不在此群，则为用户新增群

                            }
                        }
                    }
                    #endregion

                    room.UserIDs = "";//重新生成群包含的Users
                    foreach (User u in GetUsers(room))
                        room.UserIDs += u.UserID + ";";

                    changeRoom.type = type.set;//标识群信息被成功修改
                    changeRoom.UserIDs = room.UserIDs;//设置最新用户列表

                    string newVersion = room.RoomName.Trim() + room.Notice.Trim() + room.UserIDs;

                    if (oldVersion == newVersion) return;//如果没有做任何更改
                    SendMessageToRoom(room.CreateUserID, room, Factory.CreateXMLMsg(changeRoom));//通知群内原用户，群信息已经修改

                    DBHelper.UpdateRoom(room);
                }
                #endregion
            }
        }
        #endregion

        #region 修改密码事件
        void msgServer_RequestChangePassword(object sender, MsgServerEventArgs e)
        {
            ChangePassword changePWD = e.obj as ChangePassword;

            if (changePWD.NewPassword.Trim() == "") return;

            UserVcard user = getUser(changePWD.from);
            if (user == null) return;

            ///将密码Hash为MD5 
            changePWD.OldPassword = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes(changePWD.OldPassword.Trim()));
            changePWD.NewPassword = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes(changePWD.NewPassword.Trim()));

            if (e.RequestUser.isAdmin || changePWD.OldPassword == user.Password)//如果用户是管理员
            {
                if (DBHelper.UpdatePassword(user.UserID, changePWD.NewPassword) > 0)//更新密码
                {
                    user.Password = changePWD.NewPassword;
                    SendMessageToUser(e.RequestUser, e.XMLMsg);//通知用户更新密码成功  
                }
            }
        }
        #endregion

        #region 更改分组信息事件
        void msgServer_RequestChangeGroupVcard(object sender, MsgServerEventArgs e)
        {
            GroupVcard card = e.obj as GroupVcard;

            if (card.GroupID.Trim() == "" || card.GroupName.Trim() == "") return;
            card.GroupID = card.GroupID.Trim();

            if (card.type == type.New && e.RequestUser.isAdmin)//如果用户是管理员
            {
                if (DBHelper.CreateGroupVcard(card) > 0)//保存于数据库
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    BroadcastingMessageToOnlineUser(e.XMLMsg);//将创建的分组信息广播给所有在线用户  
                }
            }
            else if (card.type == type.set && e.RequestUser.isAdmin)//如果是更新分组信息
            {
                if (DBHelper.UpdateGroupVcard(card) > 0) //更新数据库
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    SendMessageToUser(e.RequestUser, e.XMLMsg);//通知管理员创建新用户成功 
                }
            }
            else if (card.type == type.get)//如果是更新 
            {
                card.type = type.result;
                SendMessageToUser(e.RequestUser, Factory.CreateXMLMsg(card));//将分组信息资料发送给请求者
            }
            else if (card.type == type.delete && e.RequestUser.isAdmin)//如果是删除分组
            {
                if (DBHelper.DelGroup(card.GroupID) > 0) //数据库中删除
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    BroadcastingMessageToOnlineUser(e.XMLMsg);//将删除的用户广播给所有在线用户 
                }
            }
        }
        #endregion

        #region 更改在线状态事件
        void msgServer_Presence(object sender, MsgServerEventArgs e)
        {
            Presence pre = e.obj as Presence;
            e.RequestUser.ShowType = pre.ShowType;//更改在线状态

            if (pre.ShowType == IMLibrary3.Enmu.ShowType.Invisible)//如果用户隐身，则将用户状态设置为离线发送给其他用户
                pre.ShowType = IMLibrary3.Enmu.ShowType.Offline;

            BroadcastingMessageToOnlineUser(pre);
        }
        #endregion

        #region 消息到达事件
        void msgServer_Message(object sender, MsgServerEventArgs e)
        {
            IMLibrary3.Protocol.Message msg = e.obj as IMLibrary3.Protocol.Message;

            msg.DateTime = DateTime.Now.ToString();//将消息发送时间设置为服务器的时间 

            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果消息发送给用户
            {
                SendMessageToUser(msg.to, msg);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)//如果消息发送给群
            {
                SendMessageToRoom(msg.from, msg.to, msg);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Notice)//如果发送通知消息给多个用户
            {
                string[] users = msg.to.Split(';');//获得要接收消息的用户数据
                if (users.Length > 0)
                    foreach (string userID in users)
                    {
                        msg.to = userID;
                        SendMessageToUser(userID, msg);
                    }
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.broadcasting)//如果发送通知消息给所有用户
                BroadcastingMessageToOnlineUser(msg);
        }
        #endregion

        #region 用户离线事件
        void msgServer_UserOffline(object sender, MsgServerEventArgs e)
        { 
            Presence presence = new Presence();
            presence.from = e.RequestUser.UserID;
            presence.ShowType = IMLibrary3.Enmu.ShowType.Offline;
            BroadcastingMessageToOnlineUser(presence);//告诉在线者用户已经离线
        }
        #endregion

        #region 登录事件
        void msgServer_Login(object sender, MsgServerEventArgs e)
        {
            Auth auth = e.obj as Auth;
            UserVcard user = getUser(auth.UserID);

            if (user != null && !user.Disable && Hasher.GetMD5Hash(TextEncoder.textToBytes(auth.Password)) == user.Password) //如果用户存在未禁用且密码正确
            {
                #region 重复登录或异地登录
                if (user.Tag != null && user.Tag is TCPServerSession)
                {
                    ///通知用户此帐号异地登录
                    auth.type = type.Else;
                    auth.Password = e.Session.ID;
                    TCPServerSession se = user.Tag as TCPServerSession;
                    if (se != null && !se.IsDisposed && se.IsConnected)
                    {
                        SendMessageToUser(user, auth);
                        se.Disconnect();//断开连接，释放资源
                    }
                }
                #endregion

                #region 不能修改这个代码
                e.Session.Tag = user;//这个代码是必须的
                e.Session.IsAuthenticated = true;//这个代码是必须的
                #endregion

                user.Tag = e.Session;
                user.ShowType = auth.ShowType;
                user.Status = auth.Status;

                ///发送登录成功消息
                auth.UserName = user.UserName;//告之登录用户的姓名
                auth.type = type.result;//告之登录用户成功登录
                auth.Password = e.Session.ID;//告之登录用户在服务器的SESSION
                auth.LastDateTime = user.LastDateTime;//告之登录用户上次登录时间
                auth.LastIP = user.LastIP;//告之登录用户上次登录IP
                auth.FileServerTCPPort = DBHelper.settings.TcpFilePort;//告之登录用户TCP文件传输服务端口
                auth.FileServerUDPPort = DBHelper.settings.UdpFilePort;//告之登录用户UDP文件传输服务端口
                auth.AVServerUDPPort = DBHelper.settings.UdpAVPort;//告之登录用户UDP音视频传输服务端口
                auth.isAdmin = user.isAdmin;//告之登录用户是否管理员
                auth.isBroadcast = user.isBroadcast;//告之登录用户是否可以发送广播消息
                auth.isSendNotice = user.isSendNotice;//告之登录用户是否可以发送通知消息
                auth.isSendSMS = user.isSendSMS;//告之登录用户是否可以发送手机短信
                auth.isEditUserData = user.isEditUserData;//告之登录用户是否可以编辑用户资料

                user.LastDateTime = DateTime.Now.ToString();
                user.LastIP = e.Session.RemoteEndPoint.Address.ToString();

                //通知用户登录成功
                SendMessageToUser(user, auth);

                //发送企业组织机构版本信息给用户
                orgVersion.RoomsCount = user.Rooms.Count;//用户加入群的数量
                SendMessageToUser(user, orgVersion);

            }
            else//如果用户不存在或密码错误
            {
                ///发送密码错误消息
                auth.type = type.error;
                auth.Password = e.Session.ID;
                msgServer.SendMessage(e.Session, auth);
                e.Session.Disconnect();//断开连接
                e.Session.Dispose();
            }
        }
        #endregion

        #endregion

        #region 字典查询

        #region 从内存字典中获取用户
        /// <summary>
        /// 从内存字典中获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private UserVcard getUser(string userID)
        {
            UserVcard user = null;
            if (Users.ContainsKey(userID.Trim())) //如果用户存在
                Users.TryGetValue(userID.Trim(), out user);
            return user;
        }
        #endregion

        #region 从内存字典中获取群
        /// <summary>
        /// 从内存字典中获取群
        /// </summary>
        /// <param name="roomID">群编号</param>
        /// <returns></returns>
        private Room getRoom(string roomID)
        {
            Room room = null;
            if (Rooms.ContainsKey(roomID.Trim())) //如果用户存在
                Rooms.TryGetValue(roomID.Trim(), out room);
            return room;
        }
        #endregion

        #region 从群中获取用户集合数组
        /// <summary>
        /// 从群中获取用户集合数组
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        private UserVcard[] GetUsers(Room room)
        {
            lock (room.Users)//确保线程安全
            {
                UserVcard[] Users = new UserVcard[room.Users.Count];
                room.Users.Values.CopyTo(Users, 0);
                return Users;
            }
        }
        #endregion

        #region 从用户中获取加入的群集合数组
        /// <summary>
        /// 从用户中获取加入的群集合数组
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Room[] GetRooms(UserVcard user)
        {
            lock (user.Rooms)//确保线程安全
            {
                Room[] Rooms = new Room[user.Rooms.Count];
                user.Rooms.Values.CopyTo(Rooms, 0);
                return Rooms;
            }
        }
        #endregion

        #endregion

    }
}
