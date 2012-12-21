using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IMLibrary3;
using IMLibrary3.Data;
using IMLibrary3.Organization;

using System.Data.SQLite;

namespace OurMsg 
{
    public partial class FormDataManage : Form 
    {
        public FormDataManage()
        {
            InitializeComponent();
            this.treeView_Organization.AfterSelect += new TreeViewEventHandler(treeView_Organization_AfterSelect);
        }

        #region 变量

        /// <summary>
        /// 当前浏览的页
        /// </summary>
        private int CurrPage = 0;

        /// <summary>
        /// 页的总数
        /// </summary>
        private int PageCount = 0;

        /// <summary>
        /// 每一页显示的记录数
        /// </summary>
        private int pageSize = 20;

        /// <summary>
        /// 对话类型
        /// </summary>
        private IMLibrary3.Enmu.MessageType MessageType = IMLibrary3.Enmu.MessageType.Group;

        /// <summary>
        /// 组织机构分组
        /// </summary>
        List<exGroup> Groups =null ;

        /// <summary>
        /// 用户
        /// </summary>
        List<exUser> Users = null;

        /// <summary>
        /// 自定义组
        /// </summary>
        List<exRoom> Rooms = null;

        /// <summary>
        /// 当前用户帐号
        /// </summary>
        public string myUserID = "";

        /// <summary>
        /// 选择用户帐号
        /// </summary>
        string SelectUserID = "";

        /// <summary>
        /// 获取对话消息
        /// </summary>
        IMLibrary3.Protocol.Message Msg =null;
        #endregion

        #region 树图节点选中后事件
        /// <summary>
        /// 树图节点选中后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Organization_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.txtRecord.Clear();

            this.ButLast.Enabled = false;
            this.ButDown.Enabled = false;
            this.ButFirst.Enabled = false;
            this.ButUp.Enabled = false;
            this.ButDelRecord.Enabled = false;

            this.PageCount = 0;
            this.CurrPage = 0;

            if (e.Node.Tag is exUser)
            {
                exUser user = e.Node.Tag as exUser;
                this.SelectUserID=user.UserID;
                this.MessageType = IMLibrary3.Enmu.MessageType.User;
            }
            else if (e.Node.Tag is exRoom)
            {
               exRoom room = e.Node.Tag as exRoom;
                this.SelectUserID = room.RoomID ;
                this.MessageType = IMLibrary3.Enmu.MessageType.Group;              
            }
            else if (e.Node.ImageIndex == 17)
                this.MessageType = IMLibrary3.Enmu.MessageType.Notice; 
    
            this.PageCount = GetPageCount(this.SelectUserID);
            if (this.PageCount > 0)
                DBRecordToRichTextBox(1);

            this.TextBoxPage.Text = this.CurrPage.ToString() + "/" + this.PageCount.ToString();
        }

        #endregion

        #region 获得对话记录消息总行数
        /// <summary>
        /// 获得对话记录消息总行数
        /// </summary>
        /// <param name="userID">发送者ID</param>
        /// <param name="MyID">自己的ID</param>
        /// <returns></returns>
        private int GetPageCount(string userID)//获得页数
        {
            int count = 0;

            string sql = "";
            switch (this.MessageType)
            {
                case IMLibrary3.Enmu.MessageType.User:
                    sql = string.Format("select Count(*) from [MsgRecord] where ((froms='{0}' and tos='{1}') or (froms='{1}' and tos='{0}')) And MessageType={2}  ORDER BY DateTime DESC", userID, myUserID, (byte)this.MessageType);
                    break;
                case IMLibrary3.Enmu.MessageType.Group:
                    sql = string.Format("select Count(*) from [MsgRecord] where  tos='{0}' and MessageType={1} ORDER BY DateTime DESC", SelectUserID, (byte)this.MessageType);
                    break;
                case IMLibrary3.Enmu.MessageType.Notice:
                    sql = string.Format("select Count(*) from [MsgRecord] where MessageType={0} ORDER BY DateTime DESC", (byte)this.MessageType);
                    break;
            }


            SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null);

            if (dr == null) return count;
            while (dr.Read())
            {
                count = Convert.ToInt32(dr[0]);
            }
            dr.Close(); dr.Dispose();


            int pageCount = Convert.ToInt32(count / this.pageSize);
            if ((count % this.pageSize) != 0)
                pageCount += 1;
            //MessageBox.Show(count.ToString());
            return pageCount;
        }

        private void addRecord(long ID)
        {
            //RecordIndex record = new RecordIndex(ID);
            //this.recordCollention.add(record);
        }
         
        #endregion

        #region 浏览分页记录
        private void DBRecordToRichTextBox(int Page)
        {
            if (Page > PageCount) return;//如果页数超过当前页数，则退出
            if (Page <= 0) return;//如果页数为负数，则退出

            this.txtRecord.Clear();
            this.CurrPage = Page;

            this.ButLast.Enabled = true;
            this.ButDown.Enabled = true;
            this.ButFirst.Enabled = true;
            this.ButUp.Enabled = true;
            this.ButDelRecord.Enabled = true;


            if (Page == this.PageCount)
            {
                this.ButLast.Enabled = false;
                this.ButDown.Enabled = false;
            }
            if (Page == 1)
            {
                this.ButFirst.Enabled = false;
                this.ButUp.Enabled = false;
            }
            this.TextBoxPage.Text = Page.ToString() + "/" + this.PageCount.ToString();

            bool IsSend = false;//标识此消息是否是对方发送的

            #region sql表达式
            string sql = "";
            switch (this.MessageType)
            {
                case IMLibrary3.Enmu.MessageType.User:
                    sql = string.Format("select *  from [MsgRecord] where ((froms='{0}' and tos='{1}') or (froms='{1}' and tos='{0}')) And MessageType={2}  ORDER BY DateTime DESC limit " + (Page - 1) * pageSize + "," + pageSize, SelectUserID, myUserID, (byte)this.MessageType);
                    break;
                case IMLibrary3.Enmu.MessageType.Group:
                    sql = string.Format("select *  from [MsgRecord] where tos='{0}' and MessageType={1} ORDER BY DateTime DESC limit " + (Page - 1) * pageSize + "," + pageSize, SelectUserID, (byte)this.MessageType);
                    break;
                case IMLibrary3.Enmu.MessageType.Notice:
                    sql = string.Format("select *  from [MsgRecord] where MessageType={0} ORDER BY DateTime DESC limit " + (Page - 1) * pageSize + "," + pageSize, (byte)this.MessageType);
                    break;
            }
            #endregion

            try
            {
                SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null);
                if (dr == null) return;
                while (dr.Read())
                {
                    IsSend = false;//标识此消息否是对方发送的

                    #region 获得消息类
                    Msg = OpeRecordDB.GetDrMsg(dr);
                    if (Msg == null) continue;
                    #endregion

                    exUser sendUserInfo = findUser(Msg.from, Users);
                    if (sendUserInfo == null) return;

                    if (sendUserInfo.UserID == this.myUserID)//此消息为对方发送的
                        IsSend = true;

                    int iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
                    string title = sendUserInfo.UserName + "(" + sendUserInfo.UserID + ")" + Msg.DateTime ;
                    Color titleColor = Color.Red;
                    if (!IsSend)//如果是收到消息
                        titleColor = Color.Blue;

                    txtRecord.AppendText(title);
                    txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
                    txtRecord.SelectionFont = new Font("宋体", 10);
                    txtRecord.SelectionColor = titleColor;
                    txtRecord.AppendText("\n  ");

                    textMsgToRich(Msg, IsSend);
                }
                dr.Close(); dr.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
        } 
        #endregion

        #region 添加文本消息到 richBox
        /// <summary>
        /// 添加文本消息到richBox
        /// </summary>
        /// <param name="IsSend">是否对方发送的消息</param>
        private void textMsgToRich(IMLibrary3.Protocol.Message Msg, bool IsSend)//添加文本消息与图片到richBox
        {
            int iniPos = this.txtRecord.TextLength;//获得当前记录richBox中最后的位置

            if (Msg.ImageInfo != "")//如果消息中有图片，则添加图片
            {
                string[] imagePos = Msg.ImageInfo.Split('|');
                int addPos = 0;//
                int currPos = 0;//当前正要添加的文本位置
                int textPos = 0;
                for (int i = 0; i < imagePos.Length - 1; i++)
                {
                    string[] imageContent = imagePos[i].Split(',');//获得图片所在的位置、图片名称、图片宽、高

                    currPos = Convert.ToInt32(imageContent[0]);//获得图片所在的位置

                    this.txtRecord.AppendText(Msg.Content.Substring(textPos, currPos - addPos));
                    this.txtRecord.SelectionStart = this.txtRecord.TextLength;

                    textPos += currPos - addPos;
                    addPos += currPos - addPos;

                    Image image=null;
                    if ( txtRecord.GetPicture(imageContent[1]) == null)
                    {
                        if (System.IO.File.Exists(Application.StartupPath + "\\face\\" + imageContent[1] + ".gif") )
                        {
                            image = System.Drawing.Image.FromFile(Application.StartupPath + @"\face\" + imageContent[1] + ".gif");
                        }
                        else if (IsSend)//如果是对方发送过来的
                        {
                            if (System.IO.File.Exists(myUserID + "\\sendImage\\" + imageContent[1] + ".gif"))
                                image = System.Drawing.Image.FromFile(myUserID + "\\sendImage\\" + imageContent[1] + ".gif");
                        }
                        else if (!IsSend)//如果是自己发送的
                        {
                            if (System.IO.File.Exists(myUserID + "\\ArrivalImage\\" + imageContent[1] + ".gif"))
                                image = System.Drawing.Image.FromFile(myUserID + "\\ArrivalImage\\" + imageContent[1] + ".gif");
                        }
                    }
                    else
                        image = this.txtRecord.GetPicture(imageContent[1]).Image;
                     
                    this.txtRecord.addGifControl(imageContent[1], image);
                    addPos++;
                }
                this.txtRecord.AppendText(Msg.Content.Substring(textPos, Msg.Content.Length - textPos) + "\n");
            }
            else//如果消息中没有图片，则直接添加消息文本
            {
                this.txtRecord.AppendText(Msg.Content + "\n");
            }

            this.txtRecord.Select(iniPos, this.txtRecord.TextLength - iniPos);
            this.txtRecord.SelectionFont =Msg.Font ;
            this.txtRecord.SelectionColor = Msg.Color ;
            this.txtRecord.Select(this.txtRecord.TextLength, 0);
        }
        #endregion

        #region 将用户通知消息加入 RichTextBox 控件
        /// <summary>
        /// 将用户通知消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        public void NoticeMsgToRichTextBox(IMLibrary3.Protocol.Message msg,exUser user)//将发送的消息加入历史rich
        {
            int iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置

            if (msg.Title != "")
            {
                #region 添加标题
                txtRecord.AppendText(msg.Title);
                txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
                txtRecord.SelectionFont = new Font("宋体", 18);
                txtRecord.SelectionColor = msg.Color;
                txtRecord.SelectionAlignment = HorizontalAlignment.Center;
                txtRecord.AppendText("\n\n");
                #endregion
            }

            #region 添加内容
            iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置

            #region //如果消息中有图片，则添加图片
            if (msg.ImageInfo != "")//如果消息中有图片，则添加图片
            {
                string[] imagePos = msg.ImageInfo.Split('|');
                int addPos = 0;//
                int currPos = 0;//当前正要添加的文本位置
                int textPos = 0;
                for (int i = 0; i < imagePos.Length - 1; i++)
                {
                    string[] imageContent = imagePos[i].Split(',');//获得图片所在的位置、图片名称 
                    currPos = Convert.ToInt32(imageContent[0]);//获得图片所在的位置

                    this.txtRecord.AppendText(msg.Content.Substring(textPos, currPos - addPos));
                    this.txtRecord.SelectionStart = this.txtRecord.TextLength;

                    textPos += currPos - addPos;
                    addPos += currPos - addPos;

                    Image image;

                    if (this.txtRecord.GetPicture(imageContent[1]) == null)
                        image = System.Drawing.Image.FromFile(Application.StartupPath + @"\face\" + imageContent[1] + ".gif");
                    else
                        image = this.txtRecord.GetPicture(imageContent[1]).Image;

                    this.txtRecord.addGifControl(imageContent[1], image);
                    addPos++;
                }
                this.txtRecord.AppendText(msg.Content.Substring(textPos, msg.Content.Length - textPos) + "  \n");
            }
            #endregion
            #region  //如果消息中没有图片，则直接添加消息文本
            else//如果消息中没有图片，则直接添加消息文本
            {
                txtRecord.AppendText(msg.Content + "\n");
            }
            #endregion
            txtRecord.Select(iniPos, this.txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = msg.Font;
            txtRecord.SelectionColor = msg.Color;
            txtRecord.SelectionAlignment = HorizontalAlignment.Left;
            #endregion

            if (msg.remark != "")
            {
                #region 添加落款
                iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
                txtRecord.AppendText("\n" + msg.remark);
                txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
                txtRecord.SelectionFont = new Font("宋体", 13);
                txtRecord.SelectionColor = msg.Color;
                txtRecord.SelectionAlignment = HorizontalAlignment.Right;
                txtRecord.AppendText("\n  ");
                #endregion
            }


            iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
            txtRecord.AppendText("\n发送用户：" + user.UserName + "(" + user.UserID + ")\n");
            txtRecord.AppendText("发送时间：" + System.DateTime.Now.ToString() + "\n");
            txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = txtRecord.Font;
            txtRecord.SelectionAlignment = HorizontalAlignment.Left;

            txtRecord.Select(this.txtRecord.TextLength, 0);
        }
        #endregion

        #region 工具栏单击事件

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToolTipText)
            {
                case "第一页":
                    DBRecordToRichTextBox(1);
                    break;
                case "上一页":
                    DBRecordToRichTextBox(this.CurrPage -1);
                    break;
                case "下一页":
                    DBRecordToRichTextBox(this.CurrPage+ 1);
                    break;
                case "最后一页":
                    DBRecordToRichTextBox(this.PageCount);
                    break;
                case "关闭":
                    this.Dispose();
                    break;
                case "删除记录":
                    delRecord();
                    break;
            }
        }
        #endregion

        #region 删除记录
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="IsNotNotice"></param>
        private void delRecord()
        {
            string sql = "1=1";
            if (MessageBox.Show("确定要删除当前的所有对话记录吗?", "提示", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                switch (this.MessageType)
                {
                    case IMLibrary3.Enmu.MessageType.User:
                        sql = string.Format("delete from [MsgRecord] where ((froms='{0}' and tos='{1}') or (froms='{1}' and tos='{0}')) And MessageType={2}", SelectUserID, myUserID, (byte)this.MessageType);
                        break;
                    case IMLibrary3.Enmu.MessageType.Group:
                        sql = string.Format("delete from [MsgRecord] where  tos='{0}' and MessageType={1}", SelectUserID, (byte)this.MessageType);
                        break;
                    case IMLibrary3.Enmu.MessageType.Notice:
                        sql = string.Format("delete from [MsgRecord] where MessageType={0}", (byte)this.MessageType);
                        break;
                }
                SQLiteDBHelper.ExecuteNonQuery(sql);
            }
            this.txtRecord.Clear();
        }
        #endregion

        #region 关键菜单事件
 
        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "MenuItemCopy":
                    this.txtRecord.Copy();
                    break;
                case "MenuItemPaset":
                    this.txtRecord.Paste();
                    break;
                case "MenuItemCut":
                    this.txtRecord.Cut();
                    break;
                case "MenuItemDel":
                    this.txtRecord.SelectedText = "";
                    break;
                case "MenuItemSelAll":
                    this.Focus();
                    this.txtRecord.SelectAll();
                    break;
            }
        }

        private void RTBRecord_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch
            {
                //MessageBox.Show("无法打开链接。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        #endregion

        private void FormDataManage_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            Groups = OpeRecordDB.GetGroups();
            Users =  OpeRecordDB.GetUsers();
            Rooms =  OpeRecordDB.GetRooms();
            LoadLocalOrg();
        }

        #region 私有方法

        #region 加载本地数据库中的组织机构过程
        private delegate void delegateLoadLocalOrg();//委托代理
        /// <summary>
        /// 加载本地数据库中的组织机构
        /// </summary>
        private void LoadLocalOrg()
        {
            if (this.treeView_Organization.Nodes.Count > 0) return;

            TreeNode nodeOrg = addNodeToTree("所有分组", 14);
            TreeNode nodeGroups=addNodeToTree("组", 16);

            addNodeToTree("通知消息", 17);

            foreach ( exGroup group in this.Groups)
                group.IsShowOnlineInfo = false;
            foreach (exUser user in this.Users)
                user.IsShowOnlineInfo = false;

            TreeNode node = this.addGroupToTreeView(this.Groups);
            this.addUserToTreeView(this.Users, this.Groups);
            if (node == null) return;

            foreach (TreeNode nodeTemp in node.Nodes)
            {
                nodeOrg.Nodes.Add(nodeTemp);
            }

            ///添加群组
            foreach ( exRoom group in this.Rooms)
            {
                TreeNode nodeTemp = new TreeNode(group.RoomName + "(" + group.RoomID + ")");
                nodeTemp.Tag = group;
                nodeTemp.ImageIndex = 16;
                nodeTemp.SelectedImageIndex = 16;

                nodeGroups.Nodes.Add(nodeTemp);
            }

            nodeOrg.Expand();
            nodeGroups.Expand();
        }

        private TreeNode addNodeToTree(string Text, int imageIndex)
        {
            TreeNode node=new TreeNode(Text);
            node.SelectedImageIndex =imageIndex;
            node.ImageIndex =imageIndex ;
            this.treeView_Organization.Nodes.Add(node);
            return node;
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
                if (findGroup(group.SuperiorID, groups) == null)
                {
                    TreeNode node = new TreeNode();
                    node.Text = group.GroupName;
                    node.ToolTipText = group.GroupName;
                    node.ImageIndex = 14;
                    node.SelectedImageIndex = 15;
                    node.Tag = group;
                    group.TreeNode = node;
                    nodeTemp.Nodes.Add(node);
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
                    parentGroup = findGroup(group.SuperiorID, groups);//找到上级部门节点
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
        private void addUserToTreeView(List<exUser> users, List<exGroup> groups)
        {
            if (users == null || groups == null) return;

            foreach (exUser user in users)
            {
                exGroup group = findGroup(user.GroupID, groups);
                if (group != null && group.TreeNode != null)
                {
                    TreeNode node = new TreeNode();
                    node.Text = user.UserName;
                    node.ToolTipText = user.UserName;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    node.Tag = user;
                    user.TreeNode = node;
                    TreeNode groupNode = group.TreeNode as TreeNode;
                    if (groupNode != null)
                        groupNode.Nodes.Add(node);

                    //user.Group = group;
                    //user.Group.UserCount++;
                }

            }
        }
        #endregion

        #region 查找组
        /// <summary>
        /// 查找组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        private  exGroup findGroup(string GroupID, List<exGroup> groups)
        {
            if (groups == null) return null;

            foreach (exGroup group in groups)
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
        private exUser findUser(string userID, List<exUser> users)
        {
            if (users == null) return null;

            foreach (exUser user in users)
                if (user.UserID == userID)
                    return user;
            return null;
        }
        #endregion

        #region 查找自定义群组
        /// <summary>
        /// 查找自定义群组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        private exRoom findGroup(string RoomID, List<exRoom> Rooms)
        {
            if (Rooms == null) return null;

            foreach (exRoom room in Rooms)
                if (room.RoomID == RoomID)
                    return room;
            return null;
        }
        #endregion

        #endregion
    }
}