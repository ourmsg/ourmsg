namespace OurMsg.Controls
{
    partial class video
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(video));
            this.panelAVUp = new System.Windows.Forms.Panel();
            this.panelRemotAV = new System.Windows.Forms.Panel();
            this.panelLocalAV = new System.Windows.Forms.Panel();
            this.panelSpDown = new System.Windows.Forms.Panel();
            this.panelSpRight = new System.Windows.Forms.Panel();
            this.panelSpLeft = new System.Windows.Forms.Panel();
            this.panelSpUp = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butHangup = new System.Windows.Forms.Button();
            this.butReceive = new System.Windows.Forms.Button();
            this.panelAVUp.SuspendLayout();
            this.panelRemotAV.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelAVUp
            // 
            this.panelAVUp.BackColor = System.Drawing.Color.Transparent;
            this.panelAVUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelAVUp.Controls.Add(this.panelRemotAV);
            this.panelAVUp.Controls.Add(this.panelSpDown);
            this.panelAVUp.Controls.Add(this.panelSpRight);
            this.panelAVUp.Controls.Add(this.panelSpLeft);
            this.panelAVUp.Controls.Add(this.panelSpUp);
            this.panelAVUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAVUp.Location = new System.Drawing.Point(0, 0);
            this.panelAVUp.Name = "panelAVUp";
            this.panelAVUp.Size = new System.Drawing.Size(330, 250);
            this.panelAVUp.TabIndex = 0;
            // 
            // panelRemotAV
            // 
            this.panelRemotAV.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelRemotAV.BackgroundImage")));
            this.panelRemotAV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelRemotAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRemotAV.Controls.Add(this.panelLocalAV);
            this.panelRemotAV.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRemotAV.Location = new System.Drawing.Point(4, 4);
            this.panelRemotAV.Name = "panelRemotAV";
            this.panelRemotAV.Size = new System.Drawing.Size(320, 240);
            this.panelRemotAV.TabIndex = 5;
            // 
            // panelLocalAV
            // 
            this.panelLocalAV.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelLocalAV.BackgroundImage")));
            this.panelLocalAV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelLocalAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLocalAV.Location = new System.Drawing.Point(243, 180);
            this.panelLocalAV.Name = "panelLocalAV";
            this.panelLocalAV.Size = new System.Drawing.Size(90, 60);
            this.panelLocalAV.TabIndex = 0;
            // 
            // panelSpDown
            // 
            this.panelSpDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSpDown.Location = new System.Drawing.Point(4, 246);
            this.panelSpDown.Name = "panelSpDown";
            this.panelSpDown.Size = new System.Drawing.Size(320, 4);
            this.panelSpDown.TabIndex = 4;
            // 
            // panelSpRight
            // 
            this.panelSpRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSpRight.Location = new System.Drawing.Point(324, 4);
            this.panelSpRight.Name = "panelSpRight";
            this.panelSpRight.Size = new System.Drawing.Size(6, 246);
            this.panelSpRight.TabIndex = 3;
            // 
            // panelSpLeft
            // 
            this.panelSpLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSpLeft.Location = new System.Drawing.Point(0, 4);
            this.panelSpLeft.Name = "panelSpLeft";
            this.panelSpLeft.Size = new System.Drawing.Size(4, 246);
            this.panelSpLeft.TabIndex = 2;
            // 
            // panelSpUp
            // 
            this.panelSpUp.BackColor = System.Drawing.Color.Transparent;
            this.panelSpUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpUp.Location = new System.Drawing.Point(0, 0);
            this.panelSpUp.Name = "panelSpUp";
            this.panelSpUp.Size = new System.Drawing.Size(330, 4);
            this.panelSpUp.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butHangup);
            this.panel1.Controls.Add(this.butReceive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 23);
            this.panel1.TabIndex = 2;
            // 
            // butHangup
            // 
            this.butHangup.Location = new System.Drawing.Point(272, -1);
            this.butHangup.Name = "butHangup";
            this.butHangup.Size = new System.Drawing.Size(62, 24);
            this.butHangup.TabIndex = 3;
            this.butHangup.Text = "挂断";
            this.butHangup.UseVisualStyleBackColor = true;
            // 
            // butReceive
            // 
            this.butReceive.Location = new System.Drawing.Point(210, -1);
            this.butReceive.Name = "butReceive";
            this.butReceive.Size = new System.Drawing.Size(62, 24);
            this.butReceive.TabIndex = 4;
            this.butReceive.Text = "接收";
            this.butReceive.UseVisualStyleBackColor = true;
            // 
            // video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelAVUp);
            this.Name = "video";
            this.Size = new System.Drawing.Size(330, 481);
            this.panelAVUp.ResumeLayout(false);
            this.panelRemotAV.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAVUp;
        private System.Windows.Forms.Panel panelLocalAV;
        private System.Windows.Forms.Panel panelSpLeft;
        private System.Windows.Forms.Panel panelSpUp;
        private System.Windows.Forms.Panel panelRemotAV;
        private System.Windows.Forms.Panel panelSpDown;
        private System.Windows.Forms.Panel panelSpRight;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butHangup;
        private System.Windows.Forms.Button butReceive;
    }
}
