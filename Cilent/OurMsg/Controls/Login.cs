using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OurMsg.Controls
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CancleEventHandler(object sender, EventArgs e);

        /// <summary>
        /// 取消事件
        /// </summary>
        public event CancleEventHandler CancelLogin;

        private void Loging_Load(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CancelLogin != null)
                CancelLogin(this, null);
        }

        /// <summary>
        /// 停止效果
        /// </summary>
        public void Stop()
        {
            this.clockWating1.Stop();
        }

        /// <summary>
        /// 开始效果
        /// </summary>
        public void Start()
        {
            this.clockWating1.Start();
        }
    }
}
