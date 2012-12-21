using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 更改群信息
    /// </summary>
    public class ChangeRoom : Element
    {
      

        /// <summary>
        /// 群组ID
        /// </summary>
        public string RoomID
        {
            set;
            get;
        }

     
        /// <summary>
        /// 设置或获取群组名称
        /// </summary>
        public string RoomName
        {
            set;
            get;
        }
         
        /// <summary>
        /// 设置或获取群组所在的OrderID。 
        /// </summary>
        public int OrderID
        {
            set;
            get;
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
        /// 删除的用户,用分号隔开
        /// </summary>
        public string DelUsers
        {
            get;
            set;
        }

    }
}
