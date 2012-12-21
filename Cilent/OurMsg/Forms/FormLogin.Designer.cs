namespace OurMsg
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.labShowTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBoxUserID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butCancel = new System.Windows.Forms.Button();
            this.butLogin = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxSavePassword = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoLogin = new System.Windows.Forms.CheckBox();
            this.webUpdate1 = new OurMsg.controls.webUpdate();
            this.ShowType1 = new IMLibrary3.Controls.ShowType();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Lime;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labShowTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 67);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Crimson;
            this.label4.Location = new System.Drawing.Point(2, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(262, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Enterprise Message 2012";
            // 
            // labShowTitle
            // 
            this.labShowTitle.AutoSize = true;
            this.labShowTitle.BackColor = System.Drawing.Color.Transparent;
            this.labShowTitle.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labShowTitle.ForeColor = System.Drawing.Color.Orange;
            this.labShowTitle.Location = new System.Drawing.Point(241, 47);
            this.labShowTitle.Name = "labShowTitle";
            this.labShowTitle.Size = new System.Drawing.Size(93, 20);
            this.labShowTitle.TabIndex = 0;
            this.labShowTitle.Text = "用户登录";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "帐号：";
            // 
            // cmbBoxUserID
            // 
            this.cmbBoxUserID.FormattingEnabled = true;
            this.cmbBoxUserID.Location = new System.Drawing.Point(61, 87);
            this.cmbBoxUserID.Name = "cmbBoxUserID";
            this.cmbBoxUserID.Size = new System.Drawing.Size(195, 20);
            this.cmbBoxUserID.TabIndex = 2;
            this.cmbBoxUserID.SelectedIndexChanged += new System.EventHandler(this.cmbBoxUserID_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(61, 118);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(195, 21);
            this.txtPassword.TabIndex = 4;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(267, 92);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 12);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "注 册";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(267, 125);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(35, 12);
            this.linkLabel2.TabIndex = 6;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "找 回";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.butCancel);
            this.panel2.Controls.Add(this.butLogin);
            this.panel2.Location = new System.Drawing.Point(0, 183);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(334, 35);
            this.panel2.TabIndex = 7;
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(12, 6);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(83, 25);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "取消";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butLogin
            // 
            this.butLogin.Location = new System.Drawing.Point(242, 6);
            this.butLogin.Name = "butLogin";
            this.butLogin.Size = new System.Drawing.Size(83, 25);
            this.butLogin.TabIndex = 0;
            this.butLogin.Text = "登录";
            this.butLogin.UseVisualStyleBackColor = true;
            this.butLogin.Click += new System.EventHandler(this.butLogin_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 30;
            this.label3.Text = "状态：";
            // 
            // checkBoxSavePassword
            // 
            this.checkBoxSavePassword.AutoSize = true;
            this.checkBoxSavePassword.Location = new System.Drawing.Point(103, 154);
            this.checkBoxSavePassword.Name = "checkBoxSavePassword";
            this.checkBoxSavePassword.Size = new System.Drawing.Size(72, 16);
            this.checkBoxSavePassword.TabIndex = 31;
            this.checkBoxSavePassword.Text = "记住密码";
            this.checkBoxSavePassword.UseVisualStyleBackColor = true;
            this.checkBoxSavePassword.CheckedChanged += new System.EventHandler(this.checkBoxSavePassword_CheckedChanged);
            // 
            // checkBoxAutoLogin
            // 
            this.checkBoxAutoLogin.AutoSize = true;
            this.checkBoxAutoLogin.Location = new System.Drawing.Point(184, 154);
            this.checkBoxAutoLogin.Name = "checkBoxAutoLogin";
            this.checkBoxAutoLogin.Size = new System.Drawing.Size(72, 16);
            this.checkBoxAutoLogin.TabIndex = 32;
            this.checkBoxAutoLogin.Text = "自动登录";
            this.checkBoxAutoLogin.UseVisualStyleBackColor = true;
            this.checkBoxAutoLogin.CheckedChanged += new System.EventHandler(this.checkBoxAutoLogin_CheckedChanged);
            // 
            // webUpdate1
            // 
            this.webUpdate1.BackColor = System.Drawing.Color.Transparent;
            this.webUpdate1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.webUpdate1.Location = new System.Drawing.Point(0, 216);
            this.webUpdate1.Name = "webUpdate1";
            this.webUpdate1.Size = new System.Drawing.Size(334, 2);
            this.webUpdate1.TabIndex = 34;
            this.webUpdate1.Visible = false;
            // 
            // ShowType1
            // 
            this.ShowType1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ShowType1.IsShowOffline = false;
            this.ShowType1.Location = new System.Drawing.Point(61, 148);
            this.ShowType1.Name = "ShowType1";
            this.ShowType1.Size = new System.Drawing.Size(36, 29);
            this.ShowType1.State = IMLibrary3.Enmu.ShowType.NONE;
            this.ShowType1.TabIndex = 33;
            this.ShowType1.ShowTypeChanged += new IMLibrary3.Controls.ShowType.ShowTypeChangedHandler(this.ShowType1_ShowTypeChanged);
            this.ShowType1.ShowTypeExitApp += new IMLibrary3.Controls.ShowType.ShowTypeChangedHandler(this.ShowType1_ShowTypeExitApp);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 218);
            this.Controls.Add(this.webUpdate1);
            this.Controls.Add(this.ShowType1);
            this.Controls.Add(this.checkBoxAutoLogin);
            this.Controls.Add(this.checkBoxSavePassword);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBoxUserID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OurMsg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butLogin;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label labShowTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBoxUserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox checkBoxSavePassword;
        private System.Windows.Forms.CheckBox checkBoxAutoLogin;
        private IMLibrary3.Controls.ShowType ShowType1;
        private System.Windows.Forms.Label label4;
        private controls.webUpdate webUpdate1;
    }
}