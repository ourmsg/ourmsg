namespace IMLibrary3.Controls
{
    partial class ShowType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowType));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labShowType = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tButShowType = new System.Windows.Forms.ToolStripDropDownButton();
            this.tMenuItemStateOnline = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenuItemStateAway = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenuItemStateBusy = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenuItemStateBlocked = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenuItemStateHiding = new System.Windows.Forms.ToolStripMenuItem();
            this.tMenuItemStateOfflineSp = new System.Windows.Forms.ToolStripSeparator();
            this.tMenuItemStateOffline = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tMenuItemExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.labShowType);
            this.panel3.Controls.Add(this.toolStrip1);
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(146, 23);
            this.panel3.TabIndex = 34;
            // 
            // labShowType
            // 
            this.labShowType.Location = new System.Drawing.Point(44, 6);
            this.labShowType.Name = "labShowType";
            this.labShowType.Size = new System.Drawing.Size(99, 17);
            this.labShowType.TabIndex = 35;
            this.labShowType.Text = "在线";
            this.labShowType.DoubleClick += new System.EventHandler(this.labShowType_DoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tButShowType});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(146, 25);
            this.toolStrip1.TabIndex = 31;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tButShowType
            // 
            this.tButShowType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tButShowType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tMenuItemStateOnline,
            this.tMenuItemStateAway,
            this.tMenuItemStateBusy,
            this.tMenuItemStateBlocked,
            this.tMenuItemStateHiding,
            this.tMenuItemStateOfflineSp,
            this.tMenuItemStateOffline,
            this.toolStripMenuItem1,
            this.tMenuItemAbout,
            this.toolStripMenuItem2,
            this.tMenuItemExitApp});
            this.tButShowType.Image = ((System.Drawing.Image)(resources.GetObject("tButShowType.Image")));
            this.tButShowType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tButShowType.Name = "tButShowType";
            this.tButShowType.Size = new System.Drawing.Size(29, 22);
            this.tButShowType.ToolTipText = "在线状态";
            // 
            // tMenuItemStateOnline
            // 
            this.tMenuItemStateOnline.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateOnline.Image")));
            this.tMenuItemStateOnline.Name = "tMenuItemStateOnline";
            this.tMenuItemStateOnline.Size = new System.Drawing.Size(152, 22);
            this.tMenuItemStateOnline.Text = "在线";
            // 
            // tMenuItemStateAway
            // 
            this.tMenuItemStateAway.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateAway.Image")));
            this.tMenuItemStateAway.Name = "tMenuItemStateAway";
            this.tMenuItemStateAway.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemStateAway.Text = "离开";
            // 
            // tMenuItemStateBusy
            // 
            this.tMenuItemStateBusy.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateBusy.Image")));
            this.tMenuItemStateBusy.Name = "tMenuItemStateBusy";
            this.tMenuItemStateBusy.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemStateBusy.Text = "忙碌";
            // 
            // tMenuItemStateBlocked
            // 
            this.tMenuItemStateBlocked.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateBlocked.Image")));
            this.tMenuItemStateBlocked.Name = "tMenuItemStateBlocked";
            this.tMenuItemStateBlocked.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemStateBlocked.Text = "请勿打扰";
            // 
            // tMenuItemStateHiding
            // 
            this.tMenuItemStateHiding.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateHiding.Image")));
            this.tMenuItemStateHiding.Name = "tMenuItemStateHiding";
            this.tMenuItemStateHiding.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemStateHiding.Text = "隐身";
            // 
            // tMenuItemStateOfflineSp
            // 
            this.tMenuItemStateOfflineSp.Name = "tMenuItemStateOfflineSp";
            this.tMenuItemStateOfflineSp.Size = new System.Drawing.Size(145, 6);
            // 
            // tMenuItemStateOffline
            // 
            this.tMenuItemStateOffline.Image = ((System.Drawing.Image)(resources.GetObject("tMenuItemStateOffline.Image")));
            this.tMenuItemStateOffline.Name = "tMenuItemStateOffline";
            this.tMenuItemStateOffline.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemStateOffline.Text = "离线";
            this.tMenuItemStateOffline.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // tMenuItemAbout
            // 
            this.tMenuItemAbout.Name = "tMenuItemAbout";
            this.tMenuItemAbout.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemAbout.Text = "关于OurMsg";
            this.tMenuItemAbout.Click += new System.EventHandler(this.tMenuItemAbout_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // tMenuItemExitApp
            // 
            this.tMenuItemExitApp.Name = "tMenuItemExitApp";
            this.tMenuItemExitApp.Size = new System.Drawing.Size(148, 22);
            this.tMenuItemExitApp.Text = "退出程序";
            this.tMenuItemExitApp.Click += new System.EventHandler(this.tMenuItemExitApp_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(111, 21);
            this.textBox1.TabIndex = 36;
            this.textBox1.Visible = false;
            this.textBox1.MouseLeave += new System.EventHandler(this.textBox1_MouseLeave);
            // 
            // ShowType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panel3);
            this.Name = "ShowType";
            this.Size = new System.Drawing.Size(162, 26);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label labShowType;
        private System.Windows.Forms.ToolStripDropDownButton tButShowType;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateOnline;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateAway;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateBusy;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateBlocked;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateHiding;
        private System.Windows.Forms.ToolStripSeparator tMenuItemStateOfflineSp;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemStateOffline;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tMenuItemExitApp;
        private System.Windows.Forms.TextBox textBox1;

    }
}
