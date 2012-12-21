using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IMLibrary3.Organization
{
    #region 用户信息类
    /// <summary>
    /// 用户信息类 
    /// </summary>
    public class User : PropertyGirdBaseObject.BaseObject
    {
        /// <summary>
        /// 用户信息类
        /// </summary>
        public User()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 用户信息类
        /// </summary>
        /// <param name="UserID">用户ID</param>
        public User(string UserID)
        {
            this.UserID = UserID;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        public User(string userID, string userName)
        {
            this.UserID = userID;
            this.UserName = userName;
        }

        /// <summary>
        /// 初始化用户信息类
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="UserName">用户姓名</param>
        /// <param name="GroupID">用户所在分组ID</param>
        /// <param name="OrderID">用户在分组内的排序</param>
        public User(string UserID, string UserName, string GroupID, int OrderID)
        {
            this.UserID = UserID;
            this.UserName  = UserName;
            this.GroupID  = GroupID;
            this.OrderID = OrderID;
        }

        #region 事件
        /// <summary>
        /// 用户在线状态改变事件
        /// </summary>
        /// <param name="sender">产生事件的对象</param>
        public delegate void ChangedEventHandler(object sender);
        /// <summary>
        /// 
        /// </summary>
        public event ChangedEventHandler UserOfflinedOrOnlined;
   
        /// <summary>
        /// 用户在线状态改变事件
        /// </summary>
        public event ChangedEventHandler UserShowTypeChanged;
 
        /// <summary>
        /// 用户姓名更新事件
        /// </summary>
        public event ChangedEventHandler UserNameChanged;

    
        /// <summary>
        /// 用户ID更新事件
        /// </summary>
        public event ChangedEventHandler UserIDChanged;

        /// <summary>
        /// 用户部门更新事件
        /// </summary>
        public event ChangedEventHandler GroupIDChanged;
        #endregion

        /// <summary>
        /// 设置或获取发送的消息是否成功。 
        /// </summary>
        //public bool IsSendSuccess = false;//标识发送给此联系人的上一次数据是否成功
  
        /// <summary>
        /// 用户Tag
        /// </summary>
        public object Tag = null;

        private string _UserID = "";// 标识用户身分唯一的ID（暂用计算机名代替）
        /// <summary>
        /// 系统用户ID。 
        /// </summary>
        public string UserID
        {
            get { return _UserID; }
            set
            {
                _UserID = value;
                if (UserIDChanged != null)
                    UserIDChanged(this);
            }
        }

        private string _UserName = "";
        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                if (UserNameChanged != null)
                    UserNameChanged(this);
            }
        }

        private string _GroupID = "";//用户所在部门的ID
        /// <summary>
        /// 设置或获取用户所在部门的ID。 
        /// </summary>
        public string GroupID
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                if (GroupIDChanged != null)
                    GroupIDChanged(this);
            }
        }

        private int _OrderID = 0;//用户所在部门的OrderID
        /// <summary>
        /// 设置或获取用户所在部门的ID。 
        /// </summary>
        public int OrderID
        {
            get { return _OrderID; }
            set
            {
                _OrderID = value;
            }
        }

        private IMLibrary3.Enmu.ShowType _ShowType = Enmu.ShowType.Offline;
        /// <summary>
        /// 获取或设置用户的在线状态。 
        /// </summary>
        public Enmu.ShowType ShowType
        {
            get { return _ShowType; }
            set
            {
                switch (value)
                {

                    case Enmu.ShowType.Offline:
                        Status = "(脱机)";
                        break;
                    case Enmu.ShowType.Invisible:
                        Status = "(脱机)";
                        break;
                    case Enmu.ShowType.NONE:
                        Status = "(联机)";
                        break;
                    case Enmu.ShowType.away:
                        Status = "(离开)";
                        break;
                    case Enmu.ShowType.chat:
                        Status = "(接听电话)";
                        break;
                    case Enmu.ShowType.dnd:
                        Status = "(请匆打扰)";
                        break;
                    case Enmu.ShowType.xa:
                        Status = "(忙碌)";
                        break;
                }

                if (_ShowType != value)//如果状态改变，则触发事件
                {
                    if ((_ShowType >= IMLibrary3.Enmu.ShowType.Invisible && value < IMLibrary3.Enmu.ShowType.Invisible)
                        || (_ShowType< IMLibrary3.Enmu.ShowType.Invisible && value >=IMLibrary3.Enmu.ShowType.Invisible))
                        if (UserOfflinedOrOnlined != null)
                            UserOfflinedOrOnlined(this);

                    _ShowType = value;
                    if (UserShowTypeChanged != null)
                        UserShowTypeChanged(this);
                }
            }
        }

        /// <summary>
        ///  用户密码
        /// </summary>
        public string Password = null;
  
        /// <summary>
        /// 在线状态描述
        /// </summary>
        public string Status = "(脱机)";
    }
    #endregion
}
