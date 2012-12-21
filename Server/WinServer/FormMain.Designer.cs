namespace OurMsgServer
{
    partial class FormMain
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
            this.butStrat = new System.Windows.Forms.Button();
            this.butStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butStrat
            // 
            this.butStrat.Location = new System.Drawing.Point(12, 28);
            this.butStrat.Name = "butStrat";
            this.butStrat.Size = new System.Drawing.Size(133, 36);
            this.butStrat.TabIndex = 0;
            this.butStrat.Text = "启动服务";
            this.butStrat.UseVisualStyleBackColor = true;
            this.butStrat.Click += new System.EventHandler(this.butStrat_Click);
            // 
            // butStop
            // 
            this.butStop.Enabled = false;
            this.butStop.Location = new System.Drawing.Point(258, 28);
            this.butStop.Name = "butStop";
            this.butStop.Size = new System.Drawing.Size(121, 36);
            this.butStop.TabIndex = 1;
            this.butStop.Text = "停止服务";
            this.butStop.UseVisualStyleBackColor = true;
            this.butStop.Click += new System.EventHandler(this.butStop_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 92);
            this.Controls.Add(this.butStop);
            this.Controls.Add(this.butStrat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ourmsg服务器";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butStrat;
        private System.Windows.Forms.Button butStop;
    }
}