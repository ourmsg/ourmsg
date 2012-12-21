
namespace OurMsg.controls
{
    partial class webUpdate
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.webFile1 = new IMLibrary3.Components.WebFile(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 51);
            this.panel1.TabIndex = 5;
            this.panel1.UseWaitCursor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ProgressBar1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 31);
            this.panel2.TabIndex = 6;
            this.panel2.UseWaitCursor = true;
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProgressBar1.Location = new System.Drawing.Point(16, 0);
            this.ProgressBar1.Maximum = 1000000;
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(268, 18);
            this.ProgressBar1.TabIndex = 3;
            this.ProgressBar1.UseWaitCursor = true;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(284, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(16, 31);
            this.panel4.TabIndex = 1;
            this.panel4.UseWaitCursor = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(16, 31);
            this.panel3.TabIndex = 0;
            this.panel3.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(0, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 68);
            this.label1.TabIndex = 8;
            this.label1.Text = "正在检测更新...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.UseWaitCursor = true;
            // 
            // webFile1
            // 
            this.webFile1.fileTransmitted += new IMLibrary3.Components.WebFile.fileTransmitEventHandler(this.webFile1_fileTransmitted);
            this.webFile1.fileTransmitError += new IMLibrary3.Components.WebFile.fileTransmitEventHandler(this.webFile1_fileTransmitError);
            this.webFile1.fileTransmitting += new IMLibrary3.Components.WebFile.fileTransmitEventHandler(this.webFile1_fileTransmitting);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // webUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "webUpdate";
            this.Size = new System.Drawing.Size(300, 150);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar  ProgressBar1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private IMLibrary3.Components.WebFile webFile1;
        private System.Windows.Forms.Timer timer1;

    }
}
