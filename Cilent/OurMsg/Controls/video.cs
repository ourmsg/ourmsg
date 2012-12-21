using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using IMLibrary3;

namespace OurMsg.Controls
{
    public partial class video : UserControl
    {
        public video()
        {
            InitializeComponent();
            butReceive.Click += new EventHandler(butReceive_Click);
            butHangup.Click += new EventHandler(butHangup_Click);
        }

        /// <summary>
        /// 通信组对像
        /// </summary>
        private IMLibrary.AV.Controls.AVComponent AVC = null;

        #region 事件
        /// <summary>
        /// 获得IPEndPoint事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="local">本地主机信息</param>
        /// <param name="remote">远程主机信息</param>
        public delegate void GetIPEndPointEventHandler(object sender, IPEndPoint local, IPEndPoint remote);
        public event GetIPEndPointEventHandler GetIPEndPoint;
        protected virtual void OnGetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            if (GetIPEndPoint != null)
                GetIPEndPoint(this, local, remote);//触发获取本机主机事件
        } 

        public delegate void CancelEventHandler(object sender);
        /// <summary>
        /// 取消事件
        /// </summary>
        public event CancelEventHandler Cancel;
        #endregion

        #region 属性
        private bool _IsInvite = false;
        /// <summary>
        /// 是否邀请方（请求方）
        /// </summary>
        public bool IsInvite
        {
            set
            {
                _IsInvite = value;
                butReceive. Visible = !value;
            }
            get { return _IsInvite; }
        }
        #endregion

        #region 初始化本地音视频
        private void IniLocalVideo(System.Net.IPEndPoint ServerEP)
        {
            if (AVC == null)
            {
                AVC = new IMLibrary.AV.Controls.AVComponent();
                AVC.GetIPEndPoint += new IMLibrary.AV.Controls.AVComponent.GetIPEndPointEventHandler(AVC_GetIPEndPoint);
                AVC.TransmitConnected += new IMLibrary.AV.Controls.AVComponent.TransmitEventHandler(AVC_TransmitConnected);
                AVC.SetControls(panelLocalAV, panelRemotAV);// 绑定音视频控件
                AVC.iniAV(IMLibrary.AV.VideoSizeModel.W320_H240, ServerEP);
            }
        }

      
        void AVC_TransmitConnected(object sender, ConnectedType connectedType)
        {
            Console.WriteLine("已经联接：" + connectedType.ToString());
        }

        void AVC_GetIPEndPoint(object sender, System.Net.IPEndPoint local, System.Net.IPEndPoint remote)
        {
            OnGetIPEndPoint(this, local, remote);
        }
        #endregion

        #region 设置对方视频位图信息，构造解码器
        /// <summary>
        /// 设置对方视频位图信息，构造解码器
        /// </summary>
        /// <param name="bitmapinfo"></param>
        public void SetVideoBitmapinfo(IMLibrary.AV.BITMAPINFO bitmapinfo)
        {
            if (AVC != null)
                AVC.SetRemoteBITMAPINFOHEADER(bitmapinfo);
        }
        #endregion

        #region 设置对方远程主机信息
        /// <summary>
        /// 设置对方远程主机信息
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        public void SetRometEP(System.Net.IPEndPoint local, System.Net.IPEndPoint remote)
        {
            IniLocalVideo(Global.AVTransmitServerEP);//设置服务器主机信息并开始服务
            AVC.frameTransmit.setRemoteIP(local, remote);//设置AV通信组件远程主机信息
        }
        #endregion

        #region 取消视频
        void butHangup_Click(object sender, EventArgs e)
        {
            CancelAV();// 取消视频
            if (Cancel != null)
                Cancel(this);
        }
         
        /// <summary>
        /// 取消视频
        /// </summary>
        public void CancelAV()
        {
            if (AVC != null)
            {
                AVC.Close();
                AVC.Dispose();
                AVC = null;
            }
        }
        #endregion

        #region 开始视频
        void butReceive_Click(object sender, EventArgs e)
        {
            butReceive.Visible = false;
            IniLocalVideo(Global.AVTransmitServerEP);
        }

        #endregion

    }
}
