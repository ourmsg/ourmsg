namespace OurMsg.Controls
{
    partial class UserLoginPanel
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
            this.loging1 = new OurMsg.Controls.Login();
            this.SuspendLayout();
            // 
            // loging1
            // 
            this.loging1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loging1.Location = new System.Drawing.Point(58, 109);
            this.loging1.Name = "loging1";
            this.loging1.Size = new System.Drawing.Size(121, 188);
            this.loging1.TabIndex = 0;
            this.loging1.CancelLogin += new OurMsg.Controls.Login.CancleEventHandler(this.loging1_CancelLogin);
            // 
            // UserLoginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loging1);
            this.Name = "UserLoginPanel";
            this.Size = new System.Drawing.Size(238, 432);
            this.Resize += new System.EventHandler(this.UserLoginPanel_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Login loging1;

    }
}
