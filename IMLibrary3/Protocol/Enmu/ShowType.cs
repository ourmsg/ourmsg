using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Enmu
{
    #region 用户在线状态枚举
    /// <summary>
    /// 用户在线状态枚举
    /// </summary>
    public enum ShowType
    {
        /// <summary>
        /// 空闲
        /// </summary>
        NONE=-1,
        /// <summary>
        /// 离开
        /// </summary>
        away = 0,
        /// <summary>
        /// 对话
        /// </summary>
        chat=1,
        /// <summary>
        /// 请匆打扰
        /// </summary>
        dnd = 2,
        /// <summary>
        /// 走远
        /// </summary>
        xa = 3,
        /// <summary>
        /// 隐身
        /// </summary>
        Invisible=100,
        /// <summary>
        /// 离线
        /// </summary>
        Offline=101,
        /// <summary>
        /// 其他
        /// </summary>
        Else=102,
    }
    #endregion


    /// <summary>
    /// 用户上网方式
    /// </summary>
    public enum OnlineWay
    {
        /// <summary>
        /// 电脑
        /// </summary>
        Computer,
        /// <summary>
        /// WEB方式上网
        /// </summary>
        Web,
        /// <summary>
        /// 移动上网
        /// </summary>
        Mobile,
    }
}
