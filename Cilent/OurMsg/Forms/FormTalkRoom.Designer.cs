namespace OurMsg
{
    partial class FormTalkRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTalkRoom));
            this.panelButtom = new System.Windows.Forms.Panel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButSendFile = new System.Windows.Forms.ToolStripButton();
            this.tsButAV = new System.Windows.Forms.ToolStripButton();
            this.tsButSetGroupData = new System.Windows.Forms.ToolStripButton();
            this.panelTool = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelGroupCount = new BSE.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new BSE.Windows.Forms.Panel();
            this.txtNotice = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelSplit = new System.Windows.Forms.Panel();
            this.panelSendMsgBut = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.butClose = new System.Windows.Forms.Button();
            this.butSend = new System.Windows.Forms.Button();
            this.MessagePanel1 = new OurMsg.Controls.MessagePanel();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panelTool.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelGroupCount.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelSendMsgBut.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtom
            // 
            this.panelButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtom.Location = new System.Drawing.Point(4, 418);
            this.panelButtom.Name = "panelButtom";
            this.panelButtom.Size = new System.Drawing.Size(550, 4);
            this.panelButtom.TabIndex = 36;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel2.LinkColor = System.Drawing.Color.DarkRed;
            this.linkLabel2.Location = new System.Drawing.Point(3, 5);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(49, 19);
            this.linkLabel2.TabIndex = 0;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "广告";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.linkLabel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(385, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(165, 30);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButSendFile,
            this.tsButAV,
            this.tsButSetGroupData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(385, 31);
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
            this.tsButSendFile.Visible = false;
            // 
            // tsButAV
            // 
            this.tsButAV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButAV.Image = ((System.Drawing.Image)(resources.GetObject("tsButAV.Image")));
            this.tsButAV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButAV.Name = "tsButAV";
            this.tsButAV.Size = new System.Drawing.Size(28, 28);
            this.tsButAV.Text = "视频对话";
            this.tsButAV.Visible = false;
            // 
            // tsButSetGroupData
            // 
            this.tsButSetGroupData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButSetGroupData.Image = ((System.Drawing.Image)(resources.GetObject("tsButSetGroupData.Image")));
            this.tsButSetGroupData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButSetGroupData.Name = "tsButSetGroupData";
            this.tsButSetGroupData.Size = new System.Drawing.Size(28, 28);
            this.tsButSetGroupData.Click += new System.EventHandler(this.tsButSetGroupData_Click);
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.toolStrip1);
            this.panelTool.Controls.Add(this.panel2);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTool.Location = new System.Drawing.Point(4, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(550, 30);
            this.panelTool.TabIndex = 38;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(4, 422);
            this.panelLeft.TabIndex = 37;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.panelGroupCount);
            this.panelRight.Controls.Add(this.panel4);
            this.panelRight.Controls.Add(this.panel1);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(390, 30);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(164, 388);
            this.panelRight.TabIndex = 46;
            // 
            // panelGroupCount
            // 
            this.panelGroupCount.AssociatedSplitter = null;
            this.panelGroupCount.BackColor = System.Drawing.Color.Transparent;
            this.panelGroupCount.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panelGroupCount.CaptionHeight = 22;
            this.panelGroupCount.Controls.Add(this.listView1);
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
            this.panelGroupCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGroupCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.Image = null;
            this.panelGroupCount.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelGroupCount.Location = new System.Drawing.Point(0, 163);
            this.panelGroupCount.MinimumSize = new System.Drawing.Size(22, 22);
            this.panelGroupCount.Name = "panelGroupCount";
            this.panelGroupCount.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelGroupCount.ShowExpandIcon = true;
            this.panelGroupCount.Size = new System.Drawing.Size(164, 225);
            this.panelGroupCount.TabIndex = 45;
            this.panelGroupCount.Text = "成员(0/0)";
            this.panelGroupCount.ToolTipTextCloseIcon = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelExpanded = null;
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
            this.listView1.Size = new System.Drawing.Size(162, 201);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 140;
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
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 159);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(164, 4);
            this.panel4.TabIndex = 44;
            // 
            // panel1
            // 
            this.panel1.AssociatedSplitter = null;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panel1.CaptionHeight = 22;
            this.panel1.Controls.Add(this.txtNotice);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.CustomColors.BorderColor = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.CaptionGradientMiddle = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.CaptionSelectedGradientBegin = System.Drawing.SystemColors.Window;
            this.panel1.CustomColors.CaptionSelectedGradientEnd = System.Drawing.SystemColors.Window;
            this.panel1.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.ContentGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Image = ((System.Drawing.Image)(resources.GetObject("panel1.Image")));
            this.panel1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(22, 22);
            this.panel1.Name = "panel1";
            this.panel1.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panel1.ShowExpandIcon = true;
            this.panel1.Size = new System.Drawing.Size(164, 159);
            this.panel1.TabIndex = 43;
            this.panel1.Text = "动态";
            this.panel1.ToolTipTextCloseIcon = null;
            this.panel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // txtNotice
            // 
            this.txtNotice.BackColor = System.Drawing.SystemColors.Control;
            this.txtNotice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotice.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtNotice.Location = new System.Drawing.Point(6, 28);
            this.txtNotice.Multiline = true;
            this.txtNotice.Name = "txtNotice";
            this.txtNotice.ReadOnly = true;
            this.txtNotice.Size = new System.Drawing.Size(157, 130);
            this.txtNotice.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(1, 28);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(5, 130);
            this.panel6.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(1, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 5);
            this.panel3.TabIndex = 1;
            // 
            // panelSplit
            // 
            this.panelSplit.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSplit.Location = new System.Drawing.Point(386, 30);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Size = new System.Drawing.Size(4, 388);
            this.panelSplit.TabIndex = 47;
            // 
            // panelSendMsgBut
            // 
            this.panelSendMsgBut.Controls.Add(this.linkLabel1);
            this.panelSendMsgBut.Controls.Add(this.butClose);
            this.panelSendMsgBut.Controls.Add(this.butSend);
            this.panelSendMsgBut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSendMsgBut.Location = new System.Drawing.Point(4, 390);
            this.panelSendMsgBut.Name = "panelSendMsgBut";
            this.panelSendMsgBut.Size = new System.Drawing.Size(382, 28);
            this.panelSendMsgBut.TabIndex = 49;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.linkLabel1.Location = new System.Drawing.Point(3, 8);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(125, 12);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "www.ourmsg.net";
            this.linkLabel1.Text = "点击此处查看每日要闻";
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Location = new System.Drawing.Point(238, 3);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(70, 25);
            this.butClose.TabIndex = 1;
            this.butClose.Text = "关闭(&C)";
            this.butClose.UseVisualStyleBackColor = true;
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSend.Location = new System.Drawing.Point(313, 3);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(70, 25);
            this.butSend.TabIndex = 0;
            this.butSend.Text = "发送(&S)";
            this.butSend.UseVisualStyleBackColor = true;
            // 
            // MessagePanel1
            // 
            this.MessagePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagePanel1.Location = new System.Drawing.Point(4, 30);
            this.MessagePanel1.Name = "MessagePanel1";
            this.MessagePanel1.Size = new System.Drawing.Size(382, 360);
            this.MessagePanel1.TabIndex = 50;
            // 
            // FormTalkRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 422);
            this.Controls.Add(this.MessagePanel1);
            this.Controls.Add(this.panelSendMsgBut);
            this.Controls.Add(this.panelSplit);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelButtom);
            this.Controls.Add(this.panelTool);
            this.Controls.Add(this.panelLeft);
            this.MinimumSize = new System.Drawing.Size(570, 460);
            this.Name = "FormTalkRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "与某某组对话";
            this.Load += new System.EventHandler(this.FormGroupChat_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTool.ResumeLayout(false);
            this.panelTool.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelGroupCount.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelSendMsgBut.ResumeLayout(false);
            this.panelSendMsgBut.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtom;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsButSendFile;
        private System.Windows.Forms.ToolStripButton tsButAV;
        private System.Windows.Forms.Panel panelTool;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.ToolStripButton tsButSetGroupData;
        private System.Windows.Forms.Panel panelRight;
        private BSE.Windows.Forms.Panel panelGroupCount;
        private System.Windows.Forms.Panel panel4;
        private BSE.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelSplit;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panelSendMsgBut;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butSend;
        private Controls.MessagePanel MessagePanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtNotice;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.ImageList imageList1;
    }
}