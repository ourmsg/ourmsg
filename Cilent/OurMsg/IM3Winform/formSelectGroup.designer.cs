namespace IMLibrary3 
{
    partial class formSelectGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formSelectGroup));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tButAddGroup = new System.Windows.Forms.ToolStripButton();
            this.tButDelGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tButGroupsRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.LabTitle = new System.Windows.Forms.ToolStripLabel();
            this.tsbOK = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LabelSelectGroup = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView_Organization = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tButAddGroup,
            this.tButDelGroup,
            this.toolStripSeparator1,
            this.tButGroupsRefresh,
            this.toolStripSeparator2,
            this.LabTitle,
            this.tsbOK});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(418, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tButAddGroup
            // 
            this.tButAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tButAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("tButAddGroup.Image")));
            this.tButAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButAddGroup.Name = "tButAddGroup";
            this.tButAddGroup.Size = new System.Drawing.Size(23, 22);
            this.tButAddGroup.Text = "添加组";
            // 
            // tButDelGroup
            // 
            this.tButDelGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tButDelGroup.Image = ((System.Drawing.Image)(resources.GetObject("tButDelGroup.Image")));
            this.tButDelGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButDelGroup.Name = "tButDelGroup";
            this.tButDelGroup.Size = new System.Drawing.Size(23, 22);
            this.tButDelGroup.Text = "删除组";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tButGroupsRefresh
            // 
            this.tButGroupsRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tButGroupsRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tButGroupsRefresh.Image")));
            this.tButGroupsRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButGroupsRefresh.Name = "tButGroupsRefresh";
            this.tButGroupsRefresh.Size = new System.Drawing.Size(23, 22);
            this.tButGroupsRefresh.Text = "刷新";
            this.tButGroupsRefresh.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // LabTitle
            // 
            this.LabTitle.Name = "LabTitle";
            this.LabTitle.Size = new System.Drawing.Size(68, 22);
            this.LabTitle.Text = "当前选择：";
            // 
            // tsbOK
            // 
            this.tsbOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOK.Image = ((System.Drawing.Image)(resources.GetObject("tsbOK.Image")));
            this.tsbOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOK.Name = "tsbOK";
            this.tsbOK.Size = new System.Drawing.Size(23, 22);
            this.tsbOK.Text = "OK";
            this.tsbOK.Click += new System.EventHandler(this.tsbOK_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "list_down.ico");
            this.imageList1.Images.SetKeyName(1, "list_normal.ico");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelSelectGroup});
            this.statusStrip1.Location = new System.Drawing.Point(0, 346);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(418, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LabelSelectGroup
            // 
            this.LabelSelectGroup.Name = "LabelSelectGroup";
            this.LabelSelectGroup.Size = new System.Drawing.Size(0, 17);
            // 
            // treeView_Organization
            // 
            this.treeView_Organization.AllowDrop = true;
            this.treeView_Organization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Organization.HideSelection = false;
            this.treeView_Organization.ImageIndex = 1;
            this.treeView_Organization.ImageList = this.imageList1;
            this.treeView_Organization.Location = new System.Drawing.Point(0, 25);
            this.treeView_Organization.Name = "treeView_Organization";
            this.treeView_Organization.SelectedImageIndex = 0;
            this.treeView_Organization.ShowRootLines = false;
            this.treeView_Organization.Size = new System.Drawing.Size(418, 321);
            this.treeView_Organization.TabIndex = 17;
            // 
            // formSelectGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 368);
            this.Controls.Add(this.treeView_Organization);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "formSelectGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择分组";
            this.Load += new System.EventHandler(this.formSelectGroup_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tButAddGroup;
        private System.Windows.Forms.ToolStripButton tButDelGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tButGroupsRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel LabTitle;
        private System.Windows.Forms.ToolStripButton tsbOK;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LabelSelectGroup;
        private System.Windows.Forms.TreeView treeView_Organization;

    }
}