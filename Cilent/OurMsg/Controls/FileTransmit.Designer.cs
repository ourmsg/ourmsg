namespace OurMsg.Controls
{
    partial class FileTransmit
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
                if (P2PFileTransmit != null)
                {
                    P2PFileTransmit.Dispose();
                    P2PFileTransmit = null;
                }
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.linkLabelOffline = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelRequest = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelConType = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelFileLengthString = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelReceive = new System.Windows.Forms.LinkLabel();
            this.linkLabelResume = new System.Windows.Forms.LinkLabel();
            this.linkLabelSaveAs = new System.Windows.Forms.LinkLabel();
            this.linkLabelCancel = new System.Windows.Forms.LinkLabel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(4, 86);
            this.panel3.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(210, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(6, 86);
            this.panel4.TabIndex = 10;
            // 
            // linkLabelOffline
            // 
            this.linkLabelOffline.AutoSize = true;
            this.linkLabelOffline.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelOffline.Location = new System.Drawing.Point(25, 102);
            this.linkLabelOffline.Name = "linkLabelOffline";
            this.linkLabelOffline.Size = new System.Drawing.Size(77, 12);
            this.linkLabelOffline.TabIndex = 7;
            this.linkLabelOffline.TabStop = true;
            this.linkLabelOffline.Text = "发送离线文件";
            this.linkLabelOffline.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelFileName);
            this.panel2.Controls.Add(this.labelRequest);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.labelConType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(4, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 42);
            this.panel2.TabIndex = 14;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(40, 23);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(47, 12);
            this.labelFileName.TabIndex = 11;
            this.labelFileName.Text = "xxx.exe";
            // 
            // labelRequest
            // 
            this.labelRequest.AutoSize = true;
            this.labelRequest.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelRequest.Location = new System.Drawing.Point(40, 7);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(89, 12);
            this.labelRequest.TabIndex = 10;
            this.labelRequest.Text = "发送文件请求：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 38);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // labelConType
            // 
            this.labelConType.AutoSize = true;
            this.labelConType.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelConType.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelConType.Location = new System.Drawing.Point(206, 0);
            this.labelConType.Name = "labelConType";
            this.labelConType.Size = new System.Drawing.Size(0, 12);
            this.labelConType.TabIndex = 8;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(4, 42);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(206, 12);
            this.progressBar1.TabIndex = 15;
            // 
            // labelFileLengthString
            // 
            this.labelFileLengthString.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFileLengthString.Location = new System.Drawing.Point(4, 54);
            this.labelFileLengthString.Name = "labelFileLengthString";
            this.labelFileLengthString.Size = new System.Drawing.Size(206, 12);
            this.labelFileLengthString.TabIndex = 16;
            this.labelFileLengthString.Text = "12.00KB";
            this.labelFileLengthString.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelReceive);
            this.panel1.Controls.Add(this.linkLabelResume);
            this.panel1.Controls.Add(this.linkLabelSaveAs);
            this.panel1.Controls.Add(this.linkLabelCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(58, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(152, 20);
            this.panel1.TabIndex = 17;
            // 
            // linkLabelReceive
            // 
            this.linkLabelReceive.AutoSize = true;
            this.linkLabelReceive.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelReceive.Location = new System.Drawing.Point(39, 5);
            this.linkLabelReceive.Name = "linkLabelReceive";
            this.linkLabelReceive.Size = new System.Drawing.Size(29, 12);
            this.linkLabelReceive.TabIndex = 5;
            this.linkLabelReceive.TabStop = true;
            this.linkLabelReceive.Text = "接收";
            // 
            // linkLabelResume
            // 
            this.linkLabelResume.AutoSize = true;
            this.linkLabelResume.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelResume.Location = new System.Drawing.Point(40, 5);
            this.linkLabelResume.Name = "linkLabelResume";
            this.linkLabelResume.Size = new System.Drawing.Size(29, 12);
            this.linkLabelResume.TabIndex = 6;
            this.linkLabelResume.TabStop = true;
            this.linkLabelResume.Text = "续传";
            this.linkLabelResume.Visible = false;
            // 
            // linkLabelSaveAs
            // 
            this.linkLabelSaveAs.AutoSize = true;
            this.linkLabelSaveAs.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelSaveAs.Location = new System.Drawing.Point(76, 5);
            this.linkLabelSaveAs.Name = "linkLabelSaveAs";
            this.linkLabelSaveAs.Size = new System.Drawing.Size(41, 12);
            this.linkLabelSaveAs.TabIndex = 4;
            this.linkLabelSaveAs.TabStop = true;
            this.linkLabelSaveAs.Text = "另存为";
            // 
            // linkLabelCancel
            // 
            this.linkLabelCancel.AutoSize = true;
            this.linkLabelCancel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.linkLabelCancel.Location = new System.Drawing.Point(123, 5);
            this.linkLabelCancel.Name = "linkLabelCancel";
            this.linkLabelCancel.Size = new System.Drawing.Size(29, 12);
            this.linkLabelCancel.TabIndex = 3;
            this.linkLabelCancel.TabStop = true;
            this.linkLabelCancel.Text = "取消";
            // 
            // FileTransmit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.linkLabelOffline);
            this.Controls.Add(this.labelFileLengthString);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "FileTransmit";
            this.Size = new System.Drawing.Size(216, 86);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.LinkLabel linkLabelOffline;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelRequest;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelConType;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelFileLengthString;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelReceive;
        private System.Windows.Forms.LinkLabel linkLabelResume;
        private System.Windows.Forms.LinkLabel linkLabelSaveAs;
        private System.Windows.Forms.LinkLabel linkLabelCancel;
    }
}
