namespace OurMsg.Controls
{
    partial class MessagePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagePanel));
            this.panelLeftManMsg = new System.Windows.Forms.Panel();
            this.panel3 = new BSE.Windows.Forms.Panel();
            this.txtRecord = new IMLibrary3.MyExtRichTextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemCopy2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSelAll2 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelSendMsg = new System.Windows.Forms.Panel();
            this.panelTxtSend = new System.Windows.Forms.Panel();
            this.txtSend = new IMLibrary3.MyExtRichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemPaset = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsButSetFont = new System.Windows.Forms.ToolStripButton();
            this.tsButColor = new System.Windows.Forms.ToolStripButton();
            this.tsButFace = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsButCaptureImageTool = new System.Windows.Forms.ToolStripButton();
            this.panelLeftManMsg.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panelSendMsg.SuspendLayout();
            this.panelTxtSend.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeftManMsg
            // 
            this.panelLeftManMsg.Controls.Add(this.panel3);
            this.panelLeftManMsg.Controls.Add(this.splitter1);
            this.panelLeftManMsg.Controls.Add(this.panelSendMsg);
            this.panelLeftManMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftManMsg.Location = new System.Drawing.Point(0, 0);
            this.panelLeftManMsg.Name = "panelLeftManMsg";
            this.panelLeftManMsg.Size = new System.Drawing.Size(268, 245);
            this.panelLeftManMsg.TabIndex = 49;
            // 
            // panel3
            // 
            this.panel3.AssociatedSplitter = null;
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panel3.CaptionHeight = 22;
            this.panel3.Controls.Add(this.txtRecord);
            this.panel3.CustomColors.BorderColor = System.Drawing.SystemColors.ControlText;
            this.panel3.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel3.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel3.CustomColors.CaptionGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel3.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel3.CustomColors.CaptionGradientMiddle = System.Drawing.SystemColors.ButtonFace;
            this.panel3.CustomColors.CaptionSelectedGradientBegin = System.Drawing.SystemColors.Window;
            this.panel3.CustomColors.CaptionSelectedGradientEnd = System.Drawing.SystemColors.Window;
            this.panel3.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel3.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel3.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel3.CustomColors.ContentGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel3.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel3.Image = null;
            this.panel3.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.MinimumSize = new System.Drawing.Size(22, 22);
            this.panel3.Name = "panel3";
            this.panel3.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panel3.ShowExpandIcon = true;
            this.panel3.Size = new System.Drawing.Size(268, 119);
            this.panel3.TabIndex = 29;
            this.panel3.Text = "对话记录";
            this.panel3.ToolTipTextCloseIcon = null;
            this.panel3.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel3.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // txtRecord
            // 
            this.txtRecord.BackColor = System.Drawing.SystemColors.Window;
            this.txtRecord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRecord.ContextMenuStrip = this.contextMenuStrip2;
            this.txtRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecord.Location = new System.Drawing.Point(1, 23);
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.ReadOnly = true;
            this.txtRecord.Size = new System.Drawing.Size(266, 95);
            this.txtRecord.TabIndex = 0;
            this.txtRecord.Text = "";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemCopy2,
            this.toolStripSeparator1,
            this.MenuItemSelAll2});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(117, 54);
            // 
            // MenuItemCopy2
            // 
            this.MenuItemCopy2.Name = "MenuItemCopy2";
            this.MenuItemCopy2.Size = new System.Drawing.Size(116, 22);
            this.MenuItemCopy2.Text = "复制(&C)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // MenuItemSelAll2
            // 
            this.MenuItemSelAll2.Name = "MenuItemSelAll2";
            this.MenuItemSelAll2.Size = new System.Drawing.Size(116, 22);
            this.MenuItemSelAll2.Text = "全选(&A)";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 119);
            this.splitter1.MinExtra = 100;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(268, 1);
            this.splitter1.TabIndex = 28;
            this.splitter1.TabStop = false;
            // 
            // panelSendMsg
            // 
            this.panelSendMsg.Controls.Add(this.panelTxtSend);
            this.panelSendMsg.Controls.Add(this.toolStrip2);
            this.panelSendMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSendMsg.Location = new System.Drawing.Point(0, 120);
            this.panelSendMsg.Name = "panelSendMsg";
            this.panelSendMsg.Size = new System.Drawing.Size(268, 125);
            this.panelSendMsg.TabIndex = 18;
            // 
            // panelTxtSend
            // 
            this.panelTxtSend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTxtSend.Controls.Add(this.txtSend);
            this.panelTxtSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTxtSend.Location = new System.Drawing.Point(0, 25);
            this.panelTxtSend.Name = "panelTxtSend";
            this.panelTxtSend.Size = new System.Drawing.Size(268, 100);
            this.panelTxtSend.TabIndex = 22;
            // 
            // txtSend
            // 
            this.txtSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSend.ContextMenuStrip = this.contextMenuStrip1;
            this.txtSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSend.Location = new System.Drawing.Point(0, 0);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(266, 98);
            this.txtSend.TabIndex = 23;
            this.txtSend.Text = "";
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
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButSetFont,
            this.tsButColor,
            this.tsButFace,
            this.tsButCaptureImageTool});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(268, 25);
            this.toolStrip2.TabIndex = 21;
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
            // 
            // MessagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelLeftManMsg);
            this.Name = "MessagePanel";
            this.Size = new System.Drawing.Size(268, 245);
            this.panelLeftManMsg.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.panelSendMsg.ResumeLayout(false);
            this.panelSendMsg.PerformLayout();
            this.panelTxtSend.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeftManMsg;
        private BSE.Windows.Forms.Panel panel3;
        private IMLibrary3.MyExtRichTextBox txtRecord;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelSendMsg;
        private System.Windows.Forms.Panel panelTxtSend;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsButSetFont;
        private System.Windows.Forms.ToolStripButton tsButColor;
        private System.Windows.Forms.ToolStripDropDownButton tsButFace;
        private System.Windows.Forms.ToolStripButton tsButCaptureImageTool;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSelAll2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem MenuItemPaset;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCut;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSelAll;
        public IMLibrary3.MyExtRichTextBox txtSend;
    }
}
