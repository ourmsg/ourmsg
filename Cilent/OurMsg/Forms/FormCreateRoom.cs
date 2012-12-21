using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IMLibrary3.Organization;
  
namespace OurMsg
{
    public partial class FormCreateRoom : Form
    {
        #region 初始化
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyUserID"></param>
        /// <param name="MyUserName"></param>
        /// <param name="isCreate"></param>
        public FormCreateRoom(string MyUserID, string MyUserName, bool isCreate)
        {
            InitializeComponent();
            myUserID = MyUserID;
            myUserName = MyUserName;

            this.textBoxCreateUser.Text = myUserID;

            ListViewItem item = new ListViewItem(myUserID);
            item.SubItems.Add(myUserName);
            item.ImageIndex = 0;
            item.Tag = myUserID;
            listViewGroupUsers.Items.Add(item);
            setSelUsersCount();

            if (isCreate)
                this.butCreateGroup.Visible = true;
            else
                this.butOK.Visible = true;
            IsCreate = isCreate;
        }
        #endregion

        #region 变量区
        /// <summary>
        /// 更新群组信息超时记数器
        /// </summary>
        private int outTime = 0;

        /// <summary>
        /// 是否创建群组，否为更新群组信息
        /// </summary>
        private bool IsCreate = false;

        /// <summary>
        /// 创建或更新群组信息是否成功
        /// </summary>
        public bool isUpdateSuccess = false;
         
        /// <summary>
        /// 用户ID
        /// </summary>
        public string myUserID { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string myUserName { get; set; }
         
        private string oldVersion = "";

        private exRoom _Room = new exRoom();
        /// <summary>
        /// 群组信息
        /// </summary>
        public exRoom Room
        {
            set
            {
                _Room = value;

                this.textBoxCreateUser.Text = value.CreateUserID;
                this.textBoxGroupID.Text = value.RoomID;
                this.textBoxGroupNotice.Text = value.Notice;
                this.textBoxGroupName.Text = value.RoomName;

                foreach ( exUser user in value.Users.Values )
                {
                    bool t = false;
                    foreach (ListViewItem tempItem in listViewGroupUsers.Items)
                        if ((tempItem.Tag as string) == user.UserID)
                        {
                            t = true;
                            continue;
                        }

                    if (!t)
                    {
                        ListViewItem item = new ListViewItem(user.UserID);
                        item.SubItems.Add(user.UserName);
                        item.ImageIndex = 0;
                        item.Tag = user.UserID;
                        listViewGroupUsers.Items.Add(item);
                        setSelUsersCount();
                    }
                }
                oldVersion = _Room.RoomName.Trim() + _Room.Notice.Trim() + _Room.UserIDs;
            }
            get { return _Room; }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 创建群组事件代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Room"></param>
        public delegate void CreateRoomEventHandler(object sender,exRoom Room);

        /// <summary>
        /// 创建群组事件
        /// </summary>
        public event CreateRoomEventHandler CreateRoom;


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

        #endregion

        #region 窗口事件
        private void FormCreateRoom_Load(object sender, EventArgs e)
        {


        }  
        #endregion

        #region 添加用户
        private void butAddUsers_Click(object sender, EventArgs e)
        {
            FormUsersToGroup fs = new FormUsersToGroup();
            fs.ShowDialog(this);
            
            foreach ( exUser user in fs.Users)
            {
                bool t = false;
                foreach (ListViewItem tempItem in listViewGroupUsers.Items)
                    if ((tempItem.Tag as string) == user.UserID)
                    {
                        t = true;
                        continue;
                    }

                if (!t)
                {
                    ListViewItem item = new ListViewItem(user.UserID);
                    item.SubItems.Add(user.UserName);
                    item.ImageIndex = 0;
                    item.Tag = user.UserID;
                    listViewGroupUsers.Items.Add(item);
                    setSelUsersCount();
                }
            }
        }
        #endregion

        #region 取消
        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 设置用户总数
        private void setSelUsersCount()
        {
            labelUserCount.Text = listViewGroupUsers.Items.Count.ToString();
        }
        #endregion

        #region 选择用户索引
        private void listViewGroupUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewGroupUsers.Items)
                if (item.Selected)
                    item.BackColor = Color.Blue;
                else
                    item.BackColor = listViewGroupUsers.BackColor;

            if (listViewGroupUsers.SelectedItems.Count > 0
                  && (listViewGroupUsers.SelectedItems[0].Tag as string) !=myUserID )
                this.butDelUser.Enabled = true;
            else
                this.butDelUser.Enabled = false;
        }
        #endregion

        #region 删除用户
        private void butDelUser_Click(object sender, EventArgs e)
        {
            if (listViewGroupUsers.SelectedItems.Count > 0)
            {
                listViewGroupUsers.Items.Remove(listViewGroupUsers.SelectedItems[0]);
                setSelUsersCount();
                this.butDelUser.Enabled = false;
            }
        }
        #endregion

        #region 超时时钟事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            outTime++;
            if (isUpdateSuccess)
            {
                timer1.Enabled = false;
                if (!IsCreate)
                {
                    MessageBox.Show("更新组信息成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("创建组信息成功！群号码："+ this.Room.RoomID, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else if (outTime > 10)
            {
                timer1.Enabled = false;
                MessageBox.Show("创建或更新组信息超时，请重新操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.butCreateGroup.Enabled = true;
                this.butOK.Enabled = true;

                outTime = 0;
            }
        }
        #endregion

        #region 创建群组按钮
        private void butCreateGroup_Click(object sender, EventArgs e)
        {  
            if (this.textBoxGroupName.Text.Trim() == "")
            {
                MessageBox.Show("组名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBoxGroupName.Focus();
                return;
            }

            this.butCreateGroup.Enabled = false;
            this.isUpdateSuccess = false;

           exRoom NewRoom = new exRoom();
            NewRoom.RoomID = this.textBoxGroupID.Text.Trim();
            NewRoom.UserIDs = "";
            NewRoom.CreateUserID = this.textBoxCreateUser.Text;//群组创建用户编号
            NewRoom.RoomName = this.textBoxGroupName.Text;//群组名称
            NewRoom.Notice = this.textBoxGroupNotice.Text;//群组通知

            foreach (ListViewItem item in this.listViewGroupUsers.Items)
                NewRoom.UserIDs += item.Text + ";";

            if (CreateRoom != null)
                CreateRoom(this, NewRoom);

            this.timer1.Enabled = true;//启动检测更新
        }
        #endregion

        #region 更新群组按钮事件
        private void butOK_Click(object sender, EventArgs e)
        {
            if (!IsCreate && myUserID  != textBoxCreateUser.Text)
            {
                this.Close();
                return;
            }

            if (this.textBoxGroupName.Text.Trim() == "")
            {
                MessageBox.Show("组名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBoxGroupName.Focus();
                return;
            } 
            this.butOK.Enabled = false;
            this.isUpdateSuccess = false;

           exRoom NewRoom = new exRoom();
            NewRoom.UserIDs = "";
            NewRoom.RoomID= this.textBoxGroupID.Text;//组编号
            NewRoom.CreateUserID = this.textBoxCreateUser.Text;//群组创建用户编号
            NewRoom.RoomName = this.textBoxGroupName.Text;//群组名称
            NewRoom.Notice = this.textBoxGroupNotice.Text;//群组通知

            foreach (ListViewItem item in this.listViewGroupUsers.Items)
                NewRoom.UserIDs += item.Text + ";";

            string newVersion = NewRoom.RoomName.Trim() + NewRoom.Notice.Trim() + NewRoom.UserIDs;

            if (newVersion == oldVersion)//如果未做修改，则退出
            {
                this.Close();
                return;
            }

            if (this.UpdateRoom != null)
                this.UpdateRoom(this, NewRoom);

            this.timer1.Enabled = true;//启动检测更新
        }
        #endregion


    }

}