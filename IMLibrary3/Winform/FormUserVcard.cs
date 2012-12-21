using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary3 
{
    public partial class FormUserVcard : Form
    {
        public FormUserVcard()
        {
            InitializeComponent();
            comboBoxSex.SelectedIndex = 0; 
        }

        #region 事件

        /// <summary>
        /// 创建用户事件代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userVcard"></param>
        public delegate void changeEventHandler(object sender, IMLibrary3.Organization.UserVcard   userVcard);

        /// <summary>
        /// 创建用户事件
        /// </summary>
        public event changeEventHandler Create;

        /// <summary>
        /// 刷新用户资料事件
        /// </summary>
        public event changeEventHandler RefreshVcard;

    
        /// <summary>
        /// 更新用户信息事件
        /// </summary>
        public event changeEventHandler UpdateVcard;


        public delegate void changePasswordEventHandler(object sender, IMLibrary3.Protocol.ChangePassword e);

        /// <summary>
        /// 密码修改事件
        /// </summary>
        public event changePasswordEventHandler ChanagePassword;

        #endregion

        #region 属性
        /// <summary>
        /// 当前用户
        /// </summary>
        public string myUserID = "";

        /// <summary>
        /// 更新群组信息超时记数器
        /// </summary>
        private int outTime = 0;

        private bool _isAdmin = false;
        public bool isAdmin
        {
            set
            {
                _isAdmin = value;
                butRefresh.Visible = !value;
                checkBox1.Enabled = value;
            }
            get { return _isAdmin; }
        }

        private bool _IsCreate = false;
        /// <summary>
        /// 是否创建，否为更新
        /// </summary>
        public bool IsCreate
        {
            set
            {
                _IsCreate = value;
                if (value)
                {
                    this.butOK.Visible = false;
                    this.butCreate.Visible = true;
                }
                else
                {
                    this.butOK.Visible = true;
                    this.butCreate.Visible = false;
                    this.textBoxUserID.ReadOnly = true;

                    if (!isAdmin)//如果不是管理员，则只能查看用户信息
                    {
                        this.butSelectGroupID.Visible = false;

                        this.textBoxUserName.ReadOnly = true;
                        this.textBoxGroupID.ReadOnly = true;
                        this.numericUpDownOrder.ReadOnly = true;
                        this.textBoxMail.ReadOnly = true;
                        this.textBoxJob.ReadOnly = true;
                        this.textBoxOfficePhone.ReadOnly = true;
                        this.textBoxPhone.ReadOnly = true;
                        this.textBoxPost.ReadOnly = true;
                        this.textBoxRemark.ReadOnly = true;
                        this.dateTimePickerBirthday.Enabled = false;
                        this.comboBoxSex.Enabled = false;
                        this.numericUpDownCreateRooms.ReadOnly = true;
                        this.checkBoxDisable.Enabled = false;
                        this.checkBoxIsAdmin.Enabled = false;
                        this.checkBoxIsSendNotice.Enabled = false;
                        this.checkBoxIsSendSMS.Enabled = false;
                    }
                }
            }
            get { return _IsCreate; }
        }

        /// <summary>
        /// 创建或更新群组信息是否成功
        /// </summary>
        public bool isUpdateSuccess = false;

        private IMLibrary3.Organization.UserVcard _UserVcard;
        public IMLibrary3.Organization.UserVcard UserVcard
        {
            set
            { 
                _UserVcard = value;
                this.textBoxUserID.Text = _UserVcard.UserID;
                this.textBoxUserName.Text = _UserVcard.UserName;
                this.textBoxGroupID.Text = _UserVcard.GroupID;
                this.numericUpDownOrder.Value = _UserVcard.OrderID;
                this.textBoxMail.Text = _UserVcard.Mail;
                this.textBoxJob.Text = _UserVcard.Job;
                this.textBoxOfficePhone.Text = _UserVcard.OfficePhone;
                this.textBoxPhone.Text = _UserVcard.Phone;
                this.textBoxPost.Text = _UserVcard.Post;
                this.textBoxRemark.Text = _UserVcard.Remark;
                this.dateTimePickerBirthday.Value = (DateTime.Now);
                this.comboBoxSex.SelectedIndex = _UserVcard.Sex;
                this.numericUpDownCreateRooms.Value = _UserVcard.CreateRooms;
                this.checkBoxDisable.Checked = _UserVcard.Disable;
                this.checkBoxIsAdmin.Checked = _UserVcard.isAdmin;
                this.checkBoxIsSendNotice.Checked = _UserVcard.isSendNotice;
                this.checkBoxIsSendSMS.Checked = _UserVcard.isSendSMS;
            }
            get
            {
                if (_UserVcard == null) _UserVcard = new IMLibrary3.Organization.UserVcard();
                _UserVcard.UserID = this.textBoxUserID.Text.Trim();
                _UserVcard.UserName = this.textBoxUserName.Text.Trim();
                _UserVcard.GroupID = this.textBoxGroupID.Text.Trim();
                _UserVcard.OrderID = (int)this.numericUpDownOrder.Value;
                _UserVcard.Job = this.textBoxJob.Text.Trim();
                _UserVcard.OfficePhone = this.textBoxOfficePhone.Text.Trim();
                _UserVcard.Mail = this.textBoxMail.Text.Trim();
                _UserVcard.Phone = this.textBoxPhone.Text.Trim();
                _UserVcard.Post = this.textBoxPost.Text.Trim();
                _UserVcard.Remark = this.textBoxRemark.Text.Trim();
                _UserVcard.Birthday = this.dateTimePickerBirthday.Value.ToShortDateString();
                _UserVcard.Sex = (byte)this.comboBoxSex.SelectedIndex;
                _UserVcard.CreateRooms = (int)this.numericUpDownCreateRooms.Value;
                _UserVcard.Disable = this.checkBoxDisable.Checked;
                _UserVcard.isAdmin = this.checkBoxIsAdmin.Checked;
                _UserVcard.isSendNotice = this.checkBoxIsSendNotice.Checked;
                _UserVcard.isSendSMS = this.checkBoxIsSendSMS.Checked;
                return _UserVcard;
            }
        }
        #endregion

        private void FormUserVcard_Load(object sender, EventArgs e)
        {

        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                this.Close();//如果不是管理员，关闭窗口
                return;
            }
            if (textBoxUserID.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写用户帐号！"); return;
            }
            if (textBoxUserName.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写用户姓名！"); return;
            }
            if (textBoxGroupID.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请选择用户所在分组！"); return;
            }

            this.butOK.Enabled = false;
            this.isUpdateSuccess = false;
            timer1.Enabled = true;

            if (UpdateVcard != null)
                UpdateVcard(this, UserVcard);
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            if (textBoxUserID.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写用户帐号！"); return;
            }
            if (textBoxUserName.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写用户姓名！"); return;
            }
            if (textBoxGroupID  .Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请选择用户所在分组！"); return;
            }
            IsCreate = true;

            this.butCreate.Enabled = false;
            this.isUpdateSuccess = false;
            timer1.Enabled = true;

            if (Create  != null)
                Create(this, UserVcard);
        }


        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butSelectGroupID_Click(object sender, EventArgs e)
        {
            formSelectGroup frmSelGroup = new formSelectGroup();
            frmSelGroup.LoadData();
            frmSelGroup.selectGroup(this.textBoxGroupID.Text);
            frmSelGroup.ShowDialog();
            if (frmSelGroup != null) 
                this.textBoxGroupID.Text = frmSelGroup.Group.GroupID;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            outTime++;
            if (isUpdateSuccess)
            {
                timer1.Enabled = false;
                if (!IsCreate)
                {
                    IMLibrary3.Global.MsgShow("更新新用户信息成功！");
                    this.Close();
                }
                else
                {
                    IMLibrary3.Global.MsgShow("创建新用户成功！");
                    string groupID = this.textBoxGroupID.Text;
                    UserVcard = new IMLibrary3.Organization.UserVcard();
                    this.textBoxGroupID.Text = groupID;
                    this.butCreate.Enabled = true;
                }
            }
            else if (outTime > 10)
            {
                timer1.Enabled = false;
                IMLibrary3.Global.MsgShow("操作超时！");

                this.butCreate.Enabled = true;
                this.butOK.Enabled = true;

                outTime = 0;
            }
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
            butRefresh.Enabled = false;


            int count = 0;
            Timer t = new Timer();
            t.Interval = 1000;
            t.Enabled = true;
            t.Tick += delegate(object sender1, EventArgs e1)
            {
                count++;
                butRefresh.Text = "更新(" + (5 - count).ToString() + ")";
                if (count == 5)
                {
                    t.Enabled = false;
                    butRefresh.Enabled = true;
                    butRefresh.Text = "更新";
                    t.Dispose();
                    sender1 = null;
                }
            };

            if (RefreshVcard != null)
                RefreshVcard(this, UserVcard);

            //Timer timer = new Timer();
            //timer.Interval = 5000;//5秒超时
            //timer.Enabled = true;
            //timer.Tick += delegate(object sender1, EventArgs e1)
            //{
            //    timer.Enabled = false;
            //    butRefresh.Enabled = true ;
            //    timer.Dispose();
            //    timer = null;
            //};
        }

        private void butChanagePassword_Click(object sender, EventArgs e)
        {
            if (textBoxOldPassword.Text.Trim() == "" && !isAdmin )
            {
                IMLibrary3.Global.MsgShow("请填写旧密码！"); return;
            }
            if (textBoxNewPassword. Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写新密码！"); return;
            }
            if (textBoxConfirmPassword. Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写确认新密码！"); return;
            }
            if (textBoxNewPassword.Text != textBoxConfirmPassword.Text )
            {
                IMLibrary3.Global.MsgShow("新密码两次输入不正确，请重新输入！"); return;
            }

            IMLibrary3.Protocol.ChangePassword cw=new Protocol.ChangePassword();
            cw.from = textBoxUserID.Text;//要更改密码的用户ID
            cw.type = Protocol.type.set;
            cw.NewPassword =textBoxNewPassword.Text;
            cw.OldPassword =textBoxOldPassword.Text;
            butChanagePassword.Enabled = false;

            int count = 0;
            Timer t = new Timer();
            t.Interval = 1000;
            t.Enabled = true;
            t.Tick += delegate(object sender1, EventArgs e1)
            {
                count++;
                butChanagePassword.Text = "提交(" + (5-count).ToString() +")";
                if (count == 5)
                {
                    t.Enabled = false;
                    butChanagePassword.Enabled = true;
                    butChanagePassword.Text = "提交";
                    t.Dispose();
                    sender1 = null;
                }
            };

            if (ChanagePassword != null) ChanagePassword(this, cw);//触发密码更新事件

        }

        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!isAdmin)
                this.textBoxOldPassword.ReadOnly = !checkBox1.Checked;
            this.textBoxConfirmPassword.ReadOnly = !checkBox1.Checked;
            this.textBoxNewPassword.ReadOnly = !checkBox1.Checked;
            this.butChanagePassword.Enabled = checkBox1.Checked;

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBoxUserID.Text == myUserID)
                checkBox1.Enabled = true;
        }
    }
}
