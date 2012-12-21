using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 出席（在线状态）
    /// </summary>
    public class Presence : Element 
    {
  
        /// <summary>
        /// 显示类型
        /// </summary>
        public IMLibrary3.Enmu.ShowType ShowType
        {
            set;
            get;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Status
        {
            set;
            get;
        }
    }
}
