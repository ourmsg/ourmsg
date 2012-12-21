using System;
using System.Collections.Generic;
using System.Text;


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
    public class Class1
    {
        #region 变量
        /// <summary>
        /// TCP服务(消息服务)
        /// </summary>
        private TCPServer tcpMessageServer = null;

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
        private Dictionary<string, UserVcard> OnlineUsers = new Dictionary<string, UserVcard>();

        /// <summary>
        /// 群组列表
        /// </summary>
        private Dictionary<string, Room> Rooms = null;

        /// <summary>
        /// 组织机构版本号
        /// </summary>
        private IMLibrary3.Protocol.OrgVersion orgVersion = null;




        /// <summary>
        /// UDP文件传输服务器
        /// </summary>
        private P2PFileServer p2pFileServer = null;
        /// <summary>
        /// 图片文件传输服务器
        /// </summary>
        private ImageFileServer imageFileServer = null;

        #endregion

        #region TCP发送消息

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="e">Element</param>
        public void BroadcastingMessageToOnlineUser(Element e)
        {
            //tcpMessageServer.BroadcastingMessage(e);

            msgServer.BroadcastingMessage(e);
        }

        /// <summary>
        /// 向在线用户广播消息
        /// </summary>
        /// <param name="XMLMsg"></param>
        public void BroadcastingMessageToOnlineUser(string XMLMsg)
        {
            //tcpMessageServer.BroadcastingMessage(XMLMsg);

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
                //tcpMessageServer.SendMessageToSession(user.Tag as TCPServerSession, XMLMsg);
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
                //tcpMessageServer.SendMessageToSession(user.Tag as TCPServerSession, Factory.CreateXMLMsg(e));
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

        #region TCP消息服务

        #region TCP创建连接
        private void tcpMessageServer_SessionCreated(object sender, TCP_ServerSessionEventArgs<TCPServerSession> e)
        {
            e.Session.PacketReceived += new TCP_ServerSession.PacketReceivedEventHandler(messageSession_PacketReceived);
            e.Session.Disonnected += new EventHandler(Session_Disonnected);
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
                    onLogin(obj as Auth, Session);
                else if (Session.IsAuthenticated)//如果提供其他服务客户端必须是已经进行验证后
                {
                    UserVcard requestUser = Session.Tag as UserVcard;//获得请求用户


                    if (obj is Message)//请求转发消息
                        onMessage(obj as Message, Session);
                    else if (obj is Presence)//请求在线状态
                    {
                        if ((obj as Presence).type == type.set)//设置在线状态
                            onPresence(obj as Presence, Session);
                        else if ((obj as Presence).type == type.get)//获得联系人在线状态
                            onGetUsersPresence(obj as Presence, Session);
                    }
                    else if (obj is Groups)//请求下载的分组信息
                        onOrgGroups(obj as Groups, Session);
                    else if (obj is Users)//请求下载用户信息
                        onOrgUsers(obj as Users, Session);
                    else if (obj is Rooms)//请求下载群信息
                        onOrgRooms(obj as Rooms, Session);
                    else if (obj is ChangeRoom)//更新群信息
                        onChangeRoom(obj as ChangeRoom, Session);
                    else if (obj is TCPImageFile)//通知客户端到文件服务器下载已上传完成的文件
                        onTCPImageFile(obj as TCPImageFile, e.Data, Session);
                    else if (obj is PFile)//转发文件传输消息
                        onPFile(obj as PFile, e.Data, Session);


                    #region 组织架构信息管理
                    if (obj is UserVcard)//如果是管理员变更用户信息资料
                        onUserVcard(obj as UserVcard, e.Data, Session);
                    else if (obj is GroupVcard)
                        onGroupVcard(obj as GroupVcard, e.Data, Session);
                    else if (obj is ChangePassword)
                        onChangePassword(obj as ChangePassword, e.Data, requestUser);
                    #endregion

                }
            }
            else //收到非法消息
                OnBadCommand(Session);
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
                    Presence presence = new Presence();
                    presence.from = user.UserID;
                    presence.ShowType = IMLibrary3.Enmu.ShowType.Offline;
                    BroadcastingMessageToOnlineUser(presence);//告诉在线者用户已经离线
                }
            }
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



        #region 私有方法事件

        #region 用户请求更新密码
        /// <summary>
        /// 用户请求更新密码
        /// </summary>
        /// <param name="changePWD">变量密码协议</param>
        /// <param name="requestUser">请求用户</param>
        private void onChangePassword(ChangePassword changePWD, string XMLMsg, UserVcard requestUser)
        {
            if (changePWD.NewPassword.Trim() == "") return;

            UserVcard user = getUser(changePWD.from);
            if (user == null) return;

            ///将密码Hash为MD5 
            changePWD.OldPassword = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes(changePWD.OldPassword.Trim()));
            changePWD.NewPassword = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes(changePWD.NewPassword.Trim()));

            if (requestUser.isAdmin || changePWD.OldPassword == user.Password)//如果用户是管理员
            {
                if (DBHelper.UpdatePassword(user.UserID, changePWD.NewPassword) > 0)//更新密码
                {
                    user.Password = changePWD.NewPassword;
                    SendMessageToUser(requestUser, XMLMsg);//通知用户更新密码成功  
                }
            }
        }
        #endregion

        #region 管理员变更分组信息
        /// <summary>
        /// 管理员变更分组信息
        /// </summary>
        /// <param name="card"></param>
        /// <param name="xmlMsg"></param>
        /// <param name="session"></param>
        private void onGroupVcard(GroupVcard card, string xmlMsg, TCPServerSession session)
        {
            if (card.GroupID.Trim() == "" || card.GroupName.Trim() == "") return;
            card.GroupID = card.GroupID.Trim();

            UserVcard requestUser = session.Tag as UserVcard;//请求用户

            if (card.type == type.New && requestUser.isAdmin)//如果用户是管理员
            {
                if (DBHelper.CreateGroupVcard(card) > 0)//保存于数据库
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    BroadcastingMessageToOnlineUser(xmlMsg);//将创建的分组信息广播给所有在线用户  
                }
            }
            else if (card.type == type.set && requestUser.isAdmin)//如果是更新分组信息
            {
                if (DBHelper.UpdateGroupVcard(card) > 0) //更新数据库
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    SendMessageToUser(requestUser, xmlMsg);//通知管理员创建新用户成功 
                }
            }
            else if (card.type == type.get)//如果是更新 
            {
                card.type = type.result;
                SendMessageToUser(requestUser, Factory.CreateXMLMsg(card));//将分组信息资料发送给请求者
            }
            else if (card.type == type.delete && requestUser.isAdmin)//如果是删除分组
            {
                if (DBHelper.DelGroup(card.GroupID) > 0) //数据库中删除
                {
                    orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息
                    BroadcastingMessageToOnlineUser(xmlMsg);//将删除的用户广播给所有在线用户 
                }
            }
        }
        #endregion

        #region 管理员添加变更用户信息
        /// <summary>
        /// 管理员添加变更用户信息
        /// </summary>
        /// <param name="card"></param>
        /// <param name="xmlMsg"></param>
        /// <param name="session"></param>
        private void onUserVcard(UserVcard card, string xmlMsg, TCPServerSession session)
        {
            if (card.UserID.Trim() == "" || card.UserName.Trim() == "" || card.GroupID.Trim() == "") return;
            card.UserID = card.UserID.Trim();

            UserVcard user = getUser(card.UserID);
            UserVcard requestUser = session.Tag as UserVcard;//请求用户

            if (card.type == type.New && user == null && requestUser.isAdmin)//如果用户不存在，创建新用户
            {
                Users.Add(card.UserID, card);
                //设置新用户密码为123456;
                card.Password = IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes("123456"));

                DBHelper.CreateUserVcard(card);//保存于数据库
                orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息

                BroadcastingMessageToOnlineUser(xmlMsg);//将创建的新用户信息广播给所有在线用户  
            }
            else if (card.type == type.set && requestUser.isAdmin)//如果是更新用户
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

                SendMessageToUser(requestUser, xmlMsg);//通知管理员创建新用户成功 
            }
            else if (card.type == type.get)//如果是获取用户资料
            {
                if (user == null) return;//如果用户不存在则退出
                user.type = type.result;
                SendMessageToUser(requestUser, Factory.CreateXMLMsg(user));//将用户资料发送给请求者
            }
            else if (card.type == type.delete && requestUser.isAdmin)//如果是删除用户
            {
                if (user == null) return;//如果用户不存在则退出
                if (requestUser.UserID == user.UserID) return;//如果管理员想删除自己，退出
                TCPServerSession se = user.Tag as TCPServerSession;
                if (se != null && se.IsConnected) { se.Disconnect(); se.Dispose(); }//如果删除的用户在线，则将其踢出
                Users.Remove(card.UserID);

                DBHelper.DelUser(card.UserID);//数据库中删除
                orgVersion = DBHelper.ResetOrgVersion(); //重设组织架构版本相关信息

                BroadcastingMessageToOnlineUser(xmlMsg);//将删除的用户广播给所有在线用户 
            }

        }
        #endregion

        #region 转发文件传输消息
        /// <summary>
        /// 转发文件传输消息
        /// </summary>
        /// <param name="FileMsg"></param>
        /// <param name="session"></param>
        private void onPFile(PFile msg, string XMLMsg, TCPServerSession session)
        {
            SendMessageToUser(msg.to, XMLMsg);
        }
        #endregion

        #region 通知客户端到文件服务器下载已上传完成的文件
        /// <summary>
        /// 通知客户端到文件服务器下载已上传完成的文件
        /// </summary>
        /// <param name="FileMsg"></param>
        /// <param name="session"></param>
        private void onTCPImageFile(TCPImageFile msg, string XMLMsg, TCPServerSession session)
        {
            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果消息发送给用户
            {
                SendMessageToUser(msg.to, XMLMsg);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)
            {
                SendMessageToRoom(msg.from, msg.to, XMLMsg);
            }
        }
        #endregion

        #region 处理用户请求创建或更新群
        /// <summary>
        /// 处理用户请求创建或更新群
        /// </summary>
        /// <param name="changeRoom"></param>
        /// <param name="session"></param>
        private void onChangeRoom(ChangeRoom changeRoom, TCPServerSession session)
        {
            UserVcard requestUser = session.Tag as UserVcard;//获得请求用户

            if (changeRoom.type == type.New && requestUser.CreatedRoomsCount < requestUser.CreateRooms)//如果请求创建群
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
                        room.CreateUserID = changeRoom.from;//创建者为请求者

                        requestUser.CreatedRoomsCount++;//标记用户创建群数

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
                if (room != null && room.CreateUserID == changeRoom.from)//如果群创建者为请求用户
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
                    SendMessageToRoom(room.CreateUserID, room, IMLibrary3.Protocol.Factory.CreateXMLMsg(changeRoom));//通知群内原用户，群信息已经修改

                    DBHelper.UpdateRoom(room);
                }
                #endregion
            }
        }
        #endregion

        #region 处理用户请求下载企业组织架构信息
        /// <summary>
        /// 用户请求下载群信息
        /// </summary>
        /// <param name="orgRooms"></param>
        /// <param name="session"></param>
        private void onOrgRooms(Rooms orgRooms, TCPServerSession session)
        {
            UserVcard user = getUser(orgRooms.from);
            if (user == null) return;//如果未找到用户，退出

            orgRooms.to = orgRooms.from; orgRooms.from = "";

            int i = 0;
            foreach (Room room in GetRooms(user))
            {
                orgRooms.Data.Add(room);
                i++;
                if (i == 5)//每次发送5个群信息
                {
                    SendMessageToUser(user, orgRooms);//发送群信息
                    orgRooms.Data = new List<object>();//用户信息清零
                    i = 0;
                }
            }
            if (orgRooms.Data.Count > 0)//发送剩下的群信息
            {
                SendMessageToUser(user, orgRooms);//发送群信息
            }
        }

        /// <summary>
        /// 处理用户获取企业组织架构用户信息请求
        /// </summary>
        /// <param name="orgUsers"></param>
        /// <param name="session"></param>
        private void onOrgUsers(Users orgUsers, TCPServerSession session)
        {
            #region 新算法
            foreach (string str in orgVersion.UsersXML.ToArray())
            {
                SendMessageToUser(orgUsers.from, str);//发送用户信息
            }
            #endregion
        }

        /// <summary>
        /// 处理用户获取企业组织架构分组信息请求
        /// </summary>
        /// <param name="orgGroups"></param>
        /// <param name="session"></param>
        private void onOrgGroups(Groups orgGroups, TCPServerSession session)
        {
            #region 新算法
            foreach (string str in orgVersion.GroupsXML.ToArray())
            {
                SendMessageToUser(orgGroups.from, str);//发送用户信息
            }
            #endregion
        }
        #endregion

        #region 处理用户请求转发聊天消息
        /// <summary>
        /// 处理用户请求转发聊天消息 
        /// </summary>
        /// <param name="presence"></param>
        /// <param name="session"></param>
        private void onMessage(Message msg, TCPServerSession session)
        {
            msg.DateTime = DateTime.Now.ToString();//将消息发送时间设置为服务器的时间 

            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果消息发送给用户
            {
                SendMessageToUser(msg.to, msg);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)
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

        #region 处理用户请求获得所有在线联系人的在线状态
        /// <summary>
        /// 处理用户请求获得所有在线联系人的在线状态
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="session"></param>
        private void onGetUsersPresence(Presence pre, TCPServerSession session)
        {
            ///将在线用户的Presence发送给登录用户
            Element e = new Element();
            foreach (TCPServerSession se in tcpMessageServer.Sessions.ToArray())
                if (se.IsAuthenticated)//如果是已认证的TCP客户端
                {
                    UserVcard userTemp = se.Tag as UserVcard;//获得TCP客户端绑定的用户
                    Presence pretemp = new Presence();
                    pretemp.from = userTemp.UserID;
                    pretemp.ShowType = userTemp.ShowType;
                    pretemp.Status = pretemp.Status;

                    e.Data.Add(pretemp);
                }
            tcpMessageServer.SendMessageToSession(session, e);

            ///广播登录用户上线消息
            UserVcard user = session.Tag as UserVcard;
            pre = new Presence();
            pre.from = user.UserID;
            pre.ShowType = user.ShowType;
            BroadcastingMessageToOnlineUser(pre);

            //发送离线消息
            List<string> offlineMsg = DBHelper.GetOfflineMessage(user.UserID, 50);//获取最后50条离线消息
            foreach (string msgXML in offlineMsg)
                SendMessageToUser(user, msgXML);
        }
        #endregion

        #region 处理用户请求更改在线状态
        /// <summary>
        /// 处理用户请求更改在线状态
        /// </summary>
        /// <param name="presence"></param>
        /// <param name="session"></param>
        private void onPresence(Presence presence, TCPServerSession session)
        {

            if (presence.ShowType == IMLibrary3.Enmu.ShowType.Invisible)//如果用户隐身，则将用户状态设置为离线发送给其他用户
                presence.ShowType = IMLibrary3.Enmu.ShowType.Offline;

            BroadcastingMessageToOnlineUser(presence);
        }
        #endregion

        #region 处理用户登录请求
        /// <summary>
        /// 处理用户登录请求
        /// </summary>
        /// <param name="auth"></param>
        private void onLogin(Auth auth, TCPServerSession session)
        {
            UserVcard user = getUser(auth.UserID);
            if (user != null && !user.Disable && Hasher.GetMD5Hash(TextEncoder.textToBytes(auth.Password)) == user.Password) //如果用户存在未禁用且密码正确
            {
                #region 重复登录或异地登录
                if (user.Tag != null && user.Tag is TCPServerSession)
                {
                    ///通知用户此帐号异地登录
                    auth.type = type.Else;
                    auth.Password = session.ID;
                    TCPServerSession se = user.Tag as TCPServerSession;
                    if (se != null && !se.IsDisposed && se.IsConnected)
                    {
                        tcpMessageServer.SendMessageToSession(se, auth);
                        se.Disconnect();//断开连接，释放资源
                    }
                }
                #endregion

                session.Tag = user;
                session.IsAuthenticated = true;

                user.Tag = session;
                user.ShowType = auth.ShowType;
                user.Status = auth.Status;

                ///发送登录成功消息
                auth.UserName = user.UserName;//告之登录用户的姓名
                auth.type = type.result;//告之登录用户成功登录
                auth.Password = session.ID;//告之登录用户在服务器的SESSION
                auth.LastDateTime = user.LastDateTime;//告之登录用户上次登录时间
                auth.LastIP = user.LastIP;//告之登录用户上次登录IP
                auth.FileServerTCPPort = DBHelper.settings.TcpFilePort;//告之登录用户TCP文件传输服务端口
                auth.FileServerUDPPort = DBHelper.settings.UdpFilePort;//告之登录用户UDP文件传输服务端口
                auth.isAdmin = user.isAdmin;//告之登录用户是否管理员
                auth.isBroadcast = user.isBroadcast;//告之登录用户是否可以发送广播消息
                auth.isSendNotice = user.isSendNotice;//告之登录用户是否可以发送通知消息
                auth.isSendSMS = user.isSendSMS;//告之登录用户是否可以发送手机短信
                auth.isEditUserData = user.isEditUserData;//告之登录用户是否可以编辑用户资料

                user.LastDateTime = DateTime.Now.ToString();
                user.LastIP = session.RemoteEndPoint.Address.ToString();

                //通知用户登录成功
                tcpMessageServer.SendMessageToSession(session, auth);

                //发送企业组织机构版本信息给用户
                orgVersion.RoomsCount = user.Rooms.Count;//用户加入群的数量
                tcpMessageServer.SendMessageToSession(session, orgVersion);

            }
            else//如果用户不存在或密码错误
            {
                ///发送密码错误消息
                auth.type = type.error;
                auth.Password = session.ID;
                tcpMessageServer.SendMessageToSession(session, auth);
            }
        }
        #endregion

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
