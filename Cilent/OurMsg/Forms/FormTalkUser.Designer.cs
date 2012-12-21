namespace OurMsg
{
    partial class FormTalkUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTalkUser));
            this.panelButtom = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelTool = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButSendFile = new System.Windows.Forms.ToolStripButton();
            this.tsButAV = new System.Windows.Forms.ToolStripButton();
            this.ButUserData = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.panelDynamicInfo = new BSE.Windows.Forms.Panel();
            this.xPanderPanelList1 = new BSE.Windows.Forms.XPanderPanelList();
            this.xPanderPanel1 = new BSE.Windows.Forms.XPanderPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.xPanderPanel2 = new BSE.Windows.Forms.XPanderPanel();
            this.video1 = new OurMsg.Controls.video();
            this.panelSplit = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelSendMsgBut = new System.Windows.Forms.Panel();
            this.butClose = new System.Windows.Forms.Button();
            this.butSend = new System.Windows.Forms.Button();
            this.MessagePanel1 = new OurMsg.Controls.MessagePanel();
            this.panelTool.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelDynamicInfo.SuspendLayout();
            this.xPanderPanelList1.SuspendLayout();
            this.xPanderPanel1.SuspendLayout();
            this.xPanderPanel2.SuspendLayout();
            this.panelSendMsgBut.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtom
            // 
            this.panelButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtom.Location = new System.Drawing.Point(0, 418);
            this.panelButtom.Name = "panelButtom";
            this.panelButtom.Size = new System.Drawing.Size(554, 4);
            this.panelButtom.TabIndex = 14;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(4, 418);
            this.panelLeft.TabIndex = 30;
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.toolStrip1);
            this.panelTool.Controls.Add(this.panel2);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTool.Location = new System.Drawing.Point(4, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(550, 30);
            this.panelTool.TabIndex = 32;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButSendFile,
            this.tsButAV,
            this.ButUserData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(401, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsButSendFile
            // 
            this.tsButSendFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButSendFile.Image = ((System.Drawing.Image)(resources.GetObject("tsButSendFile.Image")));
            this.tsButSendFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButSendFile.Name = "tsButSendFile";
            this.tsButSendFile.Size = new System.Drawing.Size(28, 28);
            this.tsButSendFile.Text = "发送文件 ";
            // 
            // tsButAV
            // 
            this.tsButAV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButAV.Image = ((System.Drawing.Image)(resources.GetObject("tsButAV.Image")));
            this.tsButAV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButAV.Name = "tsButAV";
            this.tsButAV.Size = new System.Drawing.Size(28, 28);
            this.tsButAV.Text = "视频对话";
            // 
            // ButUserData
            // 
            this.ButUserData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButUserData.Image = ((System.Drawing.Image)(resources.GetObject("ButUserData.Image")));
            this.ButUserData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButUserData.Name = "ButUserData";
            this.ButUserData.Size = new System.Drawing.Size(28, 28);
            this.ButUserData.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.linkLabel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(401, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 30);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel2.LinkColor = System.Drawing.Color.DarkRed;
            this.linkLabel2.Location = new System.Drawing.Point(3, 5);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(89, 19);
            this.linkLabel2.TabIndex = 0;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "广告招商";
            // 
            // panelDynamicInfo
            // 
            this.panelDynamicInfo.AssociatedSplitter = null;
            this.panelDynamicInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelDynamicInfo.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panelDynamicInfo.CaptionHeight = 22;
            this.panelDynamicInfo.Controls.Add(this.xPanderPanelList1);
            this.panelDynamicInfo.CustomColors.BorderColor = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.CustomColors.CaptionGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelDynamicInfo.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panelDynamicInfo.CustomColors.CaptionGradientMiddle = System.Drawing.SystemColors.ButtonFace;
            this.panelDynamicInfo.CustomColors.CaptionSelectedGradientBegin = System.Drawing.SystemColors.Window;
            this.panelDynamicInfo.CustomColors.CaptionSelectedGradientEnd = System.Drawing.SystemColors.Window;
            this.panelDynamicInfo.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelDynamicInfo.CustomColors.ContentGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panelDynamicInfo.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panelDynamicInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDynamicInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelDynamicInfo.Image = ((System.Drawing.Image)(resources.GetObject("panelDynamicInfo.Image")));
            this.panelDynamicInfo.Location = new System.Drawing.Point(405, 30);
            this.panelDynamicInfo.MinimumSize = new System.Drawing.Size(22, 22);
            this.panelDynamicInfo.Name = "panelDynamicInfo";
            this.panelDynamicInfo.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelDynamicInfo.ShowExpandIcon = true;
            this.panelDynamicInfo.Size = new System.Drawing.Size(149, 388);
            this.panelDynamicInfo.TabIndex = 33;
            this.panelDynamicInfo.Text = "动态信息";
            this.panelDynamicInfo.ToolTipTextCloseIcon = null;
            this.panelDynamicInfo.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelDynamicInfo.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // xPanderPanelList1
            // 
            this.xPanderPanelList1.CaptionStyle = BSE.Windows.Forms.CaptionStyle.Normal;
            this.xPanderPanelList1.Controls.Add(this.xPanderPanel1);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanel2);
            this.xPanderPanelList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPanderPanelList1.GradientBackground = System.Drawing.Color.Empty;
            this.xPanderPanelList1.Location = new System.Drawing.Point(0, 23);
            this.xPanderPanelList1.Name = "xPanderPanelList1";
            this.xPanderPanelList1.PanelColors = null;
            this.xPanderPanelList1.Size = new System.Drawing.Size(149, 364);
            this.xPanderPanelList1.TabIndex = 3;
            this.xPanderPanelList1.Text = "xPanderPanelList1";
            // 
            // xPanderPanel1
            // 
            this.xPanderPanel1.CaptionFont = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanel1.Controls.Add(this.flowLayoutPanel1);
            this.xPanderPanel1.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanel1.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel1.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanel1.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel1.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel1.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.Image = null;
            this.xPanderPanel1.Name = "xPanderPanel1";
            this.xPanderPanel1.Size = new System.Drawing.Size(149, 25);
            this.xPanderPanel1.TabIndex = 0;
            this.xPanderPanel1.Text = "文件传输";
            this.xPanderPanel1.ToolTipTextCloseIcon = null;
            this.xPanderPanel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanel1.ToolTipTextExpandIconPanelExpanded = null;
            this.xPanderPanel1.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FloralWhite;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 25);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(147, 0);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // xPanderPanel2
            // 
            this.xPanderPanel2.CaptionFont = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanel2.Controls.Add(this.video1);
            this.xPanderPanel2.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanel2.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel2.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanel2.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel2.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel2.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanel2.Expand = true;
            this.xPanderPanel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.Image = null;
            this.xPanderPanel2.Name = "xPanderPanel2";
            this.xPanderPanel2.Size = new System.Drawing.Size(149, 339);
            this.xPanderPanel2.TabIndex = 1;
            this.xPanderPanel2.Text = "视频对话";
            this.xPanderPanel2.ToolTipTextCloseIcon = null;
            this.xPanderPanel2.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanel2.ToolTipTextExpandIconPanelExpanded = null;
            this.xPanderPanel2.Visible = false;
            // 
            // video1
            // 
            this.video1.BackColor = System.Drawing.Color.White;
            this.video1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.video1.Location = new System.Drawing.Point(1, 25);
            this.video1.Name = "video1";
            this.video1.Size = new System.Drawing.Size(147, 314);
            this.video1.TabIndex = 0;
            // 
            // panelSplit
            // 
            this.panelSplit.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSplit.Location = new System.Drawing.Point(401, 30);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Size = new System.Drawing.Size(4, 388);
            this.panelSplit.TabIndex = 34;
            // 
            // panelSendMsgBut
            // 
            this.panelSendMsgBut.Controls.Add(this.butClose);
            this.panelSendMsgBut.Controls.Add(this.butSend);
            this.panelSendMsgBut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSendMsgBut.Location = new System.Drawing.Point(4, 390);
            this.panelSendMsgBut.Name = "panelSendMsgBut";
            this.panelSendMsgBut.Size = new System.Drawing.Size(397, 28);
            this.panelSendMsgBut.TabIndex = 36;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Location = new System.Drawing.Point(253, 3);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(70, 25);
            this.butClose.TabIndex = 1;
            this.butClose.Text = "关闭(&C)";
            this.butClose.UseVisualStyleBackColor = true;
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSend.Location = new System.Drawing.Point(328, 3);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(70, 25);
            this.butSend.TabIndex = 0;
            this.butSend.Text = "发送(&S)";
            this.butSend.UseVisualStyleBackColor = true;
            // 
            // MessagePanel1
            // 
            this.MessagePanel1.AllowDrop = true;
            this.MessagePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagePanel1.Location = new System.Drawing.Point(4, 30);
            this.MessagePanel1.Name = "MessagePanel1";
            this.MessagePanel1.Size = new System.Drawing.Size(397, 360);
            this.MessagePanel1.TabIndex = 37;
            // 
            // FormTalkUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 422);
            this.Controls.Add(this.MessagePanel1);
            this.Controls.Add(this.panelSendMsgBut);
            this.Controls.Add(this.panelSplit);
            this.Controls.Add(this.panelDynamicInfo);
            this.Controls.Add(this.panelTool);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelButtom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(570, 460);
            this.Name = "FormTalkUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "与某某(10000000)对话";
            this.Load += new System.EventHandler(this.FormTalkUser_Load);
            this.panelTool.ResumeLayout(false);
            this.panelTool.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelDynamicInfo.ResumeLayout(false);
            this.xPanderPanelList1.ResumeLayout(false);
            this.xPanderPanel1.ResumeLayout(false);
            this.xPanderPanel2.ResumeLayout(false);
            this.panelSendMsgBut.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtom;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelTool;
        private BSE.Windows.Forms.Panel panelDynamicInfo;
        private System.Windows.Forms.Panel panelSplit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsButSendFile;
        private System.Windows.Forms.ToolStripButton tsButAV;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelSendMsgBut;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butSend;
        private Controls.MessagePanel MessagePanel1;
        private System.Windows.Forms.ToolStripButton ButUserData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private BSE.Windows.Forms.XPanderPanelList xPanderPanelList1;
        private BSE.Windows.Forms.XPanderPanel xPanderPanel1;
        private BSE.Windows.Forms.XPanderPanel xPanderPanel2;
        private Controls.video video1;
    }
}