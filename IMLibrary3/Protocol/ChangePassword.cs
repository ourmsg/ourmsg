using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 更改密码
    /// </summary>
    public class ChangePassword : Element
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { set; get; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { set; get; }

    }
}
