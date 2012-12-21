namespace IMLibrary3 
{
    partial class FormGroupVcard
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
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownOrder = new System.Windows.Forms.NumericUpDown();
            this.textBoxSuperiorId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGroupID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.butCreate = new System.Windows.Forms.Button();
            this.butRefresh = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(7, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(407, 296);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.butSelectGroupID);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.numericUpDownOrder);
            this.tabPage1.Controls.Add(this.textBoxSuperiorId);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxGroupName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxGroupID);
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
            this.butSelectGroupID.Location = new System.Drawing.Point(329, 110);
            this.butSelectGroupID.Name = "butSelectGroupID";
            this.butSelectGroupID.Size = new System.Drawing.Size(40, 23);
            this.butSelectGroupID.TabIndex = 8;
            this.butSelectGroupID.Text = "...";
            this.butSelectGroupID.UseVisualStyleBackColor = true;
            this.butSelectGroupID.Click += new System.EventHandler(this.butSelectGroupID_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "排    序:";
            // 
            // numericUpDownOrder
            // 
            this.numericUpDownOrder.Location = new System.Drawing.Point(87, 152);
            this.numericUpDownOrder.Name = "numericUpDownOrder";
            this.numericUpDownOrder.Size = new System.Drawing.Size(281, 21);
            this.numericUpDownOrder.TabIndex = 21;
            // 
            // textBoxSuperiorId
            // 
            this.textBoxSuperiorId.Location = new System.Drawing.Point(87, 111);
            this.textBoxSuperiorId.MaxLength = 10;
            this.textBoxSuperiorId.Name = "textBoxSuperiorId";
            this.textBoxSuperiorId.ReadOnly = true;
            this.textBoxSuperiorId.Size = new System.Drawing.Size(281, 21);
            this.textBoxSuperiorId.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "上级分组:";
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Location = new System.Drawing.Point(87, 72);
            this.textBoxGroupName.MaxLength = 20;
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(281, 21);
            this.textBoxGroupName.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "分组名称:";
            // 
            // textBoxGroupID
            // 
            this.textBoxGroupID.Location = new System.Drawing.Point(87, 33);
            this.textBoxGroupID.MaxLength = 20;
            this.textBoxGroupID.Name = "textBoxGroupID";
            this.textBoxGroupID.Size = new System.Drawing.Size(281, 21);
            this.textBoxGroupID.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "分组编号:";
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
            // FormGroupVcard
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
            this.Name = "FormGroupVcard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分组资料";
            this.Load += new System.EventHandler(this.FormUserVcard_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.TextBox textBoxSuperiorId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGroupID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.Button butSelectGroupID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownOrder;
    }
}