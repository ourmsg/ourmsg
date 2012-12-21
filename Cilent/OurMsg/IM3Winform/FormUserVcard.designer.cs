namespace IMLibrary3 
{
    partial class FormUserVcard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.butSelectGroupID = new System.Windows.Forms.Button();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxPost = new System.Windows.Forms.TextBox();
            this.textBoxJob = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPhone = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxOfficePhone = new System.Windows.Forms.TextBox();
            this.dateTimePickerBirthday = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxSex = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownOrder = new System.Windows.Forms.NumericUpDown();
            this.textBoxGroupID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butChanagePassword = new System.Windows.Forms.Button();
            this.textBoxConfirmPassword = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxOldPassword = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxIsAdmin = new System.Windows.Forms.CheckBox();
            this.checkBoxIsSendNotice = new System.Windows.Forms.CheckBox();
            this.checkBoxIsSendSMS = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDownCreateRooms = new System.Windows.Forms.NumericUpDown();
            this.checkBoxDisable = new System.Windows.Forms.CheckBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.butCreate = new System.Windows.Forms.Button();
            this.butRefresh = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrder)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCreateRooms)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(7, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(407, 296);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.butSelectGroupID);
            this.tabPage1.Controls.Add(this.textBoxMail);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.textBoxPost);
            this.tabPage1.Controls.Add(this.textBoxJob);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.textBoxPhone);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.textBoxOfficePhone);
            this.tabPage1.Controls.Add(this.dateTimePickerBirthday);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.comboBoxSex);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.numericUpDownOrder);
            this.tabPage1.Controls.Add(this.textBoxGroupID);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxRemark);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxUserName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxUserID);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(399, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本资料";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // butSelectGroupID
            // 
            this.butSelectGroupID.Location = new System.Drawing.Point(351, 40);
            this.butSelectGroupID.Name = "butSelectGroupID";
            this.butSelectGroupID.Size = new System.Drawing.Size(32, 23);
            this.butSelectGroupID.TabIndex = 8;
            this.butSelectGroupID.Text = "...";
            this.butSelectGroupID.UseVisualStyleBackColor = true;
            this.butSelectGroupID.Click += new System.EventHandler(this.butSelectGroupID_Click);
            // 
            // textBoxMail
            // 
            this.textBoxMail.Location = new System.Drawing.Point(67, 159);
            this.textBoxMail.MaxLength = 20;
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(315, 21);
            this.textBoxMail.TabIndex = 38;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 162);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 37;
            this.label13.Text = "邮  箱:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(208, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 36;
            this.label11.Text = "职  务:";
            // 
            // textBoxPost
            // 
            this.textBoxPost.Location = new System.Drawing.Point(257, 100);
            this.textBoxPost.MaxLength = 20;
            this.textBoxPost.Name = "textBoxPost";
            this.textBoxPost.Size = new System.Drawing.Size(125, 21);
            this.textBoxPost.TabIndex = 35;
            // 
            // textBoxJob
            // 
            this.textBoxJob.Location = new System.Drawing.Point(67, 100);
            this.textBoxJob.MaxLength = 20;
            this.textBoxJob.Name = "textBoxJob";
            this.textBoxJob.Size = new System.Drawing.Size(125, 21);
            this.textBoxJob.TabIndex = 34;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 103);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 33;
            this.label10.Text = "岗  位:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(208, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "手  机:";
            // 
            // textBoxPhone
            // 
            this.textBoxPhone.Location = new System.Drawing.Point(257, 129);
            this.textBoxPhone.MaxLength = 20;
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Size = new System.Drawing.Size(125, 21);
            this.textBoxPhone.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "办公电话:";
            // 
            // textBoxOfficePhone
            // 
            this.textBoxOfficePhone.Location = new System.Drawing.Point(67, 129);
            this.textBoxOfficePhone.MaxLength = 20;
            this.textBoxOfficePhone.Name = "textBoxOfficePhone";
            this.textBoxOfficePhone.Size = new System.Drawing.Size(125, 21);
            this.textBoxOfficePhone.TabIndex = 29;
            // 
            // dateTimePickerBirthday
            // 
            this.dateTimePickerBirthday.Location = new System.Drawing.Point(67, 71);
            this.dateTimePickerBirthday.Name = "dateTimePickerBirthday";
            this.dateTimePickerBirthday.Size = new System.Drawing.Size(125, 21);
            this.dateTimePickerBirthday.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "出生日期:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "性  别:";
            // 
            // comboBoxSex
            // 
            this.comboBoxSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSex.FormattingEnabled = true;
            this.comboBoxSex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.comboBoxSex.Location = new System.Drawing.Point(67, 41);
            this.comboBoxSex.Name = "comboBoxSex";
            this.comboBoxSex.Size = new System.Drawing.Size(125, 20);
            this.comboBoxSex.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(208, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "排  序:";
            // 
            // numericUpDownOrder
            // 
            this.numericUpDownOrder.Location = new System.Drawing.Point(257, 71);
            this.numericUpDownOrder.Name = "numericUpDownOrder";
            this.numericUpDownOrder.Size = new System.Drawing.Size(125, 21);
            this.numericUpDownOrder.TabIndex = 21;
            // 
            // textBoxGroupID
            // 
            this.textBoxGroupID.Location = new System.Drawing.Point(257, 41);
            this.textBoxGroupID.MaxLength = 10;
            this.textBoxGroupID.Name = "textBoxGroupID";
            this.textBoxGroupID.ReadOnly = true;
            this.textBoxGroupID.Size = new System.Drawing.Size(125, 21);
            this.textBoxGroupID.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(208, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "分  组:";
            // 
            // textBoxRemark
            // 
            this.textBoxRemark.Location = new System.Drawing.Point(67, 189);
            this.textBoxRemark.MaxLength = 50;
            this.textBoxRemark.Multiline = true;
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.Size = new System.Drawing.Size(315, 68);
            this.textBoxRemark.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "说  明：";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(257, 12);
            this.textBoxUserName.MaxLength = 20;
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(125, 21);
            this.textBoxUserName.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "姓  名:";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Location = new System.Drawing.Point(67, 12);
            this.textBoxUserID.MaxLength = 20;
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(125, 21);
            this.textBoxUserID.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "帐  号:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(399, 270);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "安全设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(39, 31);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "修改密码";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butChanagePassword);
            this.groupBox1.Controls.Add(this.textBoxConfirmPassword);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBoxNewPassword);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.textBoxOldPassword);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Location = new System.Drawing.Point(39, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 192);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // butChanagePassword
            // 
            this.butChanagePassword.Enabled = false;
            this.butChanagePassword.Location = new System.Drawing.Point(114, 160);
            this.butChanagePassword.Name = "butChanagePassword";
            this.butChanagePassword.Size = new System.Drawing.Size(73, 26);
            this.butChanagePassword.TabIndex = 12;
            this.butChanagePassword.Text = "提交";
            this.butChanagePassword.UseVisualStyleBackColor = true;
            this.butChanagePassword.Click += new System.EventHandler(this.butChanagePassword_Click);
            // 
            // textBoxConfirmPassword
            // 
            this.textBoxConfirmPassword.Location = new System.Drawing.Point(114, 116);
            this.textBoxConfirmPassword.MaxLength = 32;
            this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            this.textBoxConfirmPassword.PasswordChar = '*';
            this.textBoxConfirmPassword.ReadOnly = true;
            this.textBoxConfirmPassword.Size = new System.Drawing.Size(141, 21);
            this.textBoxConfirmPassword.TabIndex = 11;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 119);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 10;
            this.label16.Text = "确认新密码：";
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(114, 75);
            this.textBoxNewPassword.MaxLength = 32;
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.PasswordChar = '*';
            this.textBoxNewPassword.ReadOnly = true;
            this.textBoxNewPassword.Size = new System.Drawing.Size(141, 21);
            this.textBoxNewPassword.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(55, 78);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 8;
            this.label15.Text = "新密码：";
            // 
            // textBoxOldPassword
            // 
            this.textBoxOldPassword.Location = new System.Drawing.Point(114, 36);
            this.textBoxOldPassword.MaxLength = 32;
            this.textBoxOldPassword.Name = "textBoxOldPassword";
            this.textBoxOldPassword.PasswordChar = '*';
            this.textBoxOldPassword.ReadOnly = true;
            this.textBoxOldPassword.Size = new System.Drawing.Size(141, 21);
            this.textBoxOldPassword.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(55, 39);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "旧密码：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxIsAdmin);
            this.tabPage2.Controls.Add(this.checkBoxIsSendNotice);
            this.tabPage2.Controls.Add(this.checkBoxIsSendSMS);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.numericUpDownCreateRooms);
            this.tabPage2.Controls.Add(this.checkBoxDisable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(399, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsAdmin
            // 
            this.checkBoxIsAdmin.AutoSize = true;
            this.checkBoxIsAdmin.Location = new System.Drawing.Point(26, 64);
            this.checkBoxIsAdmin.Name = "checkBoxIsAdmin";
            this.checkBoxIsAdmin.Size = new System.Drawing.Size(60, 16);
            this.checkBoxIsAdmin.TabIndex = 27;
            this.checkBoxIsAdmin.Text = "管理员";
            this.checkBoxIsAdmin.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsSendNotice
            // 
            this.checkBoxIsSendNotice.AutoSize = true;
            this.checkBoxIsSendNotice.Location = new System.Drawing.Point(26, 93);
            this.checkBoxIsSendNotice.Name = "checkBoxIsSendNotice";
            this.checkBoxIsSendNotice.Size = new System.Drawing.Size(96, 16);
            this.checkBoxIsSendNotice.TabIndex = 26;
            this.checkBoxIsSendNotice.Text = "允许发送通知";
            this.checkBoxIsSendNotice.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsSendSMS
            // 
            this.checkBoxIsSendSMS.AutoSize = true;
            this.checkBoxIsSendSMS.Location = new System.Drawing.Point(26, 126);
            this.checkBoxIsSendSMS.Name = "checkBoxIsSendSMS";
            this.checkBoxIsSendSMS.Size = new System.Drawing.Size(96, 16);
            this.checkBoxIsSendSMS.TabIndex = 25;
            this.checkBoxIsSendSMS.Text = "允许发送短信";
            this.checkBoxIsSendSMS.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(217, 229);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 24;
            this.label12.Text = "允许创建群组数:";
            // 
            // numericUpDownCreateRooms
            // 
            this.numericUpDownCreateRooms.Location = new System.Drawing.Point(318, 227);
            this.numericUpDownCreateRooms.Name = "numericUpDownCreateRooms";
            this.numericUpDownCreateRooms.Size = new System.Drawing.Size(62, 21);
            this.numericUpDownCreateRooms.TabIndex = 23;
            this.numericUpDownCreateRooms.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxDisable
            // 
            this.checkBoxDisable.AutoSize = true;
            this.checkBoxDisable.Location = new System.Drawing.Point(26, 33);
            this.checkBoxDisable.Name = "checkBoxDisable";
            this.checkBoxDisable.Size = new System.Drawing.Size(72, 16);
            this.checkBoxDisable.TabIndex = 0;
            this.checkBoxDisable.Text = "禁止登录";
            this.checkBoxDisable.UseVisualStyleBackColor = true;
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(339, 305);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(73, 26);
            this.butOK.TabIndex = 6;
            this.butOK.Text = "确定";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(258, 305);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(73, 26);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "取消";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butCreate
            // 
            this.butCreate.Location = new System.Drawing.Point(340, 305);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(73, 26);
            this.butCreate.TabIndex = 4;
            this.butCreate.Text = "创建";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Visible = false;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // butRefresh
            // 
            this.butRefresh.Location = new System.Drawing.Point(7, 305);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(73, 26);
            this.butRefresh.TabIndex = 7;
            this.butRefresh.Text = "更新";
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Visible = false;
            this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormUserVcard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 338);
            this.Controls.Add(this.butRefresh);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butCreate);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormUserVcard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户资料";
            this.Load += new System.EventHandler(this.FormUserVcard_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrder)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCreateRooms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.TextBox textBoxGroupID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownOrder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxSex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateTimePickerBirthday;
        private System.Windows.Forms.TextBox textBoxOfficePhone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxPost;
        private System.Windows.Forms.TextBox textBoxJob;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numericUpDownCreateRooms;
        private System.Windows.Forms.CheckBox checkBoxDisable;
        private System.Windows.Forms.CheckBox checkBoxIsAdmin;
        private System.Windows.Forms.CheckBox checkBoxIsSendNotice;
        private System.Windows.Forms.CheckBox checkBoxIsSendSMS;
        private System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.TextBox textBoxMail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button butSelectGroupID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxConfirmPassword;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxOldPassword;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button butChanagePassword;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}