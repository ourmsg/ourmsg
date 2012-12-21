using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Organization
{
    #region 群组
    /// <summary>
    ///  群组
    /// </summary>
    public class Room  
    {
        /// <summary>
        /// 组织机构分组信息类
        /// </summary>
        public Room()
        {
        }

        /// <summary>
        /// 分组信息名称改变后事件代理
        /// </summary>
        /// <param name="sender"></param>
        public delegate void RoomNameChangedEventHandler(object sender);

        /// <summary>
        /// 分组信息名称改变后事件 
        /// </summary>
        public event RoomNameChangedEventHandler RoomNameChanged;


        /// <summary>
        /// 群组ID
        /// </summary>
        public string RoomID
        {
            set;
            get;
        }
  
        /// <summary>
        ///  组Tag
        /// </summary>
        public object Tag = null;
        
        /// <summary>
        /// 群组包含的用户
        /// </summary>
        public Dictionary<string ,User> Users = new Dictionary<string,User>();

        private string _RoomName="";
        /// <summary>
        /// 设置或获取群组名称
        /// </summary>
        public string RoomName
        {
            set
            {
                _RoomName = value;
                if (this.RoomNameChanged != null) this.RoomNameChanged(this);//触发部门名称改变事件 
            }
            get { return _RoomName; }
        }

        private int _OrderID = 0;//用户所在群组的OrderID
        /// <summary>
        /// 设置或获取群组所在的OrderID。 
        /// </summary>
        public int OrderID
        {
            get { return _OrderID; }
            set
            {
                _OrderID = value;
            }
        }

        /// <summary>
        /// 群组通知
        /// </summary>
        public string Notice
        {
            get;
            set;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreateUserID
        {
            get;
            set;
        }
        /// <summary>
        /// 包含的用户,用分号隔开
        /// </summary>
        public string UserIDs
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化分组信息类
        /// </summary>
        /// <param name="RoomID">分组ID</param>
        /// <param name="RoomName">分组名</param>
        /// <param name="OrderID">分组排序ID</param>
        public Room(string RoomID, string RoomName, int OrderID)
        {
            this.RoomID = RoomID;
            this.RoomName = RoomName;
            this.OrderID = OrderID;
        }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool Enabled = false;
    }
    #endregion
}
