using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 企业组织架构版本
    /// </summary>
    public class OrgVersion : Element 
    {
     

        /// <summary>
        /// 分组版本
        /// </summary>
        public string GroupsVersion
        { set; get; }
        
        /// <summary>
        /// 分组总数
        /// </summary>
        public int GroupsCount
        { set; get; }

        /// <summary>
        /// 用户版本
        /// </summary>
        public string UsersVersion
        { set; get; }

        /// <summary>
        /// 用户总数
        /// </summary>
        public int UsersCount
        { set; get; }

        /// <summary>
        /// 群版本
        /// </summary>
        public string RoomsVersion
        { set; get; }

        /// <summary>
        /// 群总数
        /// </summary>
        public int RoomsCount
        { set; get; }

        /// <summary>
        /// 分组信息XML字符串
        /// </summary>
        public List<string> GroupsXML = new List<string>();

        /// <summary>
        /// 用户信息XML字符串
        /// </summary>
        public List<string> UsersXML = new List<string>();


     
    }
}
