using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IMLibrary3.Net
{
    #region  UDP数据分包类
    /// <summary>
    /// UDP数据分包类
    /// </summary>
      class SubPackage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public SubPackage()
        {

        }

        private DateTime _StartTime = DateTime.Now;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            set { _StartTime = value; }
            get { return _StartTime; }
        }

        private bool _IsArrival = false;
        /// <summary>
        /// 是否等待，直到包发送成功
        /// </summary>
        public bool IsArrival
        {
            set { _IsArrival = value; }
            get { return _IsArrival; }
        }


        private IPEndPoint _RemoteIPEndPoint;
        /// <summary>
        /// 远程主机端点
        /// </summary>
        public IPEndPoint RemoteIPEndPoint
        {
            set { _RemoteIPEndPoint = value; }
            get { return _RemoteIPEndPoint; }
        }

        /// <summary>
        /// 字节消息类
        /// </summary>
        public SubPackageBytes Msg
        {
            set;
            get;
        }
    }
    #endregion
}
