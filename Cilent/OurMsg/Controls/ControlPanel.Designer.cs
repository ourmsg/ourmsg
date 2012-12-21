namespace OurMsg.Controls
{
    partial class ControlPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timerLogin1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tButBaseUsers = new System.Windows.Forms.ToolStripSplitButton();
            this.tbutSendNoticeMsg = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmsCreateGroupVcard = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsCreateUserVcard = new System.Windows.Forms.ToolStripMenuItem();
            this.tButBaseGroups = new System.Windows.Forms.ToolStripSplitButton();
            this.tButCreateGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControlUp = new System.Windows.Forms.Panel();
            this.treeView_Organization = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TsmCreateGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmCreateUser = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmDelGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmDelUser = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmShow = new System.Windows.Forms.ToolStripSeparator();
            this.TsmShowUserVcard = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmShowGroupVcard = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView_Rooms = new System.Windows.Forms.TreeView();
            this.panelTreeView = new System.Windows.Forms.Panel();
            this.toolStripMain.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panelTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            // 
            // timerLogin1
            // 
            this.timerLogin1.Interval = 1000;
            this.timerLogin1.Tick += new System.EventHandler(this.timerLogin1_Tick);
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tButBaseUsers,
            this.tButBaseGroups});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMain.Size = new System.Drawing.Size(176, 25);
            this.toolStripMain.TabIndex = 29;
            this.toolStripMain.Text = "toolStrip3";
            // 
            // tButBaseUsers
            // 
            this.tButBaseUsers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbutSendNoticeMsg,
            this.toolStripMenuItem1,
            this.tmsCreateGroupVcard,
            this.tmsCreateUserVcard});
            this.tButBaseUsers.Image = ((System.Drawing.Image)(resources.GetObject("tButBaseUsers.Image")));
            this.tButBaseUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButBaseUsers.Name = "tButBaseUsers";
            this.tButBaseUsers.Size = new System.Drawing.Size(48, 22);
            this.tButBaseUsers.Text = "      ";
            this.tButBaseUsers.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tButBaseUsers.ButtonClick += new System.EventHandler(this.tButBaseUsers_ButtonClick);
            // 
            // tbutSendNoticeMsg
            // 
            this.tbutSendNoticeMsg.Name = "tbutSendNoticeMsg";
            this.tbutSendNoticeMsg.Size = new System.Drawing.Size(148, 22);
            this.tbutSendNoticeMsg.Text = "发送通知消息";
            this.tbutSendNoticeMsg.Click += new System.EventHandler(this.tbutSendNoticeMsg_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // tmsCreateGroupVcard
            // 
            this.tmsCreateGroupVcard.Name = "tmsCreateGroupVcard";
            this.tmsCreateGroupVcard.Size = new System.Drawing.Size(148, 22);
            this.tmsCreateGroupVcard.Text = "创建分组";
            this.tmsCreateGroupVcard.Click += new System.EventHandler(this.tmsCreateGroupVcard_Click);
            // 
            // tmsCreateUserVcard
            // 
            this.tmsCreateUserVcard.Name = "tmsCreateUserVcard";
            this.tmsCreateUserVcard.Size = new System.Drawing.Size(148, 22);
            this.tmsCreateUserVcard.Text = "创建用户";
            this.tmsCreateUserVcard.Click += new System.EventHandler(this.tmsCreateUserVcard_Click);
            // 
            // tButBaseGroups
            // 
            this.tButBaseGroups.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tButCreateGroup});
            this.tButBaseGroups.Image = ((System.Drawing.Image)(resources.GetObject("tButBaseGroups.Image")));
            this.tButBaseGroups.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButBaseGroups.Name = "tButBaseGroups";
            this.tButBaseGroups.Size = new System.Drawing.Size(48, 22);
            this.tButBaseGroups.Text = "      ";
            this.tButBaseGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tButBaseGroups.ButtonClick += new System.EventHandler(this.tButBaseGroups_ButtonClick);
            // 
            // tButCreateGroup
            // 
            this.tButCreateGroup.Name = "tButCreateGroup";
            this.tButCreateGroup.Size = new System.Drawing.Size(112, 22);
            this.tButCreateGroup.Text = "创建群";
            this.tButCreateGroup.Click += new System.EventHandler(this.tButCreateGroup_Click);
            // 
            // panelControlUp
            // 
            this.panelControlUp.BackColor = System.Drawing.Color.White;
            this.panelControlUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlUp.Location = new System.Drawing.Point(0, 25);
            this.panelControlUp.Name = "panelControlUp";
            this.panelControlUp.Size = new System.Drawing.Size(176, 4);
            this.panelControlUp.TabIndex = 31;
            // 
            // treeView_Organization
            // 
            this.treeView_Organization.BackColor = System.Drawing.SystemColors.Window;
            this.treeView_Organization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView_Organization.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView_Organization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Organization.ForeColor = System.Drawing.Color.Black;
            this.treeView_Organization.ImageIndex = 0;
            this.treeView_Organization.ImageList = this.imageList1;
            this.treeView_Organization.LineColor = System.Drawing.Color.Blue;
            this.treeView_Organization.Location = new System.Drawing.Point(0, 0);
            this.treeView_Organization.Name = "treeView_Organization";
            this.treeView_Organization.SelectedImageIndex = 0;
            this.treeView_Organization.ShowLines = false;
            this.treeView_Organization.ShowRootLines = false;
            this.treeView_Organization.Size = new System.Drawing.Size(176, 312);
            this.treeView_Organization.TabIndex = 32;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmCreateGroup,
            this.TsmCreateUser,
            this.TsmDelGroup,
            this.TsmDelUser,
            this.TsmShow,
            this.TsmShowUserVcard,
            this.TsmShowGroupVcard});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 164);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // TsmCreateGroup
            // 
            this.TsmCreateGroup.Name = "TsmCreateGroup";
            this.TsmCreateGroup.Size = new System.Drawing.Size(152, 22);
            this.TsmCreateGroup.Text = "创建分组";
            this.TsmCreateGroup.Click += new System.EventHandler(this.TsmCreateGroup_Click);
            // 
            // TsmCreateUser
            // 
            this.TsmCreateUser.Name = "TsmCreateUser";
            this.TsmCreateUser.Size = new System.Drawing.Size(152, 22);
            this.TsmCreateUser.Text = "创建用户";
            this.TsmCreateUser.Click += new System.EventHandler(this.TsmCreateUser_Click);
            // 
            // TsmDelGroup
            // 
            this.TsmDelGroup.Name = "TsmDelGroup";
            this.TsmDelGroup.Size = new System.Drawing.Size(152, 22);
            this.TsmDelGroup.Text = "删除分组";
            this.TsmDelGroup.Click += new System.EventHandler(this.TsmDelGroup_Click);
            // 
            // TsmDelUser
            // 
            this.TsmDelUser.Name = "TsmDelUser";
            this.TsmDelUser.Size = new System.Drawing.Size(152, 22);
            this.TsmDelUser.Text = "删除用户";
            this.TsmDelUser.Click += new System.EventHandler(this.TsmDelUser_Click);
            // 
            // TsmShow
            // 
            this.TsmShow.Name = "TsmShow";
            this.TsmShow.Size = new System.Drawing.Size(149, 6);
            // 
            // TsmShowUserVcard
            // 
            this.TsmShowUserVcard.Name = "TsmShowUserVcard";
            this.TsmShowUserVcard.Size = new System.Drawing.Size(152, 22);
            this.TsmShowUserVcard.Text = "查看用户资料";
            this.TsmShowUserVcard.Click += new System.EventHandler(this.TsmShowUserVcard_Click);
            // 
            // TsmShowGroupVcard
            // 
            this.TsmShowGroupVcard.Name = "TsmShowGroupVcard";
            this.TsmShowGroupVcard.Size = new System.Drawing.Size(152, 22);
            this.TsmShowGroupVcard.Text = "查看分组资料";
            this.TsmShowGroupVcard.Click += new System.EventHandler(this.TsmShowGroupVcard_Click);
            // 
            // treeView_Rooms
            // 
            this.treeView_Rooms.BackColor = System.Drawing.SystemColors.Window;
            this.treeView_Rooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView_Rooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Rooms.ForeColor = System.Drawing.Color.Black;
            this.treeView_Rooms.ImageIndex = 0;
            this.treeView_Rooms.ImageList = this.imageList1;
            this.treeView_Rooms.LineColor = System.Drawing.Color.Blue;
            this.treeView_Rooms.Location = new System.Drawing.Point(0, 0);
            this.treeView_Rooms.Name = "treeView_Rooms";
            this.treeView_Rooms.SelectedImageIndex = 0;
            this.treeView_Rooms.ShowLines = false;
            this.treeView_Rooms.ShowRootLines = false;
            this.treeView_Rooms.Size = new System.Drawing.Size(176, 312);
            this.treeView_Rooms.TabIndex = 33;
            // 
            // panelTreeView
            // 
            this.panelTreeView.Controls.Add(this.treeView_Organization);
            this.panelTreeView.Controls.Add(this.treeView_Rooms);
            this.panelTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTreeView.Location = new System.Drawing.Point(0, 29);
            this.panelTreeView.Name = "panelTreeView";
            this.panelTreeView.Size = new System.Drawing.Size(176, 312);
            this.panelTreeView.TabIndex = 34;
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelTreeView);
            this.Controls.Add(this.panelControlUp);
            this.Controls.Add(this.toolStripMain);
            this.Name = "ControlPanel";
            this.Size = new System.Drawing.Size(176, 341);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelTreeView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timerLogin1;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.Panel panelControlUp;
        private System.Windows.Forms.TreeView treeView_Organization;
        private System.Windows.Forms.ToolStripSplitButton tButBaseUsers;
        private System.Windows.Forms.ToolStripMenuItem tbutSendNoticeMsg;
        private System.Windows.Forms.ToolStripSplitButton tButBaseGroups;
        private System.Windows.Forms.ToolStripMenuItem tButCreateGroup;
        private System.Windows.Forms.TreeView treeView_Rooms;
        private System.Windows.Forms.Panel panelTreeView;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tmsCreateUserVcard;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TsmCreateGroup;
        private System.Windows.Forms.ToolStripMenuItem TsmCreateUser;
        private System.Windows.Forms.ToolStripMenuItem TsmShowUserVcard;
        private System.Windows.Forms.ToolStripMenuItem TsmDelGroup;
        private System.Windows.Forms.ToolStripMenuItem TsmDelUser;
        private System.Windows.Forms.ToolStripSeparator TsmShow;
        private System.Windows.Forms.ToolStripMenuItem TsmShowGroupVcard;
        private System.Windows.Forms.ToolStripMenuItem tmsCreateGroupVcard;
    }
}
