using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

#region IMLibrary3
using IMLibrary3;
using IMLibrary3.Enmu;
using IMLibrary3.Security;
using IMLibrary3.Organization;
using IMLibrary3.Operation;
using IMLibrary3.Protocol;

using IMLibrary3.Net;
using IMLibrary3.Net.TCP;
#endregion

namespace OurMsg.Controls
{
    public partial class ControlPanel : UserControl
    {
        #region 组件初始化
        public ControlPanel()
        {
            InitializeComponent();
            this.treeView_Organization.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView_Organization_NodeMouseDoubleClick);
            this.treeView_Organization.AfterCheck += new TreeViewEventHandler(treeView_Organization_AfterCheck);
            this.treeView_Organization.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView_Organization_NodeMouseClick);
            this.treeView_Rooms.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView_Rooms_NodeMouseDoubleClick);
            
        }
        #endregion
 
        #region 事件
        /// <summary>
        /// 事件代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ControlPanelEventHandler(object sender);


        public delegate void LoginEventHandler(object sender,Auth auth);

        /// <summary>
        /// 登录密码错误事件
        /// </summary>
        public event ControlPanelEventHandler UserLoginPasswordErrored;

        /// <summary>
        /// 登录成功
        /// </summary>
        public event LoginEventHandler UserLoginSuccessful;

        /// <summary>
        /// 登录超进
        /// </summary>
        public event ControlPanelEventHandler UserLoginOutTime;

        /// <summary>
        /// 用户掉线
        /// </summary>
        public event ControlPanelEventHandler UserOffline;
        
        /// <summary>
        /// 用户在别处登录
        /// </summary>
        public event ControlPanelEventHandler UserElseLogin;

       #endregion

        #region 变量
        /// <summary>
        /// TCP客户端
        /// </summary>
        private TCPClient tcpClient = null;
         
        /// <summary>
        /// 登录认证
        /// </summary>
        private  Auth MyAuth = null;

        /// <summary>
        /// 当前正确登录的用户密码
        /// </summary>
        private string myPassword = "";

        /// <summary>
        /// 组织机构分组
        /// </summary>
        private List<exGroup> Groups = new List<exGroup>();

        /// <summary>
        /// 用户
        /// </summary>
        private List<exUser> Users = new List<exUser>();

        /// <summary>
        /// 群
        /// </summary>
        private List<exRoom> Rooms = new List<exRoom>();
         
        /// <summary>
        /// 登录超时记数器
        /// </summary>
        private int LoginTimeCount = 0;// 登录超时记数器

        /// <summary>
        /// 发送通知消息窗口
        /// </summary>
        private FormSendNotice frmNotice =null ;

        /// <summary>
        /// 创建组窗口
        /// </summary>
        private FormCreateRoom frmCreateGroup = null;

        /// <summary>
        /// 下载组织机构窗口
        /// </summary>
        private FormDownOrganization frmOrg = null;

        /// <summary>
        /// 用户资料窗口
        /// </summary>
        private Dictionary <string,FormUserVcard> frmUserVcards =  new Dictionary<string,FormUserVcard>();

        /// <summary>
        /// 分组资料窗口
        /// </summary>
        private Dictionary<string, FormGroupVcard>frmGroupVcards = new  Dictionary<string,FormGroupVcard>();


        /// <summary>
        /// 创建用户窗口
        /// </summary>
        private FormUserVcard frmUserVcard =null;

        /// <summary>
        /// 创建分组窗口
        /// </summary>
        private FormGroupVcard frmGroupVcard = null;
        #endregion

        #region 组织机构树操作事件处理
        private void treeView_Organization_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is exUser)
            {
                exUser user = e.Node.Tag as exUser;
                FormTalkUser fs = GetUserMsgForm(user);// 获得用户消息对话框
                fs.Show();
                fs.Activate();
            }
        }

        private void treeView_Organization_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!e.Node.IsExpanded && e.Node.Nodes.Count != 0 && !e.Node.TreeView.CheckBoxes)
            {
                e.Node.TreeView.CollapseAll();
                e.Node.Expand();
            }
        }

        private void treeView_Organization_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
                node.Checked = e.Node.Checked;
        }

        #region 群树操作事件处理
        private void treeView_Rooms_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is exRoom)
            {
                exRoom room = e.Node.Tag as exRoom;
                FormTalkRoom fs = GetRoomMsgForm(room);
                fs.Show();
                fs.Activate();
            }
        }

        #endregion

        #region 加载本地数据库中的组织机构过程

        /// <summary>
        /// 加载本地数据库中的组织机构
        /// </summary>
        private void treeViewAddOrg()
        {
            if (this.treeView_Organization.Nodes.Count > 0) return;

            TreeNode node = this.addGroupToTreeView(this.Groups);
            this.addUserToTreeView(this.Users, this.Groups);
            if (node == null) return;

            foreach (exUser user in this.Users)
                user.UserName = user.UserName;

            foreach (TreeNode nodeTemp in node.Nodes)
            {
                treeView_Organization.Nodes.Add(nodeTemp);
            }
        }
        #endregion

        #region 添加群到treeView
        /// <summary>
        /// 添加群到treeView
        /// </summary>
        private void treeViewAddRooms(List<exRoom> Rooms)
        {
            treeView_Rooms.ImageList = this.imageList1;
            foreach (exRoom room in Rooms)
            {
                TreeNode node = new TreeNode();
                node.Text = room.RoomName + "(" + room.RoomID + ")";
                node.ToolTipText = room.RoomName;
                node.Tag = room;
                node.ImageIndex = 17;
                node.SelectedImageIndex = 17;
                room.TreeNode  = node;
                treeView_Rooms.Nodes.Add(node);
            }
        }
        #endregion

        #region 在treeView中添加分组groups集合并返回 TreeNode
        /// <summary>
        /// 在treeView中添加分组groups集合并返回 TreeNode
        /// </summary>
        /// <param name="treeView1"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        private TreeNode addGroupToTreeView(List<exGroup> groups)
        {
            if (groups == null) return null;
            TreeNode nodeTemp = new TreeNode();

            ///添加根分组节点
            foreach (exGroup group in groups)
                if (findGroup(group.SuperiorID) == null)
                {
                    TreeNode node = new TreeNode();
                    node.Text = group.GroupName;
                    node.ToolTipText = group.GroupName;
                    node.ImageIndex = 14;
                    node.SelectedImageIndex = 15;
                    node.Tag = group;
                    group.TreeNode = node;
                    nodeTemp.Nodes.Add(node);
                    group.GroupName = group.GroupName;
                }

            bool t = false;
           exGroup parentGroup;
            int rCount = 0;
            int sqrCount = (groups.Count * groups.Count);//最大循环次数

            while (!t && rCount <= sqrCount)//如果查询还未结束且循环次数没有超过部门数n平方，则继续
            {
                t = true;
                foreach (exGroup group in groups)
                {
                    parentGroup = findGroup(group.SuperiorID);//找到上级部门节点
                    if (parentGroup != null && group.TreeNode == null) //如果要添加的部门节点不是根部门节点且此节点还未添加 
                    {
                        if (parentGroup.TreeNode != null)// 当前的上级部门已经添加时，添加部门  
                        {
                            TreeNode node = new TreeNode();
                            node.Text = group.GroupName;
                            node.ToolTipText = group.GroupName;
                            node.ImageIndex = 14;
                            node.SelectedImageIndex = 15;
                            node.Tag = group;
                            group.TreeNode = node;
                            (parentGroup.TreeNode as TreeNode).Nodes.Add(node);
                            group.GroupName = group.GroupName;

                            group.SuperiorGroup = parentGroup;//设置上级组
                        }
                        else//如果当前部门节点的上级部门不是根部门节点并且上级部门的上级部门还未添加，则添加不成功，循环后再添加
                        {
                            t = false;
                        }
                    }
                    rCount++;//查询次数增1，如果大于部门n平方还未结束，则强行结束
                }
            }
            return nodeTemp;
        }
        #endregion

        #region 在treeView中的groups中添加用户
        /// <summary>
        /// 在treeView中的groups中添加用户
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="users"></param>
        public void addUserToTreeView(List<exUser> users, List<exGroup> groups)
        {
            if (users == null || groups == null) return;

            foreach (exUser user in users)
                addUserTreeNode(user);
        }

        /// <summary>
        /// 创建用户树节点
        /// </summary>
        /// <param name="user"></param>
        private void addUserTreeNode(exUser user)
        {
            exGroup group = findGroup(user.GroupID);
            if (group != null && group.TreeNode != null)
            {
                TreeNode node = new TreeNode();
                node.Text = user.UserName;
                node.ToolTipText = user.UserName;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                node.Tag = user;
                user.TreeNode = node;
                TreeNode groupNode = group.TreeNode as TreeNode;
                if (groupNode != null)
                    groupNode.Nodes.Add(node);

                user.Group = group;
                user.Group.UserCount++;
            }
        }
        #endregion

        #endregion

        #region TCP数据达到操作
        private void tcpClient_PacketReceived(object sender, TcpSessionEventArgs e)
        {
            PacketReceivedDelegate outdelegate = new PacketReceivedDelegate(PacketReceived);
            this.BeginInvoke(outdelegate, e.Data);
        }

        private delegate void PacketReceivedDelegate(string data);
        private void PacketReceived(string data)
        {
            Console.WriteLine(data);
            List<object> objs = Factory.CreateInstanceObjects(data);
            if (objs != null)
                foreach (object obj in objs)
                {
                    #region 组织机构管理
                    if (obj is UserVcard)//创建新用户
                    {
                        onUserVcard(obj as UserVcard);
                        return;
                    }
                    else if(obj is GroupVcard)
                    {
                        onGroupVcard(obj as GroupVcard);
                        return;
                    }
                    else if (obj is ChangePassword)
                    {
                        onChangePassword(obj as ChangePassword);
                        return;
                    }
                    #endregion 

                    if (obj is Auth)//用户登录
                        onLogin(obj as Auth);
                    else if (obj is IMLibrary3.Protocol.Message)//聊天消息
                        onMessage(obj as IMLibrary3.Protocol.Message);
                    else if (obj is Presence)//用户在线状态发生更改
                        onPresence(obj as Presence);
                    else if (obj is OrgVersion)//获得组织机构版本
                        onOrgVersion(obj as OrgVersion);
                    else if (obj is Group)//获得分组信息
                        onOrgGroups(obj as Group);
                    else if (obj is User)//获得用户信息
                        onOrgUsers(obj as User);
                    else if (obj is Room)//获得群信息
                        onOrgRooms(obj as Room);
                    else if (obj is ChangeRoom)//群信息更新
                        onChangeRoom(obj as ChangeRoom);
                    else if (obj is ImageFileMsg )//接收图片文件
                        onTCPImageFile(obj as ImageFileMsg);
                    if (obj is P2PFileMsg )//文件传输消息
                        onPFile(obj as P2PFileMsg);
                    if(obj is AVMsg)//视频对话消息
                        onAVMsg(obj as AVMsg);
                }
        }
        #endregion

        #region 服务器回应密码更改
        /// <summary>
        /// 服务器回应密码更改
        /// </summary>
        /// <param name="cPWD"></param>
        public void onChangePassword(ChangePassword cPWD)
        {
            if (cPWD.from == MyAuth.UserID)//如果是修改自己的密码
            {
                myPassword = cPWD.NewPassword; 
                IMLibrary3.Global.MsgShow("密码已经修改，请记住新密码！！");
            }
            else//如果是管理员修改别人的密码
            {
                exUser user = findUser(cPWD.from);
                if (user != null)
                    IMLibrary3.Global.MsgShow(user.UserName + "(" + user.UserID + ") 的密码修改成功！！");
            }
        }
        #endregion 

        #region 管理组织机构操作回应

        #region 创建、删除、更新分组信息操作
        private void onGroupVcard(GroupVcard card)
        {
            if (frmGroupVcard != null && !frmGroupVcard.IsDisposed && card.type == type.New)
                frmGroupVcard.isUpdateSuccess = true;

            exGroup  group = findGroup(card.GroupID);//查找本地用户是否存在操作用户

            if (card.type == type.New && group == null)
            {
                group = new exGroup();
                group.GroupID = card.GroupID;
                group.SuperiorID = card.SuperiorID;
                Groups.Add(group);
                //将用户节点添加到树
                {
                    TreeNode node = new TreeNode();
                    node.Text = group.GroupName;
                    node.ImageIndex = 14;
                    node.SelectedImageIndex = 15;
                    node.Tag = group;
                    group.TreeNode = node;
                    ///添加根分组节点
                    if (findGroup(group.SuperiorID) == null)
                        treeView_Organization.Nodes.Add(node);
                    else
                        (findGroup(group.SuperiorID).TreeNode as TreeNode).Nodes.Add(node);
                }
                group.GroupName = card.GroupName;

                OpeRecordDB.UpdateGroupVcard(card);//添加分组信息
            }
            else if (card.type == type.delete && group != null)
            {
                TreeNode node = group.TreeNode as TreeNode;
                if (node != null)
                    treeView_Organization.Nodes.Remove(node);
                OpeRecordDB.DeleteGroup(card.GroupID);//本地数据库中删除
            }
            else if (group != null)
            {
                FormGroupVcard frm = getGroupVcardForm(card.GroupID);
                if (frm != null && !frm.IsDisposed)
                    if (card.type != type.set)
                        frm.GroupVcard = card;
                    else
                        frm.isUpdateSuccess = true;
                group.GroupName = card.GroupName;

                OpeRecordDB.UpdateGroupVcard(card);//更新分组信息
            }
        }
        #endregion

        #region 创建、删除、更新用户信息操作
        private void onUserVcard(UserVcard  card)
        {
            if (frmUserVcard != null && !frmUserVcard.IsDisposed  && card.type == type.New)
                frmUserVcard.isUpdateSuccess = true;

            exUser user = findUser(card.UserID);//查找本地用户是否存在操作用户

            if (card.type == type.New && user == null)
            {
                user = new exUser();
                user.UserID = card.UserID;
                user.GroupID = card.GroupID;
                Users.Add(user);
                addUserTreeNode(user);//将用户节点添加到树
                user.UserName = card.UserName;
                OpeRecordDB.UpdateUserVcard(card);//添加到本地数据库
            }
            else if (card.type == type.delete && user != null)
            {
                TreeNode node = user.TreeNode as TreeNode;
                if (node != null && node.Parent != null)
                {
                    if (user.ShowType != IMLibrary3.Enmu.ShowType.Offline)
                        user.Group.OnLineUserCount -= 1;

                    user.Group.UserCount -= 1;

                    node.Parent.Nodes.Remove(node);//删除用户树节点
                    Form frm = user.Tag as Form;
                    if (frm != null && !frm.IsDisposed) { frm.Close(); frm.Dispose(); }//关闭用户对话窗口
                    Users.Remove(user);//删除用户
                    OpeRecordDB.DeleteUser(card.UserID);//本地数据库中删除
                }
            }
            else if (user != null)
            {
                FormUserVcard frm = getUserVcardForm(card.UserID);
                if (frm != null && !frm.IsDisposed)
                    if (card.type != type.set)
                        frm.UserVcard = card;
                    else
                        frm.isUpdateSuccess = true;
                user.UserName = card.UserName;
                OpeRecordDB.UpdateUserVcard(card);//更新本地数据库
            }
        }
        #endregion

        #endregion

        #region 联系人文件传输请求
        private void onAVMsg(AVMsg avMsg)
        {
            exUser user = findUser(avMsg.from);
            if (user != null)
            {
                FormTalkUser fs = user.Tag as FormTalkUser;

                if (avMsg.type == type.New)//如果是邀请视频
                {
                    fs = GetUserMsgForm(user);
                    fs.ReadyAV(false);
                    fs.Show();
                    fs.Activate();
                }
                else if (avMsg.type == type.cancel)//如果是取消视频
                {
                    if (fs != null || !fs.IsDisposed)
                        fs.CancelAV(false);//对方取消
                }
                else if (avMsg.type == type.set)//设置视频参数
                {
                    if (fs != null || !fs.IsDisposed)
                    {
                        fs.setAVRometEP(avMsg);
                    }
                }
                else if (avMsg.type == type.Else)//设置视频图像编码信息 
                {
                    if (fs != null || !fs.IsDisposed)
                    {
                        fs.SetVideoBitmapinfo(avMsg);
                    }
                }
            }
        }
        #endregion

        #region 联系人文件传输请求
        private void onPFile(P2PFileMsg  pfile)
        {
            exUser user = findUser(pfile.from );
            if (user != null)
            {
                FormTalkUser fs = user.Tag as FormTalkUser;

                if (pfile.type == type.New)//如果是接收新文件
                {
                    fs = GetUserMsgForm(user);
                    fs.ReceiveFile(pfile);
                    fs.Show();
                    fs.Activate();
                }
                else  if (pfile.type == type.cancel)//如果是取消文件传输
                {
                    if (fs != null || !fs.IsDisposed)
                    {
                        fs.CancelFile(pfile);
                    }
                }
                else if (pfile.type == type.set)
                {
                    if (fs != null || !fs.IsDisposed)
                    {
                        fs.setFileRometEP(pfile);
                    }
                }
            }
        }
        #endregion

        #region 用户登录

        #region 下载企业组织架构信息
        /// <summary>
        /// 获得群信息
        /// </summary>
        /// <param name="room"></param>
        private void onOrgRooms(Room room)
        {
            exRoom exroom = new exRoom();
            exroom.RoomID = room.RoomID;
            exroom.RoomName = room.RoomName;
            exroom.Notice = room.Notice;
            exroom.UserIDs = room.UserIDs;
            exroom.CreateUserID = room.CreateUserID;
            Rooms.Add(exroom);

            if (frmOrg != null && !frmOrg.IsDisposed)
            {
                frmOrg.Times = 0;
                frmOrg.Value = this.Rooms.Count;
            }
        }

        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="user"></param>
        private void onOrgUsers(User user)
        {
            exUser exuser = new exUser();
            exuser.UserID = user.UserID;
            exuser.UserName = user.UserName;
            exuser.GroupID = user.GroupID;
            exuser.OrderID = user.OrderID;
            Users.Add(exuser);

            if (frmOrg != null && !frmOrg.IsDisposed)
            {
                frmOrg.Times = 0;
                frmOrg.Value = this.Users.Count;
            }
        }

        /// <summary>
        /// 获得分组信息
        /// </summary>
        /// <param name="group"></param>
        private void onOrgGroups(Group group)
        {

            exGroup exgroup = new exGroup();
            exgroup.GroupID = group.GroupID;
            exgroup.GroupName = group.GroupName;
            exgroup.SuperiorID = group.SuperiorID;
            exgroup.OrderID = group.OrderID;
            Groups.Add(exgroup);

            if (frmOrg != null && !frmOrg.IsDisposed)
            {
                frmOrg.Times = 0;
                frmOrg.Value = this.Groups.Count;
            }
        }

        /// <summary>
        /// 获得组织机构版本信息
        /// </summary>
        /// <param name="org"></param>
        private void onOrgVersion(OrgVersion org)
        {
            try
            {
                #region 判断登录的用户本地数据库文件夹是否存在
                System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(MyAuth.UserID);
                if (!dInfo.Exists)
                    dInfo.Create();
                string FileNamePath =Application.StartupPath +"\\"+ MyAuth.UserID + "\\Record.mdb";
                if (!System.IO.File.Exists(FileNamePath))
                    System.IO.File.Copy(Application.StartupPath + "\\Record.db", FileNamePath);

                ////初始化本地数据库连接字符串
                IMLibrary3.Data.SQLiteDBHelper.connectionString = "data source=" + FileNamePath;
                #endregion

                OrgVersion localOrg = OpeRecordDB.GetOrgVersion(MyAuth.UserID);

                #region 如果版本已经改变下载组织机构组信息
                if (org.GroupsVersion != localOrg.GroupsVersion)//true
                {
                    Groups.Clear();
                    treeView_Organization.Nodes.Clear();

                    OpeRecordDB.DeleteAllGroup();//删除本地数据库中分组信息

                    frmOrg = new FormDownOrganization();
                    frmOrg.Shown += delegate(object sender, EventArgs e)
                    {
                        DownloadGroups dGroups = new DownloadGroups();
                        dGroups.from = MyAuth.UserID;
                        dGroups.type = type.get;
                        SendMessageToServer(dGroups);//请求下载分组信息
                    };
                    frmOrg.MaxValue = org.GroupsCount;
                    frmOrg.ShowText = "正在下载分组信息...";
                    frmOrg.ShowDialog();

                    OpeRecordDB.AddGroups(this.Groups);//将下载的分组信息保存于数据库中
                }
                #endregion

                #region 下载组织机构用户信息
                if (org.UsersVersion != localOrg.UsersVersion)//(true)
                {
                    Users.Clear();
                    treeView_Organization.Nodes.Clear();
                    OpeRecordDB.DeleteAllUser();//删除本地数据库中用户信息

                    frmOrg = new FormDownOrganization();
                    frmOrg.Shown += delegate(object sender, EventArgs e)
                    {
                        DownloadUsers  dUsers = new  DownloadUsers();
                        dUsers.from = MyAuth.UserID;
                        dUsers.type = type.get;
                        SendMessageToServer(dUsers);//请求下载联系人信息
                    };
                    frmOrg.MaxValue = org.UsersCount;
                    frmOrg.ShowText = "正在下载用户信息...";
                    frmOrg.ShowDialog();

                    OpeRecordDB.AddUsers(this.Users);
                }
                #endregion

                #region 下载群信息
                if (org.RoomsCount !=0)//如果群数大于0
                {
                    Rooms.Clear();
                    treeView_Rooms.Nodes.Clear();

                    //OpeRecordDB.DeleteAllRoom();//删除本地数据库中群信息

                    frmOrg = new FormDownOrganization();
                    frmOrg.Shown += delegate(object sender, EventArgs e)
                    {
                        DownloadRooms dRooms = new DownloadRooms();
                        dRooms.from = MyAuth.UserID;
                        dRooms.type = type.get;
                        SendMessageToServer(dRooms);//请求下载群信息
                    };

                    frmOrg.MaxValue = org.RoomsCount;
                    frmOrg.ShowText = "正在下载群资料...";
                    frmOrg.ShowDialog();

                    //OpeRecordDB.AddRooms(this.Rooms);
                }
                #endregion

                //如果组织机构已经更改或组织机构未加载到树图
                if (this.treeView_Organization.Nodes.Count == 0)
                {
                    //则更新到本地数据库
                    OpeRecordDB.UpdateOrgVersion(org);
                    this.Groups = OpeRecordDB.GetGroups();
                    this.Users = OpeRecordDB.GetUsers();
                    treeViewAddOrg();//加载树
                }

                if (this.treeView_Rooms.Nodes.Count == 0)
                {
                    //this.Rooms = OpeRecordDB.GetRooms();
                    foreach (Room room in this.Rooms)
                    {
                        foreach (string userID in room.UserIDs.Split(';'))
                        {
                            User user = findUser(userID);
                            if (user != null)
                                room.Users.Add(user.UserID, user);
                        }
                    }

                    treeViewAddRooms(this.Rooms);
                }

                //请求联系人在线状态信息
                Presence pre = new Presence();
                pre.from = MyAuth.UserID;
                pre.type = type.get;//必须设置 set,以表示是设置，如果为get，则是获取所有联系人的状态
                SendMessageToServer(pre);
            }
            catch (Exception ex)
            {
                IMLibrary3.Global.MsgShow(ex.Source + ex.Message);
            }
        }
  
        #endregion

        #region 开始登录
        /// <summary>
        /// 开始登录 
        /// </summary>
        /// <param name="auth">登录用户参数</param>
        /// <param name="IsOutTime">是否超时时钟重复的登录</param>
        public void Login(IMLibrary3.Protocol.Auth auth, bool IsOutTime)
        {
            if (myPassword!="")//如果已经登录成功过
                auth.Password = myPassword;//将正确的密码设置为登录密码

            MyAuth = auth;//暂存自己的登录信息于内存

            if (tcpClient == null || tcpClient.IsDisposed)
            {
                tcpClient = new TCPClient();
                tcpClient.PacketReceived += new TCP_Client.PacketReceivedEventHandler(tcpClient_PacketReceived);
                tcpClient.Disonnected += new EventHandler(tcpClient_Disonnected);
            }
            if (!tcpClient.IsConnected)
                tcpClient.Connect(Global.ServerDomain,Global.ServerMsgPort);

            if (!this.timerLogin1.Enabled)
                this.timerLogin1.Enabled = true;

            SendMessageToServer(auth);//向服务器登录
        }

        #endregion

        #region 登录心跳时钟
        private void timerLogin1_Tick(object sender, EventArgs e)
        {
            LoginTimeCount++;

            ///如果30秒后未登录成功，则触发登录超时事件
            if (LoginTimeCount > 30 && !tcpClient.IsConnected)
            {
                this.timerLogin1.Enabled = false;

                LoginTimeCount = 0;

                if (UserLoginOutTime != null)
                    UserLoginOutTime(this);
            }

            ///每个8秒重新发送一次登录请求
            if (LoginTimeCount > 1 && LoginTimeCount % 10 == 0 && !tcpClient.IsConnected)
                Login(MyAuth, true);//登录。。。              


            //如果用户已经上线，则停止检测检测
            if (tcpClient.IsConnected)
            {
                this.timerLogin1.Enabled = false;
                LoginTimeCount = 0;
            }
        }
        #endregion

        #region 登录回应处理
        private void onLogin(Auth auth)
        {

            if (auth.type == type.error)//密码错误 
                LoginPasswordError();
            else if (auth.type == type.result)//登录成功
            {
                if (myPassword == "" && MyAuth.IsSavePassword)//如果是第一次成功登录并且需要保存登录密码 
                {
                    IMLibrary3.OpeRecordDB.SaveAuth(MyAuth);//保存成功登录用户的信息
                }
                else if (myPassword == "" && !MyAuth.IsSavePassword)
                {
                    myPassword = MyAuth.Password;//暂存登录成功后的密码
                    MyAuth.Password = "";
                    IMLibrary3.OpeRecordDB.SaveAuth(MyAuth);//保存成功登录用户的信息
                }

                if (myPassword == "")
                    myPassword = MyAuth.Password;//暂存登录成功后的密码

                MyAuth = auth;//暂存登录成功后的服务器返回的登录信息

                ///设置UDP文件传输服务器端口
                Global.FileTransmitServerEP = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Global.ServerDomain), MyAuth.FileServerUDPPort);
                ///设置UDP音视频传输服务器端口
                Global.AVTransmitServerEP = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Global.ServerDomain), MyAuth.AVServerUDPPort);
                ///设置TCP图片文件传输服务器端口
                Global.ImageServerEP=new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Global.ServerDomain),MyAuth.FileServerTCPPort);


                this.LoginTimeCount = 0;//超时器清零
                this.timerLogin1.Enabled = false;//停止登录超时检测
                if (UserLoginSuccessful != null) //触发登录成功事件
                    UserLoginSuccessful(this, auth);
            }
            else if (auth.type == type.Else)//别处登录
                ElseLogin();
        }
        #endregion

        #region 用户别处登录
        /// <summary>
        /// 用户别处登录
        /// </summary>
        private void ElseLogin()
        { 
            SetAllUsersOffline();//将所有用户设置为离线

            MyAuth.ShowType = IMLibrary3.Enmu.ShowType.Offline;

            if (UserElseLogin != null)
                UserElseLogin(this);
        }
        #endregion

        #region 服务器返回用户密码错误
        /// <summary>
        /// 用户登录密码错误
        /// </summary>
        private void LoginPasswordError()
        {
            this.LoginTimeCount = 0;//超时器清零
            this.timerLogin1.Enabled = false;//停止登录超时检测

            if (UserLoginPasswordErrored != null)
                UserLoginPasswordErrored(this);
        }
        #endregion

        #endregion

        #region 用户离线
        private void tcpClient_Disonnected(object sender, EventArgs e)
        {
            DisonnectedDelegate outdelegate = new DisonnectedDelegate(Disonnected);
            this.BeginInvoke(outdelegate);
        }
        private delegate void DisonnectedDelegate();
        private void Disonnected()
        {
            if (UserOffline != null)
                UserOffline(this);
            MyAuth.ShowType = IMLibrary3.Enmu.ShowType.Offline;
            SetAllUsersOffline();
        }

        #endregion

        #region 状态更改

        #region 自己更改状态
        /// <summary>
        /// 设置登录用户在线状态
        /// </summary>
        /// <param name="showType"></param>
        public void setMyPresence(IMLibrary3.Enmu.ShowType showType)
        {
            if (showType == MyAuth.ShowType)//如果状态未做改变，则无需发送到服务器
                return;

            if (showType == IMLibrary3.Enmu.ShowType.Offline
                && (tcpClient != null && !tcpClient.IsDisposed && tcpClient.IsConnected)) //如果要下线
            {
                this.tcpClient.Disconnect();
                this.tcpClient.Dispose();
                this.tcpClient = null;
                return;
            }

            if (showType != IMLibrary3.Enmu.ShowType.Offline 
                && (tcpClient == null || tcpClient.IsDisposed || !tcpClient.IsConnected))//如果已经下线，则上线
            { 
                MyAuth.ShowType = showType;
                Login(MyAuth, true);//登录。。。 
                return;
            }

            Presence pre = new Presence();
            pre.from = MyAuth.UserID;
            pre.ShowType = showType;
            pre.type = type.set;//必须设置 set,以表示是设置，如果为get，则是获取所有联系人的状态
            SendMessageToServer(pre );
        }
        #endregion

        #region 联系人更改状态
        /// <summary>
        /// 联系人更改状态
        /// </summary>
        /// <param name="pres"></param>
        public void onPresence(IMLibrary3.Protocol.Presence pres)
        {
            //查找联系人列表中是否存在此联系人，如果存在，则更新状态信息
            exUser user = findUser(pres.from );
            if (user != null)
            {
                user.Status = pres.Status;
                user.ShowType = pres.ShowType;

                if (user.UserID == MyAuth.UserID)
                {
                    MyAuth.ShowType = user.ShowType;
                    MyAuth.UserName = user.UserName;
                }
            }
        }
        #endregion

        #endregion

        #region 群信息更新
        private void onChangeRoom(ChangeRoom changeRoom)
        {
            #region 创建临时群
            exRoom room = new exRoom();
            room.RoomID = changeRoom.RoomID;
            room.RoomName = changeRoom.RoomName;
            room.Notice = changeRoom.Notice;
            room.CreateUserID = changeRoom.CreateUserID;
            room.UserIDs = changeRoom.UserIDs;
            if(room.UserIDs !=null)
            foreach (string userID in  room.UserIDs.Split(';'))
            {
                User user = findUser(userID);
                if (user != null)
                    room.Users.Add(user.UserID, user);
            }
            #endregion

            exRoom exroom = findRoom(changeRoom.RoomID);//查找群是否已经存在

            if (changeRoom.CreateUserID == MyAuth.UserID)//如果是自己创建的群
            {
                if (changeRoom.type == type.New)//如果创建群成功,更新群成功
                {
                    if (this.frmCreateGroup != null && !this.frmCreateGroup.IsDisposed)//如果创建群窗口已经加载
                    {
                        this.frmCreateGroup.Room = room;
                        this.frmCreateGroup.isUpdateSuccess = true;
                    }
                    if (exroom == null)//如果群不存在，则添加群
                    {
                        this.Rooms.Add(room);
                        List<exRoom> rooms = new List<exRoom>();
                        rooms.Add(room);
                        treeViewAddRooms(rooms);
                    }
                }
                else if (changeRoom.type == type.set)//如果是自己更新群信息
                {
                    if (exroom != null)//如果群存在，则更新群信息即可
                    {
                        exroom.Notice = room.Notice;
                        exroom.UserIDs = room.UserIDs;
                        exroom.RoomName = room.RoomName;
                        exroom.Users = room.Users;
                        FormTalkRoom frmGroup = exroom.Tag as FormTalkRoom;
                        if (frmGroup != null)
                            frmGroup.Room = exroom;

                        OurMsg.FormCreateRoom frm = (exroom.FormData as FormCreateRoom);
                        if (frm != null && !frm.IsDisposed)
                        {
                            frm.Room = room;
                            frm.isUpdateSuccess = true;
                        }
                    }
                }
            }
            else//如果群信息发生改变
            {
                if (changeRoom.type == type.New || changeRoom.type == type.set)//如果加入群 
                {
                    if (exroom == null)//如果群不存在，则添加群
                    {
                        this.Rooms.Add(room);
                        List<exRoom> rooms = new List<exRoom>();
                        rooms.Add(room);
                        treeViewAddRooms(rooms);
                    }
                    else//如果群存在，则更新信息
                    {
                        exroom.Notice = room.Notice;
                        exroom.UserIDs = room.UserIDs;
                        exroom.RoomName = room.RoomName;
                        exroom.Users = room.Users;

                        FormTalkRoom frmGroup = exroom.Tag as FormTalkRoom;
                        if (frmGroup != null)
                            frmGroup.Room = exroom;
                    }
                }
                else if (changeRoom.type == type.delete)//如果退出群
                {
                    if (exroom != null)
                    {
                        Form frm = exroom.Tag as Form;
                        if (frm != null && !frm.IsDisposed)
                            frm.Close();

                        frm = exroom.FormData as Form;
                        if (frm != null && !frm.IsDisposed)
                            frm.Close();
                         
                        TreeNode node = (exroom.TreeNode as TreeNode);
                        if (node != null) node.Remove();
                         
                        this.Rooms.Remove(exroom);
                    }
                }
            }
        }

        #region 创建或添加群成功后操作
        /// <summary>
        /// 创建或添加群成功后操作
        /// </summary>
        /// <param name="Room"></param>
        private void CreateOrAddGroup(exRoom Room)
        {
            exRoom group = findRoom(Room.RoomID);//查找群组 

            if (group == null)
            {
                group = Room;//获取群组信息

                this.Rooms.Add(group);//内存中添加群组

                TreeNode node = new TreeNode();
                node.Text = group.RoomID + "(" + group.RoomID + ")";
                node.Tag = group;
                group.TreeNode = node;
                treeView_Rooms.Nodes.Add(node);
            }
             OpeRecordDB.UpdateRooms(group);//数据库中保存群组资料
        }
        #endregion

        #endregion

        #region 收到对话消息

        #region 处理接收图片文件
        /// <summary>
        /// 处理接收图片文件 
        /// </summary>
        /// <param name="msg"></param>
        private void onTCPImageFile(ImageFileMsg msg)
        {
            exUser user = findUser(msg.from);
            if (user == null) return;
            if (user.UserID == MyAuth.UserID) return;//如果是自己发送给自己的消息，则不处理

            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(MyAuth.UserID + "\\ArrivalImage");
            if (!dInfo.Exists)
                dInfo.Create();

            string fileName = MyAuth.UserID + "\\ArrivalImage\\" + msg.MD5 + msg.Extension;

            if (System.IO.File.Exists(fileName)) return;//如果本地已经有此文件已经存在，则退出(不接收文件)

            TFileInfo fileinfo = new TFileInfo();
            fileinfo.MD5 = msg.MD5;
            fileinfo.Length = msg.Length;
            fileinfo.Extension = msg.Extension;
            fileinfo.fullName = fileName;

            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果图片发送给用户
            {
                ImageFileClient tcpfile = new ImageFileClient(Global.ImageServerEP, fileinfo);
                tcpfile.fileTransmitted += delegate(object sender, fileTransmitEvnetArgs e)
                {
                    FormTalkUser fs = user.Tag as FormTalkUser;
                    if (fs != null)
                    {
                        List<MyPicture> needRecPics = fs.GetNeedRecPicture();
                        foreach (MyPicture pic in needRecPics)
                            if (pic.MD5 == e.fileInfo.MD5)
                            {
                                needRecPics.Remove(pic);
                                pic.Image = System.Drawing.Image.FromFile(e.fileInfo.fullName);
                                fs.Invalidate();
                            }
                    }
                    (sender as ImageFileClient).Dispose();
                    sender = null;
                };
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)//如果图片发送给群
            {

                exRoom room = findRoom(msg.to);//获得消息接收群
                if (room == null) return;
                FormTalkRoom fs = room.Tag as FormTalkRoom;

                ImageFileClient tcpfile = new ImageFileClient(Global.ImageServerEP, fileinfo);
                tcpfile.fileTransmitted += delegate(object sender, fileTransmitEvnetArgs e)
                {
                    if (fs != null)
                    {
                        List<MyPicture> needRecPics = fs.GetNeedRecPicture();
                        foreach (MyPicture pic in needRecPics)
                            if (pic.MD5 == e.fileInfo.MD5)
                            {
                                needRecPics.Remove(pic);
                                pic.Image = System.Drawing.Image.FromFile(e.fileInfo.fullName);
                                fs.Invalidate();
                            }
                    }
                    (sender as ImageFileClient).Dispose();
                    sender = null;
                };
            } 
        }

       
        #endregion

        #region 处理用户发送的对话消息
        /// <summary>
        /// 处理用户发送的对话消息 
        /// </summary>
        /// <param name="msg"></param>
        private void onMessage(IMLibrary3.Protocol.Message msg)
        {
            exUser user = findUser(msg.from);
            if (user == null) return;
            if (user.UserID == MyAuth.UserID) return;//如果是自己发送给自己的消息，则不处理

            OpeRecordDB.AddMsg(msg);//将消息添加到数据库

            if (msg.MessageType == IMLibrary3.Enmu.MessageType.User)//如果消息发送给用户
            {
                    OnUserMessage(msg,user);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Group)//如果消息发送给群
            {
                OnGroupTalk(msg,user);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.Notice)//如果发送通知消息给多个用户
            {
                OnNoticeMsg(msg, user);
            }
            else if (msg.MessageType == IMLibrary3.Enmu.MessageType.broadcasting)//如果发送通知消息给所有用户
            {

            }
        }
        #endregion

        #region 收到通知消息
        /// <summary>
        /// 收到通知消息
        /// </summary>
        private void OnNoticeMsg(IMLibrary3.Protocol.Message msg,exUser user)
        {
            FormNotice fs = new FormNotice();
            fs.MsgToRichTextBox(msg, user);
            fs.Show();
          

        }
        #endregion

        #region 收到用户对话消息
        /// <summary>
        /// 收到用户对话消息
        /// </summary>
        private void OnUserMessage(IMLibrary3.Protocol.Message msg,exUser user)
        {
            FormTalkUser fs = GetUserMsgForm(user);
            fs.MsgToRichTextBox(msg, false);
            fs.Show();
            fs.Activate();
        }
        #endregion

        #region 收到群组对话消息
        /// <summary>
        /// 收到群组对话消息
        /// </summary>
        private void OnGroupTalk(IMLibrary3.Protocol.Message Msg, exUser user)
        { 
            exRoom room = findRoom(Msg.to);//获得消息接收群
            if (room == null) return;

            FormTalkRoom fs = GetRoomMsgForm(room);
            fs.MsgToRichTextBox(Msg, false, user.UserName + "(" + user.UserID + ")");
            fs.Show();
        }
        #endregion

        #endregion

        #region 公共方法
        /// <summary>
        /// 发送消息至服务器 
        /// </summary>
        /// <param name="e"></param>
        public void SendMessageToServer(Element  e)
        {
            e.from = MyAuth.UserID;//此处告之服务器，消息由自己发送
            if (tcpClient!=null && !tcpClient.IsDisposed && tcpClient.IsConnected)
                tcpClient.Write(e);
        }

        /// <summary>
        /// 发送消息至服务器
        /// </summary>
        /// <param name="e">消息对像</param>
        public  void SendMessageToServer(object  e)
        {
            if (tcpClient != null && !tcpClient.IsDisposed && tcpClient.IsConnected)
                tcpClient.Write(IMLibrary3.Protocol.Factory.CreateXMLMsg( e));
        } 
        
        #region 关闭服务
        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="IsCloseCon">是否关闭与服务器的连接(退出程序时用)</param>
        public void Close(bool IsCloseCon)
        {
             
        }
        #endregion

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="userVcard"></param>
        public void CreateUser(UserVcard card)
        {
            card.type = type.New;
            SendMessageToServer(card);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userVcard"></param>
        public void UpdateUser(UserVcard card)
        {
            card.type = type.set;
            SendMessageToServer(card);
        }


        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="userVcard"></param>
        public void CreateGroup(GroupVcard  card)
        {
            card.type = type.New;
            SendMessageToServer(card);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userVcard"></param>
        public void UpdateGroup(GroupVcard card)
        {
            card.type = type.set;
            SendMessageToServer(card);
        }

        /// <summary>
        /// 显示用户资料窗口
        /// </summary>
        /// <param name="userID"></param>
        public void ShowUserVcard(string   userID)
        {
            exUser user = findUser(userID);
            if (user != null)
                ShowUserVcard(user);
        }

        /// <summary>
        /// 显示用户资料窗口
        /// </summary>
        /// <param name="user"></param>
        public void ShowUserVcard(exUser user)
        {
            FormUserVcard frm = getUserVcardForm(user.UserID);
            if (frm == null)
            {
                frm = new FormUserVcard();
                frmUserVcards.Add(user.UserID, frm);

                if (MyAuth.isAdmin)//如果是管理员，则添加更新事件
                    frm.UpdateVcard += delegate(object sender1, UserVcard userVcard)//更新事件
                    {
                        UpdateUser(userVcard);//更新用户资料
                    };
                else//如果不是管理员，则添加刷新事件
                    frm.RefreshVcard += delegate(object sender1, UserVcard userVcard)//刷新事件
                    {
                        //向服务器请求获得用户最新资料
                        userVcard.type = type.get;
                        SendMessageToServer(userVcard);
                    };

                frm.FormClosed += delegate(object sender1, FormClosedEventArgs e1)//窗口关闭事件
                {
                    frm.Dispose();
                    frmUserVcards.Remove(user.UserID);
                };

                frm.ChanagePassword += delegate(object sender2, ChangePassword e2)//密码修改事件
                {
                    if (!frm.isAdmin && e2.OldPassword != myPassword)
                    {
                        IMLibrary3.Global.MsgShow("旧密码不正确，请重新输入");
                        return;
                    }
                    SendMessageToServer((object)e2);
                };

                UserVcard card = OpeRecordDB.GetUserVcard(user.UserID);
                if (card == null)
                {
                    card = new UserVcard();
                    card.UserID = user.UserID;
                    card.UserName = user.UserName;
                    card.GroupID = user.GroupID;
                    card.OrderID = user.OrderID;

                    //向服务器请求获得用户最新资料
                    card.type = type.get;
                    SendMessageToServer(card);
                }
                frm.UserVcard = card;
            }

            if (MyAuth.isAdmin)//如果是管理员
                frm.Text = "修改 " + user.UserName + "(" + user.UserID + ")的资料";
            else
                frm.Text = "查看 " + user.UserName + "(" + user.UserID + ")的资料";

            frm.myUserID = MyAuth.UserID;
            frm.isAdmin = MyAuth.isAdmin;
            frm.IsCreate = false;
            frm.Show();
            frm.Activate ();
        }
        #endregion

        #region 将所有用户设置为离线
        /// <summary>
        /// 将所有用户设置为离线
        /// </summary>
        private void SetAllUsersOffline()
        {
            foreach (exUser user in this.Users)
                user.ShowType = IMLibrary3.Enmu.ShowType.Offline;
        }

        #endregion
        
        #region 获得消息对话框

        #region 获得用户消息对话框
        /// <summary>
        /// 获得用户消息对话框
        /// </summary>
        /// <param name="user"></param>
        private FormTalkUser GetUserMsgForm(exUser user)
        {
            FormTalkUser fs = null;
            try
            {
                fs = user.Tag as FormTalkUser;
                if (fs == null || fs.IsDisposed)
                {
                    fs = new FormTalkUser(user );//发送消息对话框根据需要替换
                    user.Tag = fs;
                    fs.myUserID = MyAuth.UserID;
                    fs.myUserName = MyAuth.UserName;

                    fs.Text = "与 " + user.UserName + "(" + user.UserID + ") 对话";

                    ///消息发送事件代理
                    fs.SendMsgToUser += delegate(IMLibrary3.Protocol.Element e, IMLibrary3.Organization.User User)
                    {
                        e.from = MyAuth.UserID;
                        e.to = User.UserID; 
                        SendMessageToServer(e);

                        if (e is IMLibrary3.Protocol.Message)
                            OpeRecordDB.AddMsg(e as IMLibrary3.Protocol.Message);//将消息添加到数据库
                    };
                    fs.FormClosed += delegate(object sender, FormClosedEventArgs e)
                                    { fs.Dispose(); fs = null; user.Tag = null; };
                
                }
            }
            catch (Exception ex)
            {
                IMLibrary3.Global.MsgShow(ex.Message);
            }
            return fs;
        }
       
        #endregion

        #region 获得群对话框
        /// <summary>
        /// 获得群对话框
        /// </summary>
        /// <param name="room"></param>
        private FormTalkRoom GetRoomMsgForm(exRoom room)
        {
            FormTalkRoom fs = null;
            try
            {
                fs = room.Tag as FormTalkRoom;
                if (fs == null || fs.IsDisposed)
                {
                    fs = new FormTalkRoom();//发送消息对话框根据需要替换
                    room.Tag = fs;

                    ///群中创建用户对话窗口事件发生时
                    fs.CreateFormTalkUser += delegate(object sender, exUser user)
                    {
                        FormTalkUser fTalkUser = GetUserMsgForm(user);// 获得用户消息对话框
                        fTalkUser.Show();
                        fTalkUser.Activate();
                    };

                    //更新群事件发生时
                    fs.UpdateRoom += delegate(object sender, exRoom updateRoom)
                    {
                        ChangeRoom changeRoom = new ChangeRoom();
                        changeRoom.type = type.set;
                        changeRoom.RoomID = updateRoom.RoomID;
                        changeRoom.RoomName = updateRoom.RoomName;
                        changeRoom.UserIDs = updateRoom.UserIDs;
                        changeRoom.Notice = updateRoom.Notice;
                        SendMessageToServer(changeRoom);
                        //向服务器请求更新群组资料 
                    };


                    ///当在群里发送消息事件发生时
                    fs.SendMsgToGroup += delegate(IMLibrary3.Protocol.Element e, exRoom Room)
                    {
                        SendMessageToServer(e);//发送消息到服务器
                        if (e is IMLibrary3.Protocol.Message)
                            OpeRecordDB.AddMsg(e as IMLibrary3.Protocol.Message);//将消息添加到数据库
                    }; 

                    fs.FormClosed += delegate(object sender, FormClosedEventArgs e)
                    { fs.Dispose(); fs = null; room.Tag = null; };

                    fs.Room = room;
                    fs.myUserID = MyAuth.UserID ;
                    fs.myUserName = MyAuth.UserName;
                }
            }
            catch (Exception ex)
            {
                IMLibrary3.Global.MsgShow(ex.Message);
            }
            return fs;
        }
 
        #region 创建群事件
        private void tButCreateGroup_Click(object sender, EventArgs e)
        {
            if (frmCreateGroup == null || frmCreateGroup.IsDisposed)
            {
                frmCreateGroup = new FormCreateRoom(MyAuth.UserID,MyAuth.UserName, true);
                frmCreateGroup.CreateRoom += delegate(object senders, exRoom room)
                {
                    ChangeRoom changeRoom = new ChangeRoom();
                    changeRoom.type = type.New;//标明是新建群
                    changeRoom.RoomID = room.RoomID;
                    changeRoom.RoomName = room.RoomName;
                    changeRoom.UserIDs = room.UserIDs;
                    changeRoom.Notice = room.Notice;
                    SendMessageToServer(changeRoom);
                    //发送消息到服务器，创建群组
                };

            }
            frmCreateGroup.Show();
            frmCreateGroup.Activate();
        } 
        #endregion 
         
        #endregion

        #region 发送通知消息菜单单击

        private void tbutSendNoticeMsg_Click(object sender, EventArgs e)
        {
            if (!MyAuth.isSendNotice)
            {
                IMLibrary3.Global.MsgShow("对不起，目前您没有发送通知消息的权限！");
                return;
            }

            this.treeView_Organization.CheckBoxes = true;

            if (frmNotice == null || frmNotice.IsDisposed)
            {
                frmNotice = new FormSendNotice();

                // 消息窗口关闭后事件
                frmNotice.FormClosed += delegate(object senders, FormClosedEventArgs es)
                {
                    this.treeView_Organization.CheckBoxes = false;
                };

                //当发送通知消息事件产生时
                frmNotice.SendNotice += delegate(IMLibrary3.Protocol.Message msg)
                {
                    if (msg == null) return;
                    msg.from = MyAuth.UserID;
                    int selectUserCount = 0;
                    foreach (exUser user in this.Users)
                    {
                        TreeNode treeNode = user.TreeNode as TreeNode;
                        if (treeNode!=null && treeNode.Checked)
                            msg.to += user.UserID + ";";
                        selectUserCount++;
                    }
                    if (selectUserCount > 0)//如果选择接收联系人数大于0
                    {
                        SendMessageToServer(msg);//发送消息
                        OpeRecordDB.AddMsg(msg);//将消息添加到数据库
                        //MessageBox.Show("消息发送成功！");
                    }
                };
            }
            frmNotice.Show();
        }

     
        #endregion 

        #endregion

        #region 工具栏事件
        private void tButBaseUsers_ButtonClick(object sender, EventArgs e)
        {
            treeView_Rooms.Visible = false;
            treeView_Organization.Visible = true;

            ToolStripButton btn = sender  as ToolStripButton;
            if (btn != null)
                btn.CheckState = CheckState.Checked;
        }

        private void tButBaseGroups_ButtonClick(object sender, EventArgs e)
        {
            treeView_Rooms.Visible = true;
            treeView_Organization.Visible =false ;

            ToolStripButton btn = sender as ToolStripButton;
            if (btn != null)
                btn.CheckState = CheckState.Checked;
        }
        #endregion

        #region 查找

        #region 查找组
        /// <summary>
        /// 查找组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public exGroup findGroup(string GroupID)
        {
            if (Groups == null) return null;

            foreach (exGroup group in Groups)
                if (group.GroupID == GroupID)
                    return group;
            return null;
        }
        #endregion

        #region 查找用户
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public exUser findUser(string userID)
        {
            if (Users == null) return null;
            foreach (exUser user in Users)
                if (user.UserID == userID)
                    return user;
            return null;
        }
        #endregion

        #region 查找群
        /// <summary>
        /// 查找群
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public exRoom findRoom(string RoomID)
        {
            if (Rooms == null) return null;

            foreach (exRoom room in Rooms)
                if (room.RoomID == RoomID)
                    return room;
            return null;
        }
        #endregion

        #endregion

        #region 管理组织机构信息

        #region 创建用户菜单事件
        private void TsmCreateUser_Click(object sender, EventArgs e)
        {
            if (!MyAuth.isAdmin)
            {
                IMLibrary3.Global.MsgShow("对不起，目前您没有此项管理权限！");
                return;
            }
            if (frmUserVcard == null || frmUserVcard.IsDisposed)
            {
                frmUserVcard = new FormUserVcard();
                frmUserVcard.Create += delegate(object sender1, UserVcard userVcard)
                { CreateUser(userVcard); };
            }
            frmUserVcard.IsCreate = true;//操作为创建 
            frmUserVcard.Show();
        }

        private void tmsCreateUserVcard_Click(object sender, EventArgs e)
        {
            TsmCreateUser_Click(sender, e);
        }
        #endregion

        #region 删除用户菜单事件
        private void TsmDelUser_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView_Organization.SelectedNode;
            if (node != null && node.Tag is exUser)
            {
                exUser user = node.Tag as exUser;
                if (MessageBox.Show("确定要删除用户 " + user.UserName + "(" + user.UserID + ")吗？", "提示", 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information ) == DialogResult.Yes)
                {
                    UserVcard card = new UserVcard();
                    card.UserID = user.UserID;
                    card.UserName = user.UserName;
                    card.GroupID = user.GroupID;
                    card.type = type.delete;
                    SendMessageToServer(card);//通知服务器删除用户
                }
            }
        }
        #endregion

        #region 菜单弹出事件-显示控制
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            TsmCreateGroup.Visible = false;
            TsmCreateUser.Visible = false;
            TsmDelGroup.Visible = false;
            TsmDelUser.Visible = false;
            TsmShow.Visible = false;
            TsmShowGroupVcard.Visible = false;
            TsmShowUserVcard.Visible = false;

            if (treeView_Organization.SelectedNode != null)
            {
                if (treeView_Organization.SelectedNode.Tag is exGroup)
                {
                    if (MyAuth.isAdmin)
                    {
                        TsmCreateGroup.Visible =true ;
                        TsmCreateUser.Visible = true;
                        TsmDelGroup.Visible = true;
                        TsmShow.Visible = true;
                        TsmShowGroupVcard.Text = "修改分组资料";
                    } 
                    TsmShowGroupVcard.Visible = true;
                }
                if (treeView_Organization.SelectedNode.Tag is exUser)
                {
                    if (MyAuth.isAdmin)
                    {
                        TsmCreateGroup.Visible = true;
                        TsmCreateUser.Visible = true;
                        TsmDelUser.Visible = true;
                        TsmShow.Visible = true;
                        TsmShowUserVcard.Text = "修改用户资料";
                    }
                    TsmShowUserVcard.Visible = true;
                }
            }
        }
        #endregion

        #region 获得用户资料窗口
        /// <summary>
        /// 获得用户资料窗口
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private FormUserVcard getUserVcardForm(string userID)
        {
            FormUserVcard frm = null;
            if (frmUserVcards.ContainsKey(userID))
                frmUserVcards.TryGetValue(userID, out frm);
            return frm;
        }
        #endregion
         
        #region 获得分组资料窗口
        /// <summary>
        /// 获得分组资料窗口
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private FormGroupVcard getGroupVcardForm(string groupID)
        {
            FormGroupVcard frm = null;
            if (frmGroupVcards.ContainsKey(groupID))
                frmGroupVcards.TryGetValue(groupID, out frm);
            return frm;
        }
        #endregion

        #region 显示用户资料菜单事件
        private void TsmShowUserVcard_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView_Organization.SelectedNode;
            if (node != null)
            {
                exUser user = node.Tag as exUser;
                ShowUserVcard(user);
            }
        }

       
        #endregion

        #region 创建分组
        private void TsmCreateGroup_Click(object sender, EventArgs e)
        {
            if (!MyAuth.isAdmin)
            {
                IMLibrary3.Global.MsgShow("对不起，目前您没有此项管理权限！");
                return;
            }
            if (frmGroupVcard == null || frmGroupVcard.IsDisposed)
            {
                frmGroupVcard = new  FormGroupVcard();
                frmGroupVcard.Create += delegate(object sender1, GroupVcard groupVcard)
                {  CreateGroup(groupVcard); };
            }
            frmGroupVcard.IsCreate = true;//操作为创建 
            frmGroupVcard.Show();
            
        }

        private void tmsCreateGroupVcard_Click(object sender, EventArgs e)
        { 
            TsmCreateGroup_Click(sender, e);
        }

        #endregion

        #region 显示分组资料
        private void TsmShowGroupVcard_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView_Organization.SelectedNode;
            if (node != null)
            {
                exGroup group = node.Tag as exGroup;
                FormGroupVcard  frm = getGroupVcardForm(group.GroupID);
                if (frm == null)
                {
                    frm = new FormGroupVcard();
                    frmGroupVcards.Add(group.GroupID, frm);
                    if (MyAuth.isAdmin)//如果是管理员，则添加更新事件
                        frm.UpdateVcard += delegate(object sender1, GroupVcard card)
                    {
                        UpdateGroup(card);//更新分组资料
                    };
                    else
                        frm.RefreshVcard += delegate(object sender1, GroupVcard card)
                        {
                            //向服务器请求获得分组最新资料
                            //card.type = type.get;
                            //SendMessageToServer(card);
                        };
                    frm.FormClosed += delegate(object sender1, FormClosedEventArgs e1)
                    {
                        frm.Dispose();
                        frmGroupVcards.Remove(group.GroupID);
                    };
                    GroupVcard vcard = new GroupVcard();
                    vcard.GroupID = group.GroupID;
                    vcard.GroupName = group.GroupName;
                    vcard.SuperiorID = group.SuperiorID;
                    vcard.OrderID = group.OrderID;
                    frm.GroupVcard = vcard;
                }
                if (MyAuth.isAdmin)//如果是管理员
                    frm.Text = "修改 " + group.GroupName + "(" + group.GroupID + ")的资料";
                else
                    frm.Text = "查看 " + group.GroupName + "(" + group.GroupID + ")的资料";

                frm.isAdmin = MyAuth.isAdmin;
                frm.IsCreate = false;
                frm.Show();
            }

        }
        #endregion 

        #region 删除分组菜单
        private void TsmDelGroup_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView_Organization.SelectedNode;
            if (node != null && node.Tag is exGroup)
            {
                if (node.Nodes.Count > 0)
                {
                    IMLibrary3.Global.MsgShow("请先删除此分组内所有用户和子分组！");
                    return;
                }
                exGroup group = node.Tag as exGroup;
                if (MessageBox.Show("确定要删除分组 " + group.GroupName + "(" + group.GroupID  + ")吗？", "提示",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    GroupVcard  card = new  GroupVcard();
                    card.GroupID = group.GroupID;
                    card.GroupName = group.GroupName;
                    card.type = type.delete;
                    SendMessageToServer(card);//通知服务器删除用户
                }
            }
        }
        #endregion

        #endregion

    }
}
