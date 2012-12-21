using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 认证类
    /// </summary>
    public class Auth : Element  
    {
      
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { set; get; }
       

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
      
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Resource { set; get; }

        /// <summary>
        /// TCP文件服务端口
        /// </summary>
        public int  FileServerTCPPort { set; get; }

        /// <summary>
        /// UDP文件服务端口
        /// </summary>
        public int FileServerUDPPort { set; get; }

        /// <summary>
        /// UDP音视频服务端口
        /// </summary>
        public int AVServerUDPPort { set; get; }

        /// <summary>
        /// 显示类型
        /// </summary>
        public IMLibrary3.Enmu.ShowType ShowType
        {
            set;
            get;
        }

        /// <summary>
        /// 在线显示信息
        /// </summary>
        public string Status
        {
            set;
            get;
        }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastIP
        { set; get; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public string LastDateTime
        { set; get; }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool IsAutoLogin { set; get; }
        /// <summary>
        /// 是否保存密码
        /// </summary>
        public bool IsSavePassword { set; get; }

     

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

    }


}
