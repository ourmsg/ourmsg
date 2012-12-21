using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using IMLibrary3;
using IMLibrary3.Organization;

namespace OurMsg
{
    public partial class FormTalkRoom : Form
    {

        #region 构造
        public FormTalkRoom()
        {
            InitializeComponent();
            this.butClose.Click += new EventHandler(butClose_Click);
            this.butSend.Click += new EventHandler(butSend_Click);

            this.listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
            this.MessagePanel1.CreateMsgAfter += new OurMsg.Controls.MessagePanel.CreateMsgEventHandler(MessagePanel1_CreateMsgAfter);
        }
        #endregion

        #region 事件
        public delegate void CreateFormTalkUserEventHandler(object sender, exUser user);

        public event CreateFormTalkUserEventHandler CreateFormTalkUser;

        /// <summary>
        /// 更新群组信息事件
        /// </summary>
        /// <param name="sender">产生事件对像</param>
        /// <param name="group">群组对像</param>
        public delegate void UpdateRoomEventHandler(object sender, exRoom Room);
        /// <summary>
        /// 更新群组信息事件
        /// </summary>
        public event UpdateRoomEventHandler UpdateRoom;

        /// <summary>
        /// 发送事件代理
        /// </summary>
        /// <param name="Message">要发送的消息</param>
        /// <param name="group">组信息</param>
        public delegate void SendMsgEventHandler(IMLibrary3.Protocol.Element  e, exRoom Room);

        /// <summary>
        /// 发送消息事件
        /// </summary>
        public event SendMsgEventHandler SendMsgToGroup;

        #endregion
         
        #region 变量
        /// <summary>
        /// 当前群组
        /// </summary>
        private exRoom _Room = null;

        public exRoom Room
        {
            set
            {
                _Room = value;
                if (value != null)
                {
                    this.Text = "与群 " + value.RoomName + "(" + value.RoomID + ") 对话";
                    this.txtNotice.Text = value.Notice;

                    this.listView1.Items.Clear();
                    if (value != null)
                        foreach (exUser user in Room.Users.Values)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = user.UserName + "(" + user.UserID + ")";
                            item.Tag = user;
                            user.UserShowTypeChanged += new IMLibrary3.Organization.User.ChangedEventHandler(user_UserShowTypeChanged);

                            if (user.ShowType >= IMLibrary3.Enmu.ShowType.Invisible)
                                item.ImageIndex = 0;
                            else
                                item.ImageIndex = 1;

                            this.listView1.Items.Add(item);
                        }
                    SetGroupCount();
                }
            }
            get { return _Room; }
        }
 

        string _myUserID = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public string myUserID
        {
            set
            {
                _myUserID = value;
                MessagePanel1.myUserID = value;
            }
            get { return _myUserID; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string myUserName { get;set;}

        #endregion

        #region 当用户状态发生更改时事件
        /// <summary>
        /// 当用户状态发生更改时,更改图标
        /// </summary>
        /// <param name="sender"></param>
        private void user_UserShowTypeChanged(object sender)
        {
           exUser user = sender as exUser;
            if (user != null)
                foreach (ListViewItem item in listView1.Items)
                {
                    if ((item.Tag as exUser).UserID == user.UserID)
                    {
                        if ((item.Tag as exUser).ShowType >= IMLibrary3.Enmu.ShowType.Invisible)
                            item.ImageIndex = 0;
                        else
                            item.ImageIndex = 1;

                        listView1.Refresh();
                        SetGroupCount();
                        return;
                    }
                }
        }
        #endregion

        #region listview双击事件
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Tag is exUser)
            {
               exUser user = listView1.SelectedItems[0].Tag as exUser;
                if (CreateFormTalkUser != null) CreateFormTalkUser(this, user);
            }

        }
        #endregion

        #region 创建消息事件
        private void MessagePanel1_CreateMsgAfter(object sender, IMLibrary3.Protocol.Message Message)
        {
            Message.MessageType = IMLibrary3.Enmu.MessageType.Group;//消息类型为组对话
            Message.from = myUserID;
            Message.to   = Room.RoomID;//将群组编号存入Tag
            if (SendMsgToGroup != null)
                SendMsgToGroup(Message, Room);
            this.MsgToRichTextBox(Message, true);

            #region 发送截图
            List<IMLibrary3.MyPicture> pictures = MessagePanel1.GetSendPicture();
            if (pictures != null)//如果文件是截图
                foreach (IMLibrary3.MyPicture pic in pictures)
                    if (pic.MD5.Length == 32)
                    {
                        string fileName =myUserID  + "\\sendImage\\" + pic.MD5 + ".gif";
                        ImageFileClient tcpFile = new ImageFileClient(Global.ImageServerEP, fileName);//发送图片文件到服务器
                        tcpFile.fileTransmitted += delegate(object sender1, fileTransmitEvnetArgs e)
                        {
                            if (SendMsgToGroup != null)
                            {
                                IMLibrary3.Protocol.ImageFileMsg fileMsg = new IMLibrary3.Protocol.ImageFileMsg();
                                fileMsg.from = myUserID;
                                fileMsg.to = Room.RoomID;
                                fileMsg.MessageType = IMLibrary3.Enmu.MessageType.Group;
                                fileMsg.Name = "";
                                fileMsg.MD5 = e.fileInfo.MD5;
                                fileMsg.Extension = e.fileInfo.Extension;
                                fileMsg.Length = e.fileInfo.Length;
                                SendMsgToGroup(fileMsg, Room);
                            }
                            (sender1 as ImageFileClient).Dispose();
                            sender1 = null;
                        };
                    }
            #endregion

            this.MessagePanel1.ClearMsgTextBox();
        }
        #endregion

        #region 发送消息单击事件
        private void butSend_Click(object sender, EventArgs e)
        {
            butSend.Enabled = false;
            this.MessagePanel1.SendMsg();
            butSend.Enabled = true;
        }
        #endregion

        #region 关闭单击事件
        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 窗口加载事件
        private void FormGroupChat_Load(object sender, EventArgs e)
        {
            this.MessagePanel1.FaceSet();
        }
        #endregion

        #region 将用户对话消息加入 RichTextBox 控件
        /// <summary>
        /// 将用户对话消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        private void MsgToRichTextBox(IMLibrary3.Protocol.Message msg, bool IsSend)//将发送的消息加入历史rich
        {
            string title = myUserName + "(" +myUserID + ")";
      

            MsgToRichTextBox(msg, IsSend, title);
        }
       
        /// <summary>
        /// 将用户对话消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        /// <param name="title">标题（用户名+用户ID）</param>
        public void MsgToRichTextBox(IMLibrary3.Protocol.Message msg, bool IsSend, string title)//将发送的消息加入历史rich
        {
            this.MessagePanel1.MsgToRichTextBox(msg, IsSend, title);
        }
        #endregion

        #region 设置组统计信息
        /// <summary>
        /// 设置组统计信息
        /// </summary>
        private void SetGroupCount()
        {
            int onlineCount = 0;
            foreach (exUser user in this.Room.Users.Values)
            {
                if (user.ShowType < IMLibrary3.Enmu.ShowType.Invisible)
                    onlineCount++;
            }
            panelGroupCount.Text = "成员(" + onlineCount.ToString() + "/" + this.Room.Users.Count.ToString() + ")";
        }
        #endregion

        #region 设置群资料单击事件
        private void tsButSetGroupData_Click(object sender, EventArgs e)
        {
            FormCreateRoom fs = (this.Room.FormData as FormCreateRoom);
            if (fs == null || fs.IsDisposed)
            {
                this.Room.FormData = new FormCreateRoom(myUserID,myUserName , false);
                fs = (this.Room.FormData as FormCreateRoom);
                fs.UpdateRoom += new FormCreateRoom.UpdateRoomEventHandler(fs_UpdateRoom);
            }

            fs.Room = this.Room;
            
            fs.Show();
            fs.Activate();
        }
        #endregion

        #region 群信息更新事件
        /// <summary>
        /// 群信息更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Room"></param>
        private void fs_UpdateRoom(object sender,exRoom Room)
        {
            if (this.UpdateRoom != null)
                this.UpdateRoom(this, Room);

        }
        #endregion

        #region 获取需要接收的图片文件集合
        public List<IMLibrary3.MyPicture> GetNeedRecPicture()
        {
            return MessagePanel1.needRecPictures;
        }
        #endregion
    }
}
