namespace OurMsg
{
    partial class FormNotice
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
            this.txtRecord = new IMLibrary3.MyExtRichTextBox();
            this.SuspendLayout();
            // 
            // txtRecord
            // 
            this.txtRecord.BackColor = System.Drawing.Color.White;
            this.txtRecord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecord.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRecord.Location = new System.Drawing.Point(0, 0);
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.ReadOnly = true;
            this.txtRecord.Size = new System.Drawing.Size(492, 362);
            this.txtRecord.TabIndex = 28;
            this.txtRecord.Text = "";
            // 
            // FormNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 362);
            this.Controls.Add(this.txtRecord);
            this.Name = "FormNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通知消息";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormNotice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private IMLibrary3.MyExtRichTextBox txtRecord;
    }
}