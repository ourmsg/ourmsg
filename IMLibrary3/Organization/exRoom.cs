using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary3.Organization
{
    /// <summary>
    /// 扩展群组
    /// </summary>
    public class exRoom:IMLibrary3.Organization.Room 
    {

        public exRoom()
        {
            this.RoomNameChanged += new RoomNameChangedEventHandler(exRoom_RoomNameChanged);
        }

        void exRoom_RoomNameChanged(object sender)
        {
            if (this.TreeNode is TreeNode)
            {
                TreeNode node = this.TreeNode as TreeNode;
                node.Text = this.RoomName + "(" + this.RoomID + ")";
            }
        }
 

        /// <summary>
        /// 显示树节点，开发人员可根据需要设置为其它的控件的节点对像,OurMsg中此属性为.net TreeNode
        /// </summary>
        public object TreeNode
        {
            set ;
            get;
        }

        /// <summary>
        /// 自定义群组资料设置窗口
        /// </summary>
        public object FormData
        {
            set;
            get;
        }
    }
}
