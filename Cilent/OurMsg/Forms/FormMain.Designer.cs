namespace OurMsg
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panelTop = new System.Windows.Forms.Panel();
            this.labUserName = new System.Windows.Forms.Label();
            this.ShowType1 = new IMLibrary3.Controls.ShowType();
            this.panelReserch = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmMyInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMsgMis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tButManageMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelControlUp = new System.Windows.Forms.Panel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fsfsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.联机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.接听电话ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.外出就餐ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暂时离开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工作中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vcxvToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemGroupNotice = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenMain = new System.Windows.Forms.ToolStripMenuItem();
            this.vcxvToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.userLoginPanel1 = new OurMsg.Controls.UserLoginPanel();
            this.controlPanel1 = new OurMsg.Controls.ControlPanel();
            this.selfFaceImage1 = new OurMsg.Controls.SelfFaceImage();
            this.panelTop.SuspendLayout();
            this.panelReserch.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.contextMenuNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.selfFaceImage1);
            this.panelTop.Controls.Add(this.labUserName);
            this.panelTop.Controls.Add(this.ShowType1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(219, 59);
            this.panelTop.TabIndex = 7;
            // 
            // labUserName
            // 
            this.labUserName.AutoSize = true;
            this.labUserName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labUserName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labUserName.Location = new System.Drawing.Point(57, 36);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(101, 12);
            this.labUserName.TabIndex = 23;
            this.labUserName.Text = "某某某(10000000)";
            this.labUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ShowType1
            // 
            this.ShowType1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ShowType1.IsShowOffline = true;
            this.ShowType1.Location = new System.Drawing.Point(57, 8);
            this.ShowType1.Name = "ShowType1";
            this.ShowType1.Size = new System.Drawing.Size(138, 25);
            this.ShowType1.State = IMLibrary3.Enmu.ShowType.NONE;
            this.ShowType1.TabIndex = 24;
            this.ShowType1.ShowTypeChanged += new IMLibrary3.Controls.ShowType.ShowTypeChangedHandler(this.ShowType1_ShowTypeChanged);
            this.ShowType1.ShowTypeExitApp += new IMLibrary3.Controls.ShowType.ShowTypeChangedHandler(this.ShowType1_ShowTypeExitApp);
            // 
            // panelReserch
            // 
            this.panelReserch.BackColor = System.Drawing.Color.Transparent;
            this.panelReserch.Controls.Add(this.panel5);
            this.panelReserch.Controls.Add(this.panel1);
            this.panelReserch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReserch.Location = new System.Drawing.Point(0, 59);
            this.panelReserch.Name = "panelReserch";
            this.panelReserch.Size = new System.Drawing.Size(219, 10);
            this.panelReserch.TabIndex = 20;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(215, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(4, 10);
            this.panel5.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(4, 10);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(219, 30);
            this.statusStrip1.TabIndex = 24;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMyInfo,
            this.tsmMsgMis,
            this.toolStripMenuItem1,
            this.退出ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // tsmMyInfo
            // 
            this.tsmMyInfo.Name = "tsmMyInfo";
            this.tsmMyInfo.Size = new System.Drawing.Size(146, 22);
            this.tsmMyInfo.Text = "个人信息设置";
            this.tsmMyInfo.Click += new System.EventHandler(this.tsmMyInfo_Click);
            // 
            // tsmMsgMis
            // 
            this.tsmMsgMis.Name = "tsmMsgMis";
            this.tsmMsgMis.Size = new System.Drawing.Size(146, 22);
            this.tsmMsgMis.Text = "消息管理器";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(106, 25);
            this.toolStripStatusLabel2.Text = "www.OurMsg.net";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tButManageMsg,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 86);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(32, 334);
            this.toolStrip2.TabIndex = 26;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.Visible = false;
            // 
            // tButManageMsg
            // 
            this.tButManageMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tButManageMsg.Image = ((System.Drawing.Image)(resources.GetObject("tButManageMsg.Image")));
            this.tButManageMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButManageMsg.Name = "tButManageMsg";
            this.tButManageMsg.Size = new System.Drawing.Size(21, 20);
            this.tButManageMsg.Text = "toolStripButton4";
            this.tButManageMsg.ToolTipText = "信息管理器";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(21, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.controlPanel1);
            this.panelMain.Controls.Add(this.panelControlUp);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 69);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(219, 351);
            this.panelMain.TabIndex = 27;
            // 
            // panelControlUp
            // 
            this.panelControlUp.BackColor = System.Drawing.Color.White;
            this.panelControlUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlUp.Location = new System.Drawing.Point(0, 0);
            this.panelControlUp.Name = "panelControlUp";
            this.panelControlUp.Size = new System.Drawing.Size(219, 4);
            this.panelControlUp.TabIndex = 0;
            this.panelControlUp.Visible = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuNotify;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "OurMsg";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuNotify
            // 
            this.contextMenuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fsfsToolStripMenuItem,
            this.vcxvToolStripMenuItem,
            this.MenuItemGroupNotice,
            this.MenuItemOpenMain,
            this.vcxvToolStripMenuItem3,
            this.MenuItemClose,
            this.MenuItemExit});
            this.contextMenuNotify.Name = "cMenuNotify";
            this.contextMenuNotify.Size = new System.Drawing.Size(135, 126);
            this.contextMenuNotify.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuNotify_ItemClicked);
            // 
            // fsfsToolStripMenuItem
            // 
            this.fsfsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.联机ToolStripMenuItem,
            this.接听电话ToolStripMenuItem,
            this.外出就餐ToolStripMenuItem,
            this.暂时离开ToolStripMenuItem,
            this.工作中ToolStripMenuItem});
            this.fsfsToolStripMenuItem.Name = "fsfsToolStripMenuItem";
            this.fsfsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.fsfsToolStripMenuItem.Text = "我的状态";
            this.fsfsToolStripMenuItem.Visible = false;
            // 
            // 联机ToolStripMenuItem
            // 
            this.联机ToolStripMenuItem.Checked = true;
            this.联机ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.联机ToolStripMenuItem.Name = "联机ToolStripMenuItem";
            this.联机ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.联机ToolStripMenuItem.Text = "联机";
            // 
            // 接听电话ToolStripMenuItem
            // 
            this.接听电话ToolStripMenuItem.Name = "接听电话ToolStripMenuItem";
            this.接听电话ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.接听电话ToolStripMenuItem.Text = "接听电话";
            // 
            // 外出就餐ToolStripMenuItem
            // 
            this.外出就餐ToolStripMenuItem.Name = "外出就餐ToolStripMenuItem";
            this.外出就餐ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.外出就餐ToolStripMenuItem.Text = "外出就餐";
            // 
            // 暂时离开ToolStripMenuItem
            // 
            this.暂时离开ToolStripMenuItem.Name = "暂时离开ToolStripMenuItem";
            this.暂时离开ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.暂时离开ToolStripMenuItem.Text = "暂时离开";
            // 
            // 工作中ToolStripMenuItem
            // 
            this.工作中ToolStripMenuItem.Name = "工作中ToolStripMenuItem";
            this.工作中ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.工作中ToolStripMenuItem.Text = "工作中";
            // 
            // vcxvToolStripMenuItem
            // 
            this.vcxvToolStripMenuItem.Name = "vcxvToolStripMenuItem";
            this.vcxvToolStripMenuItem.Size = new System.Drawing.Size(131, 6);
            this.vcxvToolStripMenuItem.Visible = false;
            // 
            // MenuItemGroupNotice
            // 
            this.MenuItemGroupNotice.Name = "MenuItemGroupNotice";
            this.MenuItemGroupNotice.Size = new System.Drawing.Size(134, 22);
            this.MenuItemGroupNotice.Text = "群发消息";
            this.MenuItemGroupNotice.Visible = false;
            // 
            // MenuItemOpenMain
            // 
            this.MenuItemOpenMain.Name = "MenuItemOpenMain";
            this.MenuItemOpenMain.Size = new System.Drawing.Size(134, 22);
            this.MenuItemOpenMain.Text = "打开主面板";
            // 
            // vcxvToolStripMenuItem3
            // 
            this.vcxvToolStripMenuItem3.Name = "vcxvToolStripMenuItem3";
            this.vcxvToolStripMenuItem3.Size = new System.Drawing.Size(131, 6);
            // 
            // MenuItemClose
            // 
            this.MenuItemClose.Name = "MenuItemClose";
            this.MenuItemClose.Size = new System.Drawing.Size(134, 22);
            this.MenuItemClose.Text = "关闭";
            this.MenuItemClose.Visible = false;
            // 
            // MenuItemExit
            // 
            this.MenuItemExit.Name = "MenuItemExit";
            this.MenuItemExit.Size = new System.Drawing.Size(134, 22);
            this.MenuItemExit.Text = "退出程序";
            // 
            // userLoginPanel1
            // 
            this.userLoginPanel1.Location = new System.Drawing.Point(0, 0);
            this.userLoginPanel1.Name = "userLoginPanel1";
            this.userLoginPanel1.Size = new System.Drawing.Size(10, 10);
            this.userLoginPanel1.TabIndex = 28;
            this.userLoginPanel1.CancelLogin += new OurMsg.Controls.UserLoginPanel.CancleEventHandler(this.userLoginPanel1_CancelLogin);
            // 
            // controlPanel1
            // 
            this.controlPanel1.BackColor = System.Drawing.Color.Transparent;
            this.controlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel1.ForeColor = System.Drawing.Color.Transparent;
            this.controlPanel1.Location = new System.Drawing.Point(0, 4);
            this.controlPanel1.Name = "controlPanel1";
            this.controlPanel1.Size = new System.Drawing.Size(219, 347);
            this.controlPanel1.TabIndex = 29;
            // 
            // selfFaceImage1
            // 
            this.selfFaceImage1.BackColor = System.Drawing.SystemColors.Control;
            this.selfFaceImage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selfFaceImage1.Location = new System.Drawing.Point(11, 10);
            this.selfFaceImage1.Name = "selfFaceImage1";
            this.selfFaceImage1.Size = new System.Drawing.Size(42, 40);
            this.selfFaceImage1.TabIndex = 25;
            this.selfFaceImage1.ImageClick += new OurMsg.Controls.SelfFaceImage.EventHandler(this.selfFaceImage1_ImageClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(219, 450);
            this.Controls.Add(this.userLoginPanel1);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelReserch);
            this.Controls.Add(this.panelTop);
            this.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 450);
            this.Name = "FormMain";
            this.Text = "OurMsg";
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelReserch.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.contextMenuNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labUserName;
        private System.Windows.Forms.Panel panelReserch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tButManageMsg;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private IMLibrary3.Controls.ShowType ShowType1;
        private OurMsg.Controls.SelfFaceImage selfFaceImage1;
        private System.Windows.Forms.Panel panelMain;
        private OurMsg.Controls.ControlPanel controlPanel1;
        private System.Windows.Forms.Panel panelControlUp;
        private Controls.UserLoginPanel userLoginPanel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotify;
        private System.Windows.Forms.ToolStripMenuItem fsfsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 联机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 接听电话ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 外出就餐ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 暂时离开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工作中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator vcxvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemGroupNotice;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenMain;
        private System.Windows.Forms.ToolStripSeparator vcxvToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem MenuItemClose;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExit;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsmMsgMis;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmMyInfo;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
    }
}

