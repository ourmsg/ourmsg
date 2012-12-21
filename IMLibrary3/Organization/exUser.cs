using System;
using System.Collections.Generic;
using System.Text;


namespace IMLibrary3.Organization
{
    /// <summary>
    /// 扩展用户
    /// </summary>
    public class exUser:IMLibrary3.Organization.User 
    {
        /// <summary>
        /// 
        /// </summary>
        public exUser()
        {
            this.UserShowTypeChanged += new ChangedEventHandler(User_UserShowTypeChanged);
            this.UserOfflinedOrOnlined += new ChangedEventHandler(exUser_UserOfflinedOrOnlined);
            this.UserNameChanged += new ChangedEventHandler(User_UserNameChanged);
        }

        private void exUser_UserOfflinedOrOnlined(object sender)
        {
            if (Group != null)
                if (ShowType >= IMLibrary3.Enmu.ShowType.Invisible)//如果离线
                    Group.OnLineUserCount++;
                else//如果上线
                    Group.OnLineUserCount--;

        }

        private void User_UserNameChanged(object sender)
        {
            User_UserShowTypeChanged(sender);
        }

        private void User_UserShowTypeChanged(object sender)
        {
            System.Windows.Forms.TreeNode node = this.TreeNode as System.Windows.Forms.TreeNode;
            if (node == null) return;

            if (IsShowOnlineInfo)
            {
                node.Text = UserName + this.Status;
                if (ShowType >= IMLibrary3.Enmu.ShowType.Invisible)
                {
                    node.SelectedImageIndex = 0;
                    node.ImageIndex = 0;
                }
                else
                {
                    node.SelectedImageIndex = 1;
                    node.ImageIndex = 1;
                }
            }
            else
            {
                node.SelectedImageIndex = 1;
                node.ImageIndex = 1;
                node.Text = UserName;
            }
        }

        /// <summary>
        /// 是否显示在线信息
        /// </summary>
        public bool IsShowOnlineInfo = true;

        /// <summary>
        /// 显示树节点，开发人员可根据需要设置为其它的控件的节点对像,OurMsg中此属性为.net TreeNode
        /// </summary>
        public object  TreeNode
        {
            set;
            get;
        }

        /// <summary>
        /// 所在分组
        /// </summary>
        public exGroup Group = null;
    }
}
