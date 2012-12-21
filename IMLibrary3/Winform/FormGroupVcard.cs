using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary3 
{
    public partial class FormGroupVcard : Form
    {
        public FormGroupVcard()
        {
            InitializeComponent();
        }

        #region 事件

        /// <summary>
        /// 创建用户事件代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="groupVcard"></param>
        public delegate void changeEventHandler(object sender, IMLibrary3.Organization.GroupVcard groupVcard);

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

        #endregion

        #region 属性
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
                    this.textBoxGroupID.ReadOnly = true;

                    if (!isAdmin)//如果不是管理员，则只能查看用户信息
                    {
                        this.butSelectGroupID.Visible = false;

                        this.textBoxGroupID.ReadOnly = true;
                        this.textBoxGroupName.ReadOnly = true;
                        this.textBoxSuperiorId.ReadOnly = true;
                        this.numericUpDownOrder.ReadOnly = true;
                    }
                }
            }
            get { return _IsCreate; }
        }

        /// <summary>
        /// 创建或更新群组信息是否成功
        /// </summary>
        public bool isUpdateSuccess = false;

        private IMLibrary3.Organization.GroupVcard _GroupVcard;
        public IMLibrary3.Organization.GroupVcard GroupVcard
        {
            set
            {
                _GroupVcard = value;
                this.textBoxGroupID.Text = _GroupVcard.GroupID ;
                this.textBoxGroupName.Text = _GroupVcard.GroupName ;
                this.textBoxSuperiorId.Text = _GroupVcard.SuperiorID ;
                this.numericUpDownOrder.Value = _GroupVcard.OrderID ;
                
            }
            get
            {
                if (_GroupVcard == null) _GroupVcard = new  IMLibrary3.Organization.GroupVcard();
                _GroupVcard.GroupID = this.textBoxGroupID.Text.Trim();
                _GroupVcard.GroupName = this.textBoxGroupName.Text.Trim();
                _GroupVcard.SuperiorID = this.textBoxSuperiorId.Text.Trim();
                _GroupVcard.OrderID = (int)this.numericUpDownOrder.Value;

                return _GroupVcard;
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
            if (textBoxGroupID.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写分组编号！"); return;
            }
            if (textBoxGroupName.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写分组名称！"); return;
            }
           

            this.butOK.Enabled = false;
            this.isUpdateSuccess = false;
            timer1.Enabled = true;

            if (UpdateVcard != null)
                UpdateVcard(this, GroupVcard);
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            if (textBoxGroupID.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写分组编号！"); return;
            }
            if (textBoxGroupName.Text.Trim() == "")
            {
                IMLibrary3.Global.MsgShow("请填写分组名称！"); return;
            }
           
            IsCreate = true;

            this.butCreate.Enabled = false;
            this.isUpdateSuccess = false;
            timer1.Enabled = true;

            if (Create  != null)
                Create(this, GroupVcard);
        }


        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butSelectGroupID_Click(object sender, EventArgs e)
        {
            formSelectGroup frmSelGroup = new formSelectGroup();
            frmSelGroup.LoadData();
            frmSelGroup.selectGroup(this.textBoxSuperiorId.Text);
            frmSelGroup.ShowDialog();
            if (frmSelGroup != null) 
                this.textBoxSuperiorId.Text = frmSelGroup.Group.GroupID;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            outTime++;
            if (isUpdateSuccess)
            {
                timer1.Enabled = false;
                if (!IsCreate)
                {
                    IMLibrary3.Global.MsgShow("更新新分组信息成功！");
                    this.Close();
                }
                else
                {
                    IMLibrary3.Global.MsgShow("创建分组成功！");
                    string groupID = this.textBoxSuperiorId.Text;
                    GroupVcard = new  IMLibrary3.Organization.GroupVcard();
                    this.textBoxSuperiorId.Text=groupID;
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
            //return;
            butRefresh.Enabled = false;
            if (RefreshVcard != null)
                RefreshVcard(this, GroupVcard);
            Timer timer = new Timer();
            timer.Interval = 5000;//5秒超时
            timer.Enabled = true;
            timer.Tick += delegate(object sender1, EventArgs e1)
            {
                timer.Enabled = false;
                butRefresh.Enabled = true ;
                timer.Dispose();
                timer = null;
            };
        }
    }
}
