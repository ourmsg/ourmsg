using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IMLibrary3.Controls;

namespace OurMsg
{
    public partial class FormLogin : Form   
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 是否登录
        /// </summary>
        private bool isLogin = false;
        /// <summary>
        /// 是否退出程序
        /// </summary>
        public bool  isExit=false;

        /// <summary>
        /// 是否因为密码错误或服务超时产生的重复登录
        /// </summary>
        public bool IsRepeat = false;

        /// <summary>
        /// 登录参数
        /// </summary>
        public IMLibrary3.Protocol.Auth  auth = new  IMLibrary3.Protocol.Auth();

        #endregion

        private void FormLogin_Load(object sender, EventArgs e)
        {
            //if (Global.RunningInstance() == null && !IsRepeat)//如果是第一次登录或程序第一次运行
            //    this.webUpdate1.DownLoad();//检测有无升级版程序

            this.cmbBoxUserID.DisplayMember = "userID";

            List<IMLibrary3.Protocol.Auth> auths =IMLibrary3.OpeRecordDB.GetLoginAuths();//获得所有登录信息
            if(auths!=null )
                foreach (IMLibrary3.Protocol.Auth a in auths )
                    this.cmbBoxUserID.Items.Add(a);
             
            if (!this.IsRepeat && this.cmbBoxUserID.Items.Count > 0) 
                this.cmbBoxUserID.SelectedIndex = 0;//选择第一项
         }

        private void butLogin_Click(object sender, EventArgs e)
        {
            this.auth.UserID = cmbBoxUserID.Text.Trim();
            this.auth.Password = txtPassword.Text;

            if (auth.ShowType == IMLibrary3.Enmu.ShowType.Offline) 
                auth.ShowType =IMLibrary3.Enmu.ShowType.NONE;

            if (this.auth.UserID == "" || this.auth.Password == "")
            {
                IMLibrary3.Global.MsgShow("必须输入帐号与密码才能登录！");
                return;
            }
            isLogin = true;//正常登录

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            isExit = true;
            this.Close();
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!isLogin)//表示用户退出程序
                isExit = true;
        }

        private void checkBoxSavePassword_CheckedChanged(object sender, EventArgs e)
        {
            this.auth.IsSavePassword = checkBoxSavePassword.Checked;
               }

        private void checkBoxAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoLogin.Checked)//如果自动登录，必须保存密码
                checkBoxSavePassword.Checked = checkBoxAutoLogin.Checked;

            this.auth.IsAutoLogin = checkBoxAutoLogin.Checked;
        }

        private void ShowType1_ShowTypeChanged(object sender, ShowType.ShowTypeEventArgs e)
        {
            this.auth.ShowType = e.ShowType; 
        }

        private void cmbBoxUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            IMLibrary3.Protocol.Auth a = cmbBoxUserID.SelectedItem as IMLibrary3.Protocol.Auth;
            if (a != null)
            {
                this.txtPassword.Text = a.Password;
                this.checkBoxAutoLogin.Checked = a.IsAutoLogin;
                this.checkBoxSavePassword.Checked = a.IsSavePassword;
                this.ShowType1.State =  a.ShowType ;
                this.auth.ShowType = a.ShowType; 

                if (this.checkBoxSavePassword.Checked && this.checkBoxAutoLogin.Checked && !IsRepeat)
                    butLogin_Click(null, null);//自动登录
            } 
        }

        /// <summary>
        /// 设置登录参数
        /// </summary>
        public void SetLoginParameter(IMLibrary3.Protocol.Auth auth)
        {
            this.auth = auth;
            cmbBoxUserID.Text = auth.UserID;
            txtPassword.Text = auth.Password;
            ShowType1.State = auth.ShowType;
            checkBoxSavePassword.Checked = auth.IsSavePassword;
            checkBoxAutoLogin.Checked = auth.IsAutoLogin;
        }

        private void ShowType1_ShowTypeExitApp(object sender,  ShowType.ShowTypeEventArgs e)
        {
            butCancel_Click(null, null);
        }

    }
}
