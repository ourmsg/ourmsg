using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace OurMsg
{
    public partial class FormMain :Form 
    {
        #region  重写WndProc(用于捕获Windows消息)
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //   一旦程序收到WM_SHUTDOWN消息， 
            //   将systemShutdown布尔值设置为true。 
            if (m.Msg == WM_SHUTDOWN)
                systemShutdown = true;
            base.WndProc(ref   m);
        }
        #endregion

        #region 窗口初始化
        public FormMain()
        {
            InitializeComponent();

            #region 控制面板事件
            this.controlPanel1.UserLoginOutTime += new OurMsg.Controls.ControlPanel.ControlPanelEventHandler(controlPanel1_UserLoginOutTime);
            this.controlPanel1.UserLoginPasswordErrored += new OurMsg.Controls.ControlPanel.ControlPanelEventHandler(this.controlPanel1_UserLoginPasswordErrored);
            this.controlPanel1.UserLoginSuccessful += new OurMsg.Controls.ControlPanel.LoginEventHandler(controlPanel1_UserLoginSuccessful);
            this.controlPanel1.UserOffline += new OurMsg.Controls.ControlPanel.ControlPanelEventHandler(controlPanel1_UserOffline);
            this.controlPanel1.UserElseLogin += new OurMsg.Controls.ControlPanel.ControlPanelEventHandler(controlPanel1_UserElseLogin);
            #endregion

            this.Load += new EventHandler(FormMain_Load);
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);
            this.tButManageMsg.Click += new EventHandler(tButManageMsg_Click);
            this.tsmMsgMis.Click += new EventHandler(tButManageMsg_Click);
        }
         
        private void tButManageMsg_Click(object sender, EventArgs e)
        {
            if (Global.FormDataManageage == null || Global.FormDataManageage.IsDisposed)
            {
                Global.FormDataManageage = new FormDataManage();
                Global.FormDataManageage.myUserID = auth.UserID;
                Global.FormDataManageage.FormClosed += delegate(object sender1, FormClosedEventArgs e1)
                {
                    Global.FormDataManageage.Dispose(); Global.FormDataManageage = null;
                };
            }
            Global.FormDataManageage.Show();
        }
        #endregion

        #region 变量
        /// <summary>
        /// 关闭电脑时的Windows   Message的值
        /// </summary>
        private static int WM_SHUTDOWN = 0x11;

        /// <summary>
        /// 布尔值判断是否是关闭电脑 
        /// </summary>
        private static bool systemShutdown = false;

        /// <summary>
        /// 登录参数
        /// </summary>
        public IMLibrary3.Protocol.Auth auth = null;

        /// <summary>
        /// 标识当前用户是否成功登录过系统
        /// </summary>
        private bool IsLogined = false;//标识当前用户是否成功登录过系统

        private FormLogin frmLogin = null;//登录窗口
        #endregion

        #region 窗口相关事件 

        #region 加载窗口
        private void FormMain_Load(object sender, EventArgs e)
        {
            setFormPoinstion();
            FormMain_Resize(null, null);
             
            if (Global.RunningInstance() == null)//如果程序线程已经运行了
                ReadyLogin(false);// 准备登录,系统中没有OurMsg程序在运行
            else
                ReadyLogin(true);// 准备登录，系统中至少有一个OurMsg程序在运行
        }
        #endregion

        #region 设置窗口位置
        /// <summary>
        /// 设置窗口位置
        /// </summary>
        private void setFormPoinstion()
        {
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
        }
        #endregion

        #region 准备登录
        /// <summary>
        /// 准备登录
        /// </summary>
        private void ReadyLogin(bool IsRepeat)
        {
            try
            {
                this.Hide();
                if (frmLogin == null || frmLogin.IsDisposed)
                    frmLogin = new FormLogin();

                frmLogin.IsRepeat = IsRepeat;

                if (IsRepeat && this.auth != null)//如果是重复登录，则更改登录参数
                    frmLogin.SetLoginParameter(this.auth);

                frmLogin.ShowDialog();
                if (frmLogin.isExit)//如果退出应用程序
                {
                    this.ExitApp();//退出应用程序
                }
                else//如果不是退出应用程序，则登录
                {
                    if (!this.userLoginPanel1.IsDisposed)//如果登录进度显示组件未释放
                        this.userLoginPanel1.Start();//则显示登录进度

                    this.Show();//显示主窗口
                    this.auth = frmLogin.auth;//将当前登录参数设置为最新的
                    this.controlPanel1.Login(auth, false);//登录
                }
                frmLogin = null;
            }
            catch (Exception ex)
            {
                IMLibrary3.Global.MsgShow(ex.Message + ex.Source);
            }
        }
        #endregion

        #region 窗口关闭事件
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!systemShutdown)//如果不是关闭windows
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else//如果关闭windows，则退出程序
            {
                this.notifyIcon1.Dispose();//删除小图标
                this.controlPanel1.Close(true);//退出程序
                Application.Exit();
            }
        }
        #endregion
         
        #region 应用退出程序函数 ExitApp()
        /// <summary>
        /// 应用退出程序函数
        /// </summary>
        private void ExitApp()
        {
            systemShutdown = true ;//强行指定为windows关闭
            this.Close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();//强制杀进程结束程序
            try
            { Application.Exit(); }
            catch { }
        }
        #endregion

        #endregion

        #region 在线状态控件选择后事件
        private void ShowType1_ShowTypeChanged(object sender, IMLibrary3.Controls.ShowType.ShowTypeEventArgs e)
        {
            this.controlPanel1.setMyPresence(e.ShowType);
        }

        private void ShowType1_ShowTypeExitApp(object sender, IMLibrary3.Controls .ShowType.ShowTypeEventArgs e)
        {
            this.ExitApp();
        }
        #endregion

        #region 窗口尺寸更改事件
        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (!this.userLoginPanel1.IsDisposed)
            {
                this.userLoginPanel1.Width = this.Width;
                this.userLoginPanel1.Height = this.Height;
            }
        }
        #endregion

        #region 取消登录事件
        private void userLoginPanel1_CancelLogin(object sender, EventArgs e)
        {
            ReadyLogin(true);// 准备登录
        }
        #endregion

        #region 用户别处登录
        private void controlPanel1_UserElseLogin(object sender )
        {
            this.ShowType1.State = IMLibrary3.Enmu.ShowType.Offline;
            showNotifyIconMessage("用户在别处登录，系统已强迫您下线！", "OurMsg掉线", 3000);
        }
        #endregion

        #region 登录密码错误处理
        private void controlPanel1_UserLoginPasswordErrored(object sender )
        {
           if (!this.userLoginPanel1.IsDisposed)
                this.userLoginPanel1.Stop();

           IMLibrary3.Global.MsgShow("密码错误！");
            ReadyLogin(true);// 准备登录
        }
        #endregion

        #region 登录超时处理
        private void controlPanel1_UserLoginOutTime(object sender )
        {
            if (!this.userLoginPanel1.IsDisposed)
                this.userLoginPanel1.Stop();


            if (!IsLogined)//如果没有成功登录过系统
            {
                IMLibrary3.Global.MsgShow("连接服务器超时。\n请重试！");
                ReadyLogin(true);// 准备登录
            }
            else
            {
                this.showNotifyIconMessage("连接服务器超时。\n请重试！", "OurMsg 提示", 5000);
            }
        }
        #endregion

        #region 登录成功处理

        private void controlPanel1_UserLoginSuccessful(object sender, IMLibrary3.Protocol.Auth auth)
        {
            this.showNotifyIconMessage("上次登录IP：" + (auth.LastIP == null ? "" : auth.LastIP) + " \n上次登录时间：" + (auth.LastDateTime == null ? "" : auth.LastDateTime), "OurMsg 登录成功", 5000);
            this.notifyIcon1.Text ="OurMsg:"+ auth.UserName + "(" + auth.UserID + ")";
            this.labUserName.Text = auth.UserName + "(" + auth.UserID + ")";
            ShowType1.State = auth.ShowType;

            #region 删除登录进度效果显示面板
            this.Controls.Remove(this.userLoginPanel1);
            this.userLoginPanel1.Dispose();
            #endregion

            IsLogined = true;//标识当前用户已经登录过系统
        }
         
        #endregion

        #region 用户离线
        public void controlPanel1_UserOffline(object sender )
        {
            this.ShowType1.State = IMLibrary3.Enmu.ShowType.Offline;
            //showNotifyIconMessage("与服务器失去连接！","OurMsg掉线",3000);
        }
        #endregion

        #region 显示提示信息
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tip">要显示的内容</param>
        /// <param name="title">标题</param>
        /// <param name="timeLen">显示时间，以毫秒为单位</param>
        private void showNotifyIconMessage(string tip, string title, int timeLen)
        {
            this.notifyIcon1.BalloonTipText = tip;
            this.notifyIcon1.BalloonTipTitle = title;
            this.notifyIcon1.ShowBalloonTip(timeLen);
        }
        #endregion

        #region 图标双击
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (frmLogin != null && !frmLogin.IsDisposed )
            {
                frmLogin.Activate();
                return;
            }
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        #endregion

        #region 图标关联菜单事件
        private void contextMenuNotify_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "MenuItemExit":
                    this.ExitApp();//退出程序
                    break;
                case "MenuItemOpenMain":
                    {
                        this.Show();
                        this.WindowState = FormWindowState.Normal;
                    }
                    break;
            }
        }
        #endregion

        #region 个人信息设置
        private void tsmMyInfo_Click(object sender, EventArgs e)
        {
            controlPanel1.ShowUserVcard(auth.UserID);
        } 

        private void selfFaceImage1_ImageClick(object sender, EventArgs e)
        {
            controlPanel1.ShowUserVcard(auth.UserID);
        }
        #endregion

    }
}
