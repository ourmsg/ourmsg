using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OurMsg.Controls
{
    public partial class SelfFaceImage : UserControl
    {
        public SelfFaceImage()
        {
            InitializeComponent();
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void  EventHandler(object sender,EventArgs e);
        /// <summary>
        /// 头像单击事件
        /// </summary>
        public event EventHandler ImageClick;


        private void toolStrip1_MouseLeave(object sender, EventArgs e)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;

        }

        private void toolStrip1_MouseEnter(object sender, EventArgs e)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ImageClick != null)
                ImageClick(this,new EventArgs());
        }
    }
}
