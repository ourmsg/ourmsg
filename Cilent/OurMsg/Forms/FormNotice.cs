using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IMLibrary3;
using IMLibrary3.Organization;


namespace OurMsg
{
    public partial class FormNotice : Form
    {
        public FormNotice()
        {
            InitializeComponent();
            this.txtRecord.LinkClicked += new LinkClickedEventHandler(txtRecord_LinkClicked);
        }

        void txtRecord_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch
            {
                //MessageBox.Show("无法打开链接。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void FormNotice_Load(object sender, EventArgs e)
        {

        }

        #region 将用户对话消息加入 RichTextBox 控件
        /// <summary>
        /// 将用户对话消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        public void MsgToRichTextBox(IMLibrary3.Protocol.Message  msg,  exUser user)//将发送的消息加入历史rich
        {
            int iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置

            if (msg.Title!= "")
            {
                #region 添加标题
                txtRecord.AppendText(msg.Title);
                txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
                txtRecord.SelectionFont = new Font("宋体", 18);
                txtRecord.SelectionColor = msg.Color;
                txtRecord.SelectionAlignment = HorizontalAlignment.Center;
                txtRecord.AppendText("\n\n");
                #endregion
            }

            #region 添加内容
            iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置

            #region //如果消息中有图片，则添加图片
            if (msg.ImageInfo != "")//如果消息中有图片，则添加图片
            {
                string[] imagePos = msg.ImageInfo.Split('|');
                int addPos = 0;//
                int currPos = 0;//当前正要添加的文本位置
                int textPos = 0;
                for (int i = 0; i < imagePos.Length - 1; i++)
                {
                    string[] imageContent = imagePos[i].Split(',');//获得图片所在的位置、图片名称 
                    currPos = Convert.ToInt32(imageContent[0]);//获得图片所在的位置

                    this.txtRecord.AppendText(msg.Content.Substring(textPos, currPos - addPos));
                    this.txtRecord.SelectionStart = this.txtRecord.TextLength;

                    textPos += currPos - addPos;
                    addPos += currPos - addPos;

                    Image image;

                    if (this.txtRecord.GetPicture (imageContent[1]) == null)
                        image = System.Drawing.Image.FromFile(Application.StartupPath + @"\face\" + imageContent[1] + ".gif");
                    else
                        image = this.txtRecord.GetPicture(imageContent[1]).Image;

                    this.txtRecord.addGifControl(imageContent[1], image);
                    addPos++;
                }
                this.txtRecord.AppendText(msg.Content.Substring(textPos, msg.Content.Length - textPos) + "  \n");
            }
            #endregion
            #region  //如果消息中没有图片，则直接添加消息文本
            else//如果消息中没有图片，则直接添加消息文本
            {
                txtRecord.AppendText(msg.Content + "\n");
            }
            #endregion
            txtRecord.Select(iniPos, this.txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = msg.Font;
            txtRecord.SelectionColor = msg.Color;
            txtRecord.SelectionAlignment = HorizontalAlignment.Left ;
            #endregion

            if (msg.remark != "")
            {
                #region 添加落款
                iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
                txtRecord.AppendText("\n" + msg.remark);
                txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
                txtRecord.SelectionFont = new Font("宋体", 13);
                txtRecord.SelectionColor = msg.Color;
                txtRecord.SelectionAlignment = HorizontalAlignment.Right;
                txtRecord.AppendText("\n  ");
                #endregion
            }


            iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
            txtRecord.AppendText("\n发送用户："+ user.UserName +"("+user.UserID +")\n");
            txtRecord.AppendText("发送时间：" + System.DateTime.Now.ToString()+ "\n");
            txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = txtRecord.Font ;
            txtRecord.SelectionAlignment = HorizontalAlignment.Left ;

            txtRecord.Select(this.txtRecord.TextLength, 0);
        }
        #endregion
    }
}
