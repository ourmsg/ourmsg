namespace OurMsg
{
    partial class FormSendNotice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSendNotice));
            this.panelSendMsgBut = new System.Windows.Forms.Panel();
            this.butClose = new System.Windows.Forms.Button();
            this.butSend = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemPaset = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelMsg = new System.Windows.Forms.Panel();
            this.txtSend = new IMLibrary3.MyExtRichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsButSetFont = new System.Windows.Forms.ToolStripButton();
            this.tsButColor = new System.Windows.Forms.ToolStripButton();
            this.tsButFace = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsButCaptureImageTool = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panelRemark = new System.Windows.Forms.Panel();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.paneltitleText = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.panelSendMsgBut.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelMsg.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panelRemark.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.paneltitleText.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSendMsgBut
            // 
            this.panelSendMsgBut.Controls.Add(this.butClose);
            this.panelSendMsgBut.Controls.Add(this.butSend);
            this.panelSendMsgBut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSendMsgBut.Location = new System.Drawing.Point(0, 424);
            this.panelSendMsgBut.Name = "panelSendMsgBut";
            this.panelSendMsgBut.Size = new System.Drawing.Size(595, 31);
            this.panelSendMsgBut.TabIndex = 23;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Location = new System.Drawing.Point(445, 3);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(70, 25);
            this.butClose.TabIndex = 1;
            this.butClose.Text = "关闭(&C)";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSend.Location = new System.Drawing.Point(520, 3);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(70, 25);
            this.butSend.TabIndex = 0;
            this.butSend.Text = "发送(&S)";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemCopy,
            this.MenuItemPaset,
            this.MenuItemCut,
            this.MenuItemDel,
            this.toolStripMenuItem1,
            this.MenuItemSelAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 120);
            // 
            // MenuItemCopy
            // 
            this.MenuItemCopy.Name = "MenuItemCopy";
            this.MenuItemCopy.Size = new System.Drawing.Size(117, 22);
            this.MenuItemCopy.Text = "复制(&C)";
            // 
            // MenuItemPaset
            // 
            this.MenuItemPaset.Name = "MenuItemPaset";
            this.MenuItemPaset.Size = new System.Drawing.Size(117, 22);
            this.MenuItemPaset.Text = "粘贴(&P)";
            // 
            // MenuItemCut
            // 
            this.MenuItemCut.Name = "MenuItemCut";
            this.MenuItemCut.Size = new System.Drawing.Size(117, 22);
            this.MenuItemCut.Text = "剪切(&T)";
            // 
            // MenuItemDel
            // 
            this.MenuItemDel.Name = "MenuItemDel";
            this.MenuItemDel.Size = new System.Drawing.Size(117, 22);
            this.MenuItemDel.Text = "删除(&D)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(114, 6);
            // 
            // MenuItemSelAll
            // 
            this.MenuItemSelAll.Name = "MenuItemSelAll";
            this.MenuItemSelAll.Size = new System.Drawing.Size(117, 22);
            this.MenuItemSelAll.Text = "全选(&A)";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panelContent);
            this.panel1.Controls.Add(this.panelRemark);
            this.panel1.Controls.Add(this.panelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(595, 424);
            this.panel1.TabIndex = 25;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panelMsg);
            this.panelContent.Controls.Add(this.panel10);
            this.panelContent.Controls.Add(this.panel9);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 80);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(593, 281);
            this.panelContent.TabIndex = 30;
            // 
            // panelMsg
            // 
            this.panelMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMsg.Controls.Add(this.txtSend);
            this.panelMsg.Controls.Add(this.toolStrip2);
            this.panelMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMsg.Location = new System.Drawing.Point(13, 0);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Size = new System.Drawing.Size(567, 281);
            this.panelMsg.TabIndex = 33;
            // 
            // txtSend
            // 
            this.txtSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSend.ContextMenuStrip = this.contextMenuStrip1;
            this.txtSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSend.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSend.Location = new System.Drawing.Point(0, 25);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(565, 254);
            this.txtSend.TabIndex = 27;
            this.txtSend.Text = "";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButSetFont,
            this.tsButColor,
            this.tsButFace,
            this.tsButCaptureImageTool,
            this.toolStripLabel1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(565, 25);
            this.toolStrip2.TabIndex = 26;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsButSetFont
            // 
            this.tsButSetFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButSetFont.Image = ((System.Drawing.Image)(resources.GetObject("tsButSetFont.Image")));
            this.tsButSetFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButSetFont.Name = "tsButSetFont";
            this.tsButSetFont.Size = new System.Drawing.Size(23, 22);
            this.tsButSetFont.Text = "字体设置";
            // 
            // tsButColor
            // 
            this.tsButColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButColor.Image = ((System.Drawing.Image)(resources.GetObject("tsButColor.Image")));
            this.tsButColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButColor.Name = "tsButColor";
            this.tsButColor.Size = new System.Drawing.Size(23, 22);
            this.tsButColor.Text = "字体颜色";
            // 
            // tsButFace
            // 
            this.tsButFace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButFace.Image = ((System.Drawing.Image)(resources.GetObject("tsButFace.Image")));
            this.tsButFace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButFace.Name = "tsButFace";
            this.tsButFace.ShowDropDownArrow = false;
            this.tsButFace.Size = new System.Drawing.Size(20, 22);
            this.tsButFace.Text = "插入表情";
            this.tsButFace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tsButCaptureImageTool
            // 
            this.tsButCaptureImageTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButCaptureImageTool.Image = ((System.Drawing.Image)(resources.GetObject("tsButCaptureImageTool.Image")));
            this.tsButCaptureImageTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButCaptureImageTool.Name = "tsButCaptureImageTool";
            this.tsButCaptureImageTool.Size = new System.Drawing.Size(23, 22);
            this.tsButCaptureImageTool.Text = "插入截图";
            this.tsButCaptureImageTool.Visible = false;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(128, 22);
            this.toolStripLabel1.Text = "【以下输入通知内容】";
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(580, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(13, 281);
            this.panel10.TabIndex = 32;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(13, 281);
            this.panel9.TabIndex = 31;
            // 
            // panelRemark
            // 
            this.panelRemark.Controls.Add(this.txtRemark);
            this.panelRemark.Controls.Add(this.panel3);
            this.panelRemark.Controls.Add(this.panel6);
            this.panelRemark.Controls.Add(this.panel7);
            this.panelRemark.Controls.Add(this.panel8);
            this.panelRemark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRemark.Location = new System.Drawing.Point(0, 361);
            this.panelRemark.Name = "panelRemark";
            this.panelRemark.Size = new System.Drawing.Size(593, 61);
            this.panelRemark.TabIndex = 28;
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.paneltitleText);
            this.panelTitle.Controls.Add(this.panel2);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(593, 80);
            this.panelTitle.TabIndex = 25;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(593, 10);
            this.panel2.TabIndex = 0;
            // 
            // paneltitleText
            // 
            this.paneltitleText.Controls.Add(this.txtTitle);
            this.paneltitleText.Controls.Add(this.panel14);
            this.paneltitleText.Controls.Add(this.panel13);
            this.paneltitleText.Controls.Add(this.panel12);
            this.paneltitleText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paneltitleText.Location = new System.Drawing.Point(0, 10);
            this.paneltitleText.Name = "paneltitleText";
            this.paneltitleText.Size = new System.Drawing.Size(593, 70);
            this.paneltitleText.TabIndex = 3;
            // 
            // panel12
            // 
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(13, 70);
            this.panel12.TabIndex = 2;
            // 
            // panel13
            // 
            this.panel13.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel13.Location = new System.Drawing.Point(580, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(13, 70);
            this.panel13.TabIndex = 3;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel14.Location = new System.Drawing.Point(13, 60);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(567, 10);
            this.panel14.TabIndex = 4;
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTitle.Location = new System.Drawing.Point(13, 0);
            this.txtTitle.MaxLength = 100;
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(567, 60);
            this.txtTitle.TabIndex = 28;
            this.txtTitle.Text = "通  知";
            this.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(13, 51);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(567, 10);
            this.panel6.TabIndex = 36;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(13, 61);
            this.panel7.TabIndex = 35;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(580, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(13, 61);
            this.panel8.TabIndex = 34;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(13, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(567, 10);
            this.panel3.TabIndex = 38;
            // 
            // txtRemark
            // 
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.Location = new System.Drawing.Point(13, 10);
            this.txtRemark.MaxLength = 100;
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(567, 41);
            this.txtRemark.TabIndex = 39;
            this.txtRemark.Text = "公司";
            this.txtRemark.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FormSendNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 455);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelSendMsgBut);
            this.Name = "FormSendNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发送通知消息";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormSendNotice_Load);
            this.panelSendMsgBut.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelMsg.ResumeLayout(false);
            this.panelMsg.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panelRemark.ResumeLayout(false);
            this.panelRemark.PerformLayout();
            this.panelTitle.ResumeLayout(false);
            this.paneltitleText.ResumeLayout(false);
            this.paneltitleText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSendMsgBut;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem MenuItemPaset;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCut;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSelAll;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Panel panelRemark;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelMsg;
        private IMLibrary3.MyExtRichTextBox txtSend;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsButSetFont;
        private System.Windows.Forms.ToolStripButton tsButColor;
        private System.Windows.Forms.ToolStripDropDownButton tsButFace;
        private System.Windows.Forms.ToolStripButton tsButCaptureImageTool;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel paneltitleText;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Panel panel3;
    }
}