namespace OurMsg 
{
    partial class FormCreateRoom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
          

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateRoom));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelUserCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.butDelUser = new System.Windows.Forms.Button();
            this.butAddUsers = new System.Windows.Forms.Button();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listViewGroupUsers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBoxGroupNotice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCreateUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGroupID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butCreateGroup = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(449, 350);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelUserCount);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.butDelUser);
            this.tabPage1.Controls.Add(this.butAddUsers);
            this.tabPage1.Controls.Add(this.textBoxGroupName);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.listViewGroupUsers);
            this.tabPage1.Controls.Add(this.textBoxGroupNotice);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxCreateUser);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxGroupID);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(441, 324);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // labelUserCount
            // 
            this.labelUserCount.AutoSize = true;
            this.labelUserCount.Location = new System.Drawing.Point(116, 293);
            this.labelUserCount.Name = "labelUserCount";
            this.labelUserCount.Size = new System.Drawing.Size(11, 12);
            this.labelUserCount.TabIndex = 14;
            this.labelUserCount.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "成员数:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(199, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "申请";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // butDelUser
            // 
            this.butDelUser.Enabled = false;
            this.butDelUser.Location = new System.Drawing.Point(266, 293);
            this.butDelUser.Name = "butDelUser";
            this.butDelUser.Size = new System.Drawing.Size(69, 24);
            this.butDelUser.TabIndex = 11;
            this.butDelUser.Text = "删除成员";
            this.butDelUser.UseVisualStyleBackColor = true;
            this.butDelUser.Click += new System.EventHandler(this.butDelUser_Click);
            // 
            // butAddUsers
            // 
            this.butAddUsers.Location = new System.Drawing.Point(340, 293);
            this.butAddUsers.Name = "butAddUsers";
            this.butAddUsers.Size = new System.Drawing.Size(81, 24);
            this.butAddUsers.TabIndex = 10;
            this.butAddUsers.Text = "添加成员";
            this.butAddUsers.UseVisualStyleBackColor = true;
            this.butAddUsers.Click += new System.EventHandler(this.butAddUsers_Click);
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Location = new System.Drawing.Point(74, 48);
            this.textBoxGroupName.MaxLength = 10;
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(347, 21);
            this.textBoxGroupName.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "群  名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "群成员:";
            // 
            // listViewGroupUsers
            // 
            this.listViewGroupUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewGroupUsers.FullRowSelect = true;
            this.listViewGroupUsers.Location = new System.Drawing.Point(74, 117);
            this.listViewGroupUsers.MultiSelect = false;
            this.listViewGroupUsers.Name = "listViewGroupUsers";
            this.listViewGroupUsers.Size = new System.Drawing.Size(347, 170);
            this.listViewGroupUsers.SmallImageList = this.imageList1;
            this.listViewGroupUsers.TabIndex = 6;
            this.listViewGroupUsers.UseCompatibleStateImageBehavior = false;
            this.listViewGroupUsers.View = System.Windows.Forms.View.Details;
            this.listViewGroupUsers.SelectedIndexChanged += new System.EventHandler(this.listViewGroupUsers_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "用户帐号";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "用户名";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "姓别";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "");
            // 
            // textBoxGroupNotice
            // 
            this.textBoxGroupNotice.Location = new System.Drawing.Point(74, 78);
            this.textBoxGroupNotice.MaxLength = 50;
            this.textBoxGroupNotice.Multiline = true;
            this.textBoxGroupNotice.Name = "textBoxGroupNotice";
            this.textBoxGroupNotice.Size = new System.Drawing.Size(347, 33);
            this.textBoxGroupNotice.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "群公告:";
            // 
            // textBoxCreateUser
            // 
            this.textBoxCreateUser.Location = new System.Drawing.Point(296, 19);
            this.textBoxCreateUser.MaxLength = 20;
            this.textBoxCreateUser.Name = "textBoxCreateUser";
            this.textBoxCreateUser.ReadOnly = true;
            this.textBoxCreateUser.Size = new System.Drawing.Size(125, 21);
            this.textBoxCreateUser.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "创建者:";
            // 
            // textBoxGroupID
            // 
            this.textBoxGroupID.Location = new System.Drawing.Point(74, 19);
            this.textBoxGroupID.MaxLength = 20;
            this.textBoxGroupID.Name = "textBoxGroupID";
            this.textBoxGroupID.ReadOnly = true;
            this.textBoxGroupID.Size = new System.Drawing.Size(125, 21);
            this.textBoxGroupID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "群  号:";
            // 
            // butCreateGroup
            // 
            this.butCreateGroup.Location = new System.Drawing.Point(374, 359);
            this.butCreateGroup.Name = "butCreateGroup";
            this.butCreateGroup.Size = new System.Drawing.Size(73, 26);
            this.butCreateGroup.TabIndex = 1;
            this.butCreateGroup.Text = "创建";
            this.butCreateGroup.UseVisualStyleBackColor = true;
            this.butCreateGroup.Visible = false;
            this.butCreateGroup.Click += new System.EventHandler(this.butCreateGroup_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(292, 359);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(73, 26);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "取消";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(374, 359);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(73, 26);
            this.butOK.TabIndex = 3;
            this.butOK.Text = "确定";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Visible = false;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormCreateRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 391);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butCreateGroup);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormCreateRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "群资料设置";
            this.Load += new System.EventHandler(this.FormCreateRoom_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button butCreateGroup;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGroupID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxGroupNotice;
        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listViewGroupUsers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button butDelUser;
        private System.Windows.Forms.Button butOK;
        public System.Windows.Forms.Button butAddUsers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelUserCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBoxCreateUser;

    }
}