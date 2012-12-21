using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;


namespace IMLibrary3.Protocol
{
    /// <summary>
    /// Element节
    /// </summary>
    public class Element
    {
         
       
        /// <summary>
        /// 消息发送帐号
        /// </summary>
        public string from
        {
            set;
            get;
        }


        /// <summary>
        /// 消息接收者
        /// </summary>
        public string to
        {
            set;
            get;
        }

        /// <summary>
        /// 协议类型
        /// </summary>
        public type type
        {
            get;
            set;
        }

        /// <summary>
        /// 服务id
        /// </summary>
        public string id
        {
            get;
            set;
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string version
        {
            set;
            get;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public List<Object> Data = new List<object>();
    }
}
