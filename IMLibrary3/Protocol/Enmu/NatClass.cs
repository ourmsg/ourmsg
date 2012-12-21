using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Enmu
{

    /// <summary>
    /// 客户端Nat类型
    /// </summary>
    public enum NatClass
    {
        /// <summary>
        /// UDP未知NAT
        /// </summary>
        UdpUnkonw = 0,
        /// <summary>
        /// 全双工UDP NAT
        /// </summary>
        FullCone = 1,
        /// <summary>
        /// 注册锥形NAT
        /// </summary>
        RestrictedCone = 2,
        /// <summary>
        /// 端口注册锥形NAT
        /// </summary>
        PortRestrictedCone = 3,
        /// <summary>
        /// 对称NAT
        /// </summary>
        Symmetic = 4,
        /// <summary>
        /// TCP联连
        /// </summary>
        Tcp = 5,
        /// <summary>
        /// 无任何联接
        /// </summary>
        None = 6,
    }
}
