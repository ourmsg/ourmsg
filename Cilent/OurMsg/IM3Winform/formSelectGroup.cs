using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IMLibrary3.Organization;

namespace IMLibrary3 
{
    public partial class formSelectGroup : Form
    {
        public formSelectGroup( )
        {
            InitializeComponent();
            treeView_Organization.AfterSelect += new TreeViewEventHandler(treeView_Organization_AfterSelect);
        }

       
        void treeView_Organization_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Group = e.Node.Tag as Organization.exGroup;
        }

        /// <summary>
        /// 选择分组
        /// </summary>
        /// <param name="groupID"></param>
        public void selectGroup(string groupID)
        {
            Organization.exGroup group = findGroup(groupID, this.Groups);

            if (group != null)
            {
                Group = group;
                treeView_Organization.SelectedNode = group.TreeNode as TreeNode;
            }
        }

        #region 变量
        /// <summary>
        /// 组织机构分组
        /// </summary>
        List< Organization.exGroup> Groups = null;

        private Organization.exGroup _Group = null;
        public Organization.exGroup Group
        {
            set
            {
                _Group = value;
                if (_Group != null)
                {
                    this.LabTitle.Text = "当前选择：" + _Group.GroupID;
                    this.LabelSelectGroup.Text = "当前选择：" + (_Group.TreeNode as TreeNode).FullPath + "(" + _Group.GroupID + ")";
                }
            }

            get { return _Group; }
        }
        #endregion


        private void formSelectGroup_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载本地数据库中组织机构数据
        /// </summary>
        public void LoadData()
        {
            Groups = OpeRecordDB.GetGroups();
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


            foreach (Organization.exGroup group in this.Groups)
                group.IsShowOnlineInfo = false; 

            TreeNode node = this.addGroupToTreeView(this.Groups);
            
            if (node == null) return;

            foreach (TreeNode nodeTemp in node.Nodes)
            {
              treeView_Organization.Nodes.Add(nodeTemp);
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
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
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
                            node.ImageIndex = 0;
                            node.SelectedImageIndex = 1;
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
         
        #region 查找组
        /// <summary>
        /// 查找组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        private  exGroup findGroup(string GroupID, List< exGroup> groups)
        {
            if (groups == null) return null;

            foreach ( exGroup group in groups)
                if (group.GroupID == GroupID)
                    return group;
            return null;
        }
        #endregion 

        private void tsbOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
