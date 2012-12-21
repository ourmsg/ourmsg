using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OurMsg.Controls
{
    public partial class UserLoginPanel : UserControl
    {
        public UserLoginPanel()
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

        private void UserLoginPanel_Resize(object sender, EventArgs e)
        {
            this.loging1.Left = (this.Width - loging1.Width) / 2;
            this.loging1.Top = (this.Height - loging1.Height) / 3;
        }

        private void loging1_CancelLogin(object sender, EventArgs e)
        {
            if (CancelLogin != null)
                CancelLogin(this, null);
        }

        /// <summary>
        /// 停止效果
        /// </summary>
        public void Stop()
        {
            this.loging1.Stop();
        }

        /// <summary>
        /// 开始效果
        /// </summary>
        public void Start()
        {
            this.loging1.Start();
        }
    }
}
