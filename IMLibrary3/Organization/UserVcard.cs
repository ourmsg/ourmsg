using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Organization
{
    /// <summary>
    /// UserVcard
    /// </summary>
    public class UserVcard : User 
    {
        
        /// <summary>
        /// 协议类型
        /// </summary>
        public IMLibrary3.Protocol.type type { get; set; }

        /// <summary>
        /// 用户加入的群
        /// </summary>
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        /// <summary>
        /// 已创建群数量
        /// </summary>
        public int CreatedRoomsCount = 0;

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public string LastDateTime = null;
        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastIP = null;
        /// <summary>
        /// 在线时长
        /// </summary>
        public int onlineDateLength = 0;


        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disable { set; get; }
        /// <summary>
        /// 是否可以发送短信
        /// </summary>
        public bool isSendSMS { set; get; }
        /// <summary>
        /// 是否可以编辑用户数据
        /// </summary>
        public bool isEditUserData { set; get; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool isAdmin { set; get; }
        /// <summary>
        /// 是否可以发送消息
        /// </summary>
        public bool isSendNotice { set; get; }
        /// <summary>
        /// 是否可以广播
        /// </summary>
        public bool isBroadcast { set; get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { set; get; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Job { set; get; }
        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficePhone { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Post { set; get; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex { set; get; }
        /// <summary>
        /// 最大创建群数
        /// </summary>
        public int CreateRooms { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        ///  用户头像索引位置
        /// </summary>
        public int FaceIndex{ set; get; }


        /// <summary>
        /// 群版本
        /// </summary>
        public string RoomsVersion = "";

        /// <summary>
        /// 群总数
        /// </summary>
        public int RoomsCount = 0;
    }
}
