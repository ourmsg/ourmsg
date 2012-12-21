using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IMLibrary3.Organization
{

    #region 组织机构分组信息类
    /// <summary>
    ///  组织机构分组信息类
    /// </summary>
    public class Group : PropertyGirdBaseObject.BaseObject
    {
        /// <summary>
        /// 组织机构分组信息类
        /// </summary>
        public Group()
        {

        }

        /// <summary>
        /// 分组信息名称改变后事件代理
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ChangedEventHandler(object sender);

        /// <summary>
        /// 分组信息名称改变后事件 
        /// </summary>
        public event ChangedEventHandler GroupNameChanged;


        /// <summary>
        /// 分组ID
        /// </summary>
        [
        CategoryAttribute("全局设置"),
        ReadOnlyAttribute(true),
        DescriptionAttribute("分组ID"),
        DefaultValueAttribute(""),
        PropertyGirdBaseObject.showText("分组ID")
       ]
        public string GroupID
        {
            get;
            set;
        }


        /// <summary>
        /// 上级分组ID
        /// </summary>
        [
        CategoryAttribute("全局设置"),
        ReadOnlyAttribute(true),
        DescriptionAttribute("上级分组ID"),
        DefaultValueAttribute(""),
        PropertyGirdBaseObject.showText("上级分组ID")
       ]
        public string  SuperiorID
        {
            get;
            set;
        }
         
        /// <summary>
        /// 分组Tag
        /// </summary>
        public object Tag = null;

        private string _GroupName;
        /// <summary>
        /// 设置或获取分组名称
        /// </summary>
        [
        CategoryAttribute("全局设置"),
        ReadOnlyAttribute(false ),
        DescriptionAttribute("设置或获取分组名称"),
        DefaultValueAttribute(""),
        PropertyGirdBaseObject.showText("分组名")
       ]
        public string GroupName
        {
            set
            {
                _GroupName = value;
                if (this.GroupNameChanged != null) this.GroupNameChanged(this);//触发部门名称改变事件 
            }
            get { return _GroupName; }
        }

        /// <summary>
        /// 设置或获取分组名称
        /// </summary>
        [
        CategoryAttribute("全局设置"),
        ReadOnlyAttribute(false),
        DescriptionAttribute("设置或获取排序"),
        DefaultValueAttribute(""),
        PropertyGirdBaseObject.showText("排序")
       ]
        public int OrderID { get; set; } 

        /// <summary>
        /// 初始化分组信息类
        /// </summary>
        /// <param name="GroupID">分组ID</param>
        /// <param name="GroupName">分组名</param>
        /// <param name="SuperiorID">上级分组ID</param>
        /// <param name="OrderID">分组排序ID</param>
        public Group(string GroupID, string GroupName, string SuperiorID, int OrderID)
        {
            this.GroupID = GroupID;
            this.GroupName = GroupName;
            this.SuperiorID = SuperiorID;
            this.OrderID = OrderID;
        }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool Enabled = false;

    }
    #endregion

}
