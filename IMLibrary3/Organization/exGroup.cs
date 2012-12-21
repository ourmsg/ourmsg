using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Organization
{
    public class exGroup:IMLibrary3.Organization.Group 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public exGroup()
        {
            CreateEventHandler();
        }

        private void CreateEventHandler( )
        {
            this.GroupNameChanged += new ChangedEventHandler(SetGroupText);
        }

        private void SetGroupText(object sender)
        {
            System.Windows.Forms.TreeNode node = this.TreeNode as System.Windows.Forms.TreeNode;
            if (node == null) return;

            if (IsShowOnlineInfo)
                node.Text = this.GroupName + "(" + this.OnLineUserCount.ToString() + "/" + this.UserCount.ToString() + ")";
            else
                node.Text = this.GroupName + "(" + this.UserCount.ToString() + ")";
        }
 
        /// <summary>
        /// 是否显示在线信息
        /// </summary>
        public bool IsShowOnlineInfo=true;

        /// <summary>
        /// 显示树节点，开发人员可根据需要设置为其它的控件的节点对像,OurMsg中此属性为.net TreeNode
        /// </summary>
        public object TreeNode
        {
            set;
            get;
        }

        /// <summary>
        /// 上级分组
        /// </summary>
        public exGroup SuperiorGroup = null;
    
        private  int _OnLineUserCount=0;    
        /// <summary>
        /// 设置或获取分组内的在线用户数
        /// </summary>
        public int OnLineUserCount
        {
            set
            {
                if (this.SuperiorGroup != null)//如果有父节点
                    if (_OnLineUserCount > value)
                        this.SuperiorGroup.OnLineUserCount -= 1;
                    else
                        this.SuperiorGroup.OnLineUserCount += 1;

                if (_OnLineUserCount > value)//如果原来的数大于现在的数，在线人数减少
                    _OnLineUserCount--;
                else//否则增加
                    _OnLineUserCount++;

                SetGroupText(this);
            }
            get { return _OnLineUserCount; }
        }
 
        private int _UserCount = 0;
        /// <summary>
        /// 设置或获取分组内用户数
        /// </summary>
        public int UserCount
        {
            set
            {
                if (_UserCount< value && this.SuperiorGroup != null)
                    this.SuperiorGroup.UserCount += 1;
                if (_UserCount > value && this.SuperiorGroup != null)
                    this.SuperiorGroup.UserCount -= 1;

                _UserCount = value;
   
                SetGroupText(this);
            }
            get { return _UserCount; }
        }
    }
}
