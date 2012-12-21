using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OurMsg 
{
    public partial class FormDownOrganization : Form
    {
        public FormDownOrganization()
        {
            InitializeComponent();
        }

        private int _OutTimeSeconds = 5;
        /// <summary>
        /// 设置或获取超时秒数
        /// </summary>
        public int OutTimeSeconds
        {
            set
            {
                _OutTimeSeconds = value;
            }
            get { return _OutTimeSeconds; }
        }


        private int _Times = 0;
        /// <summary>
        /// 超时计数器
        /// </summary>
        public int Times
        {
            set { _Times = value; }
            get { return _Times; }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string ShowText
        {
            set { this.LabMsg.Text = value; }
            get { return this.LabMsg.Text; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public int Value
        {
            set
            {
                this.ProgressBar.Value = value;
                this.label1.Text = value + "/" + MaxValue;
            }
            get { return this.ProgressBar.Value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            set
            {
                this.ProgressBar.Maximum = value;
                this.label1.Text = this.ProgressBar.Value + "/" + this.ProgressBar.Maximum;
            }
            get { return this.ProgressBar.Maximum; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _Times++;
            if (this.ProgressBar.Value == this.ProgressBar.Maximum)
                this.Close();

            if (_Times > OutTimeSeconds)
                this.Close();
        }
    }
}
