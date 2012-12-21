namespace OurMsg
{
    partial class FormDataManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataManage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButFirst = new System.Windows.Forms.ToolStripButton();
            this.ButUp = new System.Windows.Forms.ToolStripButton();
            this.TextBoxPage = new System.Windows.Forms.ToolStripTextBox();
            this.ButDown = new System.Windows.Forms.ToolStripButton();
            this.ButLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButDelRecord = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelGroupCount = new BSE.Windows.Forms.Panel();
            this.treeView_Organization = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panel1 = new BSE.Windows.Forms.Panel();
            this.txtRecord = new IMLibrary3.MyExtRichTextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelGroupCount.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButFirst,
            this.ButUp,
            this.TextBoxPage,
            this.ButDown,
            this.ButLast,
            this.toolStripSeparator1,
            this.ButDelRecord});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(763, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // ButFirst
            // 
            this.ButFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButFirst.Image = ((System.Drawing.Image)(resources.GetObject("ButFirst.Image")));
            this.ButFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButFirst.Name = "ButFirst";
            this.ButFirst.Size = new System.Drawing.Size(23, 22);
            this.ButFirst.Text = "第一页";
            this.ButFirst.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ButFirst.ToolTipText = "第一页";
            // 
            // ButUp
            // 
            this.ButUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButUp.Image = ((System.Drawing.Image)(resources.GetObject("ButUp.Image")));
            this.ButUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButUp.Name = "ButUp";
            this.ButUp.Size = new System.Drawing.Size(23, 22);
            this.ButUp.Text = "上一页";
            this.ButUp.ToolTipText = "上一页";
            // 
            // TextBoxPage
            // 
            this.TextBoxPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxPage.Name = "TextBoxPage";
            this.TextBoxPage.Size = new System.Drawing.Size(80, 25);
            this.TextBoxPage.Text = "0/0";
            this.TextBoxPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButDown
            // 
            this.ButDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButDown.Image = ((System.Drawing.Image)(resources.GetObject("ButDown.Image")));
            this.ButDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButDown.Name = "ButDown";
            this.ButDown.Size = new System.Drawing.Size(23, 22);
            this.ButDown.Text = "下一页";
            this.ButDown.ToolTipText = "下一页";
            // 
            // ButLast
            // 
            this.ButLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButLast.Image = ((System.Drawing.Image)(resources.GetObject("ButLast.Image")));
            this.ButLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButLast.Name = "ButLast";
            this.ButLast.Size = new System.Drawing.Size(23, 22);
            this.ButLast.Text = "最后一页";
            this.ButLast.ToolTipText = "最后一页";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButDelRecord
            // 
            this.ButDelRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButDelRecord.Image = ((System.Drawing.Image)(resources.GetObject("ButDelRecord.Image")));
            this.ButDelRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButDelRecord.Name = "ButDelRecord";
            this.ButDelRecord.Size = new System.Drawing.Size(23, 22);
            this.ButDelRecord.Text = "删除记录";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelContent);
            this.splitContainer1.Panel2.Controls.Add(this.panel6);
            this.splitContainer1.Panel2.Controls.Add(this.panel5);
            this.splitContainer1.Size = new System.Drawing.Size(763, 463);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelGroupCount);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(222, 463);
            this.panel2.TabIndex = 0;
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
            this.panelGroupCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGroupCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelGroupCount.Image = null;
            this.panelGroupCount.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelGroupCount.Location = new System.Drawing.Point(6, 0);
            this.panelGroupCount.MinimumSize = new System.Drawing.Size(22, 22);
            this.panelGroupCount.Name = "panelGroupCount";
            this.panelGroupCount.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelGroupCount.ShowExpandIcon = true;
            this.panelGroupCount.Size = new System.Drawing.Size(216, 457);
            this.panelGroupCount.TabIndex = 46;
            this.panelGroupCount.Text = "分组信息";
            this.panelGroupCount.ToolTipTextCloseIcon = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelGroupCount.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // treeView_Organization
            // 
            this.treeView_Organization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView_Organization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Organization.ImageIndex = 0;
            this.treeView_Organization.ImageList = this.imageList1;
            this.treeView_Organization.Location = new System.Drawing.Point(1, 23);
            this.treeView_Organization.Name = "treeView_Organization";
            this.treeView_Organization.SelectedImageIndex = 0;
            this.treeView_Organization.Size = new System.Drawing.Size(214, 433);
            this.treeView_Organization.TabIndex = 0;
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
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(6, 457);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(216, 6);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(6, 463);
            this.panel3.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panel1);
            this.panelContent.Controls.Add(this.listView1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(529, 457);
            this.panelContent.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AssociatedSplitter = null;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.panel1.CaptionHeight = 22;
            this.panel1.Controls.Add(this.txtRecord);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Image = null;
            this.panel1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(22, 22);
            this.panel1.Name = "panel1";
            this.panel1.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panel1.ShowExpandIcon = true;
            this.panel1.Size = new System.Drawing.Size(529, 457);
            this.panel1.TabIndex = 47;
            this.panel1.Text = "消息记录";
            this.panel1.ToolTipTextCloseIcon = null;
            this.panel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // txtRecord
            // 
            this.txtRecord.BackColor = System.Drawing.SystemColors.Window;
            this.txtRecord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRecord.ContextMenuStrip = this.contextMenuStrip;
            this.txtRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecord.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRecord.Location = new System.Drawing.Point(1, 23);
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.ReadOnly = true;
            this.txtRecord.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRecord.Size = new System.Drawing.Size(527, 433);
            this.txtRecord.TabIndex = 46;
            this.txtRecord.Text = "";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemCopy,
            this.toolStripSeparator2,
            this.MenuItemSelAll});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(117, 54);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // MenuItemCopy
            // 
            this.MenuItemCopy.Name = "MenuItemCopy";
            this.MenuItemCopy.Size = new System.Drawing.Size(116, 22);
            this.MenuItemCopy.Text = "复制(&C)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(113, 6);
            // 
            // MenuItemSelAll
            // 
            this.MenuItemSelAll.Name = "MenuItemSelAll";
            this.MenuItemSelAll.Size = new System.Drawing.Size(116, 22);
            this.MenuItemSelAll.Text = "全选(&A)";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(529, 457);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "项目";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "内容";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "备注";
            this.columnHeader3.Width = 200;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 457);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(529, 6);
            this.panel6.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(529, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(6, 463);
            this.panel5.TabIndex = 1;
            // 
            // FormDataManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 488);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息管理器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormDataManage_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelGroupCount.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton ButFirst;
        private System.Windows.Forms.ToolStripButton ButUp;
        private System.Windows.Forms.ToolStripButton ButDown;
        private System.Windows.Forms.ToolStripButton ButLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButDelRecord;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSelAll;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripTextBox TextBoxPage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.ImageList imageList1;
        private BSE.Windows.Forms.Panel panelGroupCount;
        private System.Windows.Forms.Panel panelContent;
        private BSE.Windows.Forms.Panel panel1;
        public IMLibrary3.MyExtRichTextBox txtRecord;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TreeView treeView_Organization;

    }
}