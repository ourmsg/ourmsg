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
    public partial class FormUsersToGroup : Form
    {
        public FormUsersToGroup()
        {
            InitializeComponent();
        }


        #region 变量


        /// <summary>
        /// 组织机构分组
        /// </summary>
        List< exGroup> Groups = null;

        /// <summary>
        /// 选择的用户集合
        /// </summary>
        public  List< exUser> Users = null;

        #endregion

        private void FormUsersToGroup_Load(object sender, EventArgs e)
        {
            Groups = OpeRecordDB.GetGroups();
            Users = OpeRecordDB.GetUsers();
            LoadLocalOrg();
            Users = new List<exUser>();//重新添加用户
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

            TreeNode node = this.addGroupToTreeView(this.Groups);
            this.addUserToTreeView(this.Users, this.Groups);
            if (node == null) return;

            foreach (TreeNode nodeTemp in node.Nodes)
            {
                treeView_Organization.Nodes.Add(nodeTemp);
                nodeTemp.Expand();
            }

        }

        private TreeNode addNodeToTree(string Text, int imageIndex)
        {
            TreeNode node = new TreeNode(Text);
            node.SelectedImageIndex = imageIndex;
            node.ImageIndex = imageIndex;
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
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 2;
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
                            node.ImageIndex = 1;
                            node.SelectedImageIndex = 2;
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
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
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
        private exGroup findGroup(string GroupID, List<exGroup> groups)
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
        private exRoom findGroup(string GroupID, List<exRoom> groups)
        {
            if (groups == null) return null;

            foreach (exRoom group in groups)
                if (group.RoomID == GroupID)
                    return group;
            return null;
        }
        #endregion

        #endregion

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void butOK_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
              exUser user=  item.Tag as exUser;
              if (user != null) Users.Add(user);
            }
            this.Close();
        }

        private void treeView_Organization_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is exUser)
            {
                foreach (ListViewItem tempItem in listView1.Items)
                    if (tempItem.Tag == e.Node.Tag)
                        return;
                exUser user = e.Node.Tag as  exUser;

                ListViewItem item = new ListViewItem(user.UserName + "(" + user.UserID + ")");
                item.ImageIndex = 0;
                item.Tag = e.Node.Tag;
                listView1.Items.Add(item);
                setSelUsersCount();
            }
        }

        private void setSelUsersCount()
        {
            panel6.Text = "已选联系人(" + listView1.Items.Count.ToString() + ")";
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
                setSelUsersCount();
            }
        }

        private void butAddUser_Click(object sender, EventArgs e)
        {
            if (treeView_Organization.SelectedNode == null) return;
        }

        private void butDelUser_Click(object sender, EventArgs e)
        {
            listView1_MouseDoubleClick(null, null);
        }
         
    }
}
