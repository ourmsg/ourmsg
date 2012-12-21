namespace OurMsg
{
    partial class FormUsersToGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsersToGroup));
            this.panel4 = new System.Windows.Forms.Panel();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelGroupCount = new BSE.Windows.Forms.Panel();
            this.treeView_Organization = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel6 = new BSE.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel5 = new System.Windows.Forms.Panel();
            this.butDelUser = new System.Windows.Forms.Button();
            this.butAddUser = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panelGroupCount.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.butOK);
            this.panel4.Controls.Add(this.butCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 366);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(596, 35);
            this.panel4.TabIndex = 49;
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(437, 6);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(73, 26);
            this.butOK.TabIndex = 5;
            this.butOK.Text = "确定";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(516, 6);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(73, 26);
            this.butCancel.TabIndex = 4;
            this.butCancel.Text = "取消";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(596, 6);
            this.panel1.TabIndex = 50;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(590, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(6, 360);
            this.panel2.TabIndex = 51;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(6, 360);
            this.panel3.TabIndex = 52;
            // 
            // panelGroupCount
            // 
            this.panelGroupCount.AssociatedSplitter = null;
            this.panelGroupCount.BackColor = System.Drawing.Color.Transparent;
            this.panelGroupCount.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panelGroupCount.CaptionHeight = 22;
            this.panelGroupCount.Controls.Add(this.treeView_Organization);
            this.panelGroupCount.CustomColors.BorderColor = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.CustomColors.CaptionGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelGroupCount.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panelGroupCount.CustomColors.CaptionGradientMiddle = System.Drawing.SystemColors.ButtonFace;
            this.panelGroupCount.CustomColors.CaptionSelectedGradientBegin = System.Drawing.SystemColors.Window;
            this.panelGroupCount.CustomColors.CaptionSelectedGradientEnd = System.Drawing.SystemColors.Window;
            this.panelGroupCount.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelGroupCount.CustomColors.ContentGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panelGroupCount.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panelGroupCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelGroupCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.Image = null;
            this.panelGroupCount.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelGroupCount.Location = new System.Drawing.Point(6, 6);
            this.panelGroupCount.MinimumSize = new System.Drawing.Size(22, 22);
            this.panelGroupCount.Name = "panelGroupCount";
            this.panelGroupCount.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelGroupCount.Size = new System.Drawing.Size(312, 360);
            this.panelGroupCount.TabIndex = 53;
            this.panelGroupCount.Text = "分组信息";
            this.panelGroupCount.ToolTipTextCloseIcon = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // treeView_Organization
            // 
            this.treeView_Organization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView_Organization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Organization.ImageIndex = 1;
            this.treeView_Organization.ImageList = this.imageList1;
            this.treeView_Organization.Location = new System.Drawing.Point(1, 23);
            this.treeView_Organization.Name = "treeView_Organization";
            this.treeView_Organization.SelectedImageIndex = 0;
            this.treeView_Organization.Size = new System.Drawing.Size(310, 336);
            this.treeView_Organization.TabIndex = 0;
            this.treeView_Organization.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_Organization_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // panel6
            // 
            this.panel6.AssociatedSplitter = null;
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panel6.CaptionHeight = 22;
            this.panel6.Controls.Add(this.listView1);
            this.panel6.CustomColors.BorderColor = System.Drawing.SystemColors.ControlText;
            this.panel6.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel6.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel6.CustomColors.CaptionGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel6.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel6.CustomColors.CaptionGradientMiddle = System.Drawing.SystemColors.ButtonFace;
            this.panel6.CustomColors.CaptionSelectedGradientBegin = System.Drawing.SystemColors.Window;
            this.panel6.CustomColors.CaptionSelectedGradientEnd = System.Drawing.SystemColors.Window;
            this.panel6.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel6.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel6.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel6.CustomColors.ContentGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel6.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel6.Image = null;
            this.panel6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel6.Location = new System.Drawing.Point(392, 6);
            this.panel6.MinimumSize = new System.Drawing.Size(22, 22);
            this.panel6.Name = "panel6";
            this.panel6.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panel6.Size = new System.Drawing.Size(198, 360);
            this.panel6.TabIndex = 55;
            this.panel6.Text = "已选联系人";
            this.panel6.ToolTipTextCloseIcon = null;
            this.panel6.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel6.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(1, 23);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(196, 336);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 140;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.butDelUser);
            this.panel5.Controls.Add(this.butAddUser);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(318, 6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(74, 360);
            this.panel5.TabIndex = 56;
            // 
            // butDelUser
            // 
            this.butDelUser.Location = new System.Drawing.Point(11, 233);
            this.butDelUser.Name = "butDelUser";
            this.butDelUser.Size = new System.Drawing.Size(54, 21);
            this.butDelUser.TabIndex = 1;
            this.butDelUser.Text = "< 删除";
            this.butDelUser.UseVisualStyleBackColor = true;
            this.butDelUser.Click += new System.EventHandler(this.butDelUser_Click);
            // 
            // butAddUser
            // 
            this.butAddUser.Location = new System.Drawing.Point(11, 76);
            this.butAddUser.Name = "butAddUser";
            this.butAddUser.Size = new System.Drawing.Size(54, 21);
            this.butAddUser.TabIndex = 0;
            this.butAddUser.Text = "添加 >";
            this.butAddUser.UseVisualStyleBackColor = true;
            this.butAddUser.Click += new System.EventHandler(this.butAddUser_Click);
            // 
            // FormUsersToGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 401);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panelGroupCount);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormUsersToGroup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "群用户设置";
            this.Load += new System.EventHandler(this.FormUsersToGroup_Load);
            this.panel4.ResumeLayout(false);
            this.panelGroupCount.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private BSE.Windows.Forms.Panel panelGroupCount;
        private System.Windows.Forms.TreeView treeView_Organization;
        private BSE.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button butDelUser;
        private System.Windows.Forms.Button butAddUser;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;

    }
}