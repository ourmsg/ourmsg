using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Enmu
{
    /// <summary>
    /// 对话类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 用户消息
        /// </summary>
        User = 0,
        /// <summary>
        /// 群组消息
        /// </summary>
        Group,
        /// <summary>
        /// 通知消息
        /// </summary>
        Notice,
        /// <summary>
        /// 广播消息
        /// </summary>
        broadcasting,
        /// <summary>
        /// 其他消息
        /// </summary>
        Else,
    }

}
