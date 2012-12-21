using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CSharpWin;

namespace OurMsg.Controls
{
    public partial class MessagePanel : UserControl
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public MessagePanel()
        {
            InitializeComponent();

            this.txtSend.KeyUp += new KeyEventHandler(txtSend_KeyUp);
            this.txtSend.KeyDown += new KeyEventHandler(txtSend_KeyDown);
            this.txtSend.DragDrop += new DragEventHandler(txtSend_DragDrop);
            this.txtSend.DragEnter += new DragEventHandler(txtSend_DragEnter);
            this.txtSend.AllowDrop = true;

            //this.txtRecord.DragDrop += new DragEventHandler(txtSend_DragDrop);
            //this.txtRecord.DragEnter += new DragEventHandler(txtSend_DragEnter);
            //this.txtRecord.AllowDrop = true;


            this.txtRecord.LinkClicked += new LinkClickedEventHandler(txtRecord_LinkClicked);

            this.tsButSetFont.Click += new EventHandler(tsButSetFont_Click);
            this.tsButColor.Click += new EventHandler(tsButColor_Click);
            this.tsButCaptureImageTool.Click +=new EventHandler(tsButCaptureImageTool_Click);
            this.contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this.contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            this.contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip2_ItemClicked);
   
        }

        void txtSend_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.All;
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        void txtSend_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            { 
                string[] MyFiles;
                int i;
                // 将文件赋给一个数组。
                MyFiles = (string[])(e.Data.GetData(DataFormats.FileDrop));
                // 循环处理数组并将文件添加到列表中。
                for (i = 0; i <= MyFiles.Length - 1; i++)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(MyFiles[i]);
                    if (f.Exists && SendFile!=null)
                        SendFile(this,f.FullName);//触发发送文件事件
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text) && this.txtSend.SelectedText != "")
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetDataPresent(DataFormats.Text) && this.txtSend.SelectedText == "")
                e.Effect = DragDropEffects.Copy;
        }

        void txtRecord_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch
            {
                MessageBox.Show("无法打开链接。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 变量
        /// <summary>
        /// 表情菜单
        /// </summary>
        private ToolStripDropDown DropDownFace = new ToolStripDropDown();
        /// <summary>
        /// 需要接收的图片文件
        /// </summary>
        public List<IMLibrary3.MyPicture> needRecPictures = new List<IMLibrary3.MyPicture>();
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string myUserID = "";
        #endregion

        #region 事件
        /// <summary>
        /// 发送事件代理
        /// </summary>
        /// <param name="Msg">要发送的消息</param>
        public delegate void CreateMsgEventHandler(object sender, IMLibrary3.Protocol.Message Msg);

        /// <summary>
        /// 登录密码错误事件
        /// </summary>
        public event CreateMsgEventHandler CreateMsgAfter;

        public delegate void SendFileEventHandler(object sender, string filename);

        /// <summary>
        /// 发送文件
        /// </summary>
        public event SendFileEventHandler SendFile;

        #endregion
        
        #region 公共方法

        #region 清除消息文本
        /// <summary>
        /// 清除消息文本
        /// </summary>
        public void ClearMsgTextBox()
        {
            txtSend.Clear();
            txtSend.Focus();
            txtSend.Select(0, 0);
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// Sends currently typed text from active wrie tab.
        /// </summary>
        public void SendMsg()
        {
            try
            {
                string text = "";
                text = this.txtSend.Text;

                if (text.Length == 0)
                    return;

                IMLibrary3.Protocol.Message msg = GetSendMsg();
                if (CreateMsgAfter != null)
                    CreateMsgAfter(this, msg);//触发发送消息事件

            }
            catch (Exception x)
            {
                MessageBox.Show(this, "Error: " + x.ToString(), "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 将用户对话消息加入 RichTextBox 控件
        /// <summary>
        /// 将用户对话消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        /// <param name="title">标题（用户名+用户ID）</param>
        public void MsgToRichTextBox(IMLibrary3.Protocol.Message msg, bool IsSend, string title)//将发送的消息加入历史rich
        {
            int iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
            title += System.DateTime.Now.ToString();
            Color titleColor = Color.Red;

            if (!IsSend)//如果是收到消息
                titleColor = Color.Blue;
          

            txtRecord.AppendText(title);
            txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = new Font("宋体", 10);
            txtRecord.SelectionColor = titleColor;

            txtRecord.AppendText("\n  ");

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

                    txtRecord.AppendText(msg.Content.Substring(textPos, currPos - addPos));
                    txtRecord.SelectionStart = txtRecord.TextLength;

                    textPos += currPos - addPos;
                    addPos += currPos - addPos;

                    Image image = null;
                    if (IsSend)//如果图片是自己发送给自己的
                    {
                        image = txtSend.GetPicture(imageContent[1]).Image;
                    }
                    else//如果图片是对方发送过来的
                    {
                        if (txtRecord.GetPicture(imageContent[1]) != null)
                            image = txtRecord.GetPicture(imageContent[1]).Image;
                        else
                        {
                            string fileName = myUserID + "\\ArrivalImage\\" + imageContent[1] + ".gif";
                            if (System.IO.File.Exists(fileName)) //如果本地已经有此文件已经存在 
                                image = System.Drawing.Image.FromFile(fileName);
                        }

                        if (imageContent[1].Length < 32 && image == null)
                            image = System.Drawing.Image.FromFile(Application.StartupPath + @"\face\" + imageContent[1] + ".gif");
                    }
                    if (image == null)
                    {
                        image = OurMsg.Properties.Resources.Wait ;
                        IMLibrary3.MyPicture pic = txtRecord.addGifControl(imageContent[1], image);
                        needRecPictures.Add(pic);
                    }
                    else
                    {
                        txtRecord.addGifControl(imageContent[1], image);
                    }

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

            txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = msg.Font;
            txtRecord.SelectionColor = msg.Color;

            txtRecord.Select(txtRecord.TextLength, 0);
            txtRecord.ScrollToCaret();
        }
        #endregion

        #region 表情菜单初始化
        /// <summary>
        /// 表情菜单初始化
        /// </summary>
        public void FaceSet()
        {
            this.DropDownFace.ItemClicked +=delegate(object sender, ToolStripItemClickedEventArgs e)
            {
                string FileNamePath = Application.StartupPath + @"\face\" + e.ClickedItem.ToolTipText.ToString() + ".gif";
                if (System.IO.File.Exists(FileNamePath))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(FileNamePath);
                    this.txtSend.addGifControl(e.ClickedItem.ToolTipText.ToString(), image);
                }
            };  


            tsButFace.DropDown = DropDownFace;
            DropDownFace.ImageScalingSize = new Size(24, 24);//图片大小
            DropDownFace.LayoutStyle = ToolStripLayoutStyle.Table;//设置布局
            ((TableLayoutSettings)DropDownFace.LayoutSettings).ColumnCount = 15;//设置每行显示数目
            for (int i = 0; i < Global.ImageListFace.Images.Count; i++)
            {
                ToolStripItem item = DropDownFace.Items.Add(Global.ImageListFace.Images[i]);
                item.ToolTipText = i.ToString(); 
            }
        }
        #endregion

        #region 获得要发送的图片集合
        /// <summary>
        /// 获得要发送的图片集合
        /// </summary>
        /// <returns></returns>
        public List<IMLibrary3.MyPicture> GetSendPicture()
        {
            return txtSend.GetExistGifs();
        }
        #endregion

        #region 添加文本到对话记录文本框
        /// <summary>
        /// 添加文本到对话记录文本框
        /// </summary>
        /// <param name="text"></param>
        public void addRemarkTextToRecord(string text)
        {
            int iniPos = txtRecord.TextLength;//获得当前记录richBox中最后的位置
            Color titleColor = Color.Brown;
            txtRecord.AppendText(text);
            txtRecord.Select(iniPos, txtRecord.TextLength - iniPos);
            txtRecord.SelectionFont = new Font("宋体", 10);
            txtRecord.SelectionColor = titleColor;
            txtRecord.AppendText("\n");
            txtRecord.Select(txtRecord.TextLength, 0);
            txtRecord.ScrollToCaret();
        }
        #endregion

        #endregion

        #region 发送消息框事件处理过程

        private void txtSend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                txtSend.Clear();
                txtSend.Focus();
                txtSend.Select(0, 0);
            }
        }

        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                SendMsg();               
            }
        }

        #endregion

        #region 获得发送的消息类
        /// <summary>
        /// 获得发送的消息类
        /// </summary>
        /// <returns></returns>
        private IMLibrary3.Protocol.Message GetSendMsg()//获得要发送的消息类
        {
            IMLibrary3.Protocol.Message msg = new IMLibrary3.Protocol.Message();
            msg.Content = this.txtSend.Text.TrimEnd();//获得消息文本内容
            msg.Font = this.txtSend.Font;
            msg.Color = this.txtSend.ForeColor; 
            msg.ImageInfo = this.txtSend.GetImageInfo();
            //List<IMLibrary3.MyPicture> tempGifs = this.txtSend.GetExistGifs();//获得richtextBox中存在的图片信息的集合
            //foreach (IMLibrary3.MyPicture pic in tempGifs)
            //    if (Convert.ToUInt32(pic.Tag) > 200)//如果图片是用户自己定义的 
            //        pic.IsSent = true;//标识此图片需要使用P2P方式发送给对方
            return msg;
        }
        #endregion

        #region 表情菜单操作
 
        #endregion

        #region 消息字体设置
        private void tsButSetFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = this.txtSend.Font;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.txtSend.Font = fd.Font;

        }

        private void tsButColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = this.txtSend.ForeColor;
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.txtSend.ForeColor = cd.Color;
        }
        #endregion

        #region 关联菜单事件
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "MenuItemCopy":
                    this.txtSend.Copy();
                    break;
                case "MenuItemPaset":
                    {
                        System.Windows.Forms.IDataObject data = Clipboard.GetDataObject();//从剪贴板中获取数据
                        if (data.GetDataPresent(typeof(Image)))//判断是否是图片类型
                            MessageBox.Show(data.GetType().ToString());
                        //this.txtSend.Paste();
                    }
                    break;
                case "MenuItemCut":
                    this.txtSend.Cut();
                    break;
                case "MenuItemDel":
                    this.txtSend.SelectedText = "";
                    break;
                case "MenuItemSelAll":
                    this.Focus();
                    this.txtSend.SelectAll();
                    break;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool t = true;

            if (this.txtSend.SelectedText.Length == 0)
                t = false;

            this.MenuItemCopy.Visible = t;
            this.MenuItemCut.Visible = t;
            this.MenuItemDel.Visible = t;
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "MenuItemCopy2":
                    this.txtRecord.Copy();
                    break;

                case "MenuItemSelAll2":
                    this.Focus();
                    this.txtRecord.SelectAll();
                    break;
            }
        }

        #endregion

        #region 截图操作
        private void tsButCaptureImageTool_Click(object sender, EventArgs e)
        {
            CaptureImageTool capture = new CaptureImageTool();
            if (capture.ShowDialog() == DialogResult.OK)
            {
                Image image = capture.Image;
                if (image != null)
                {
                    System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(myUserID+"\\sendImage");
                    if (!dInfo.Exists)
                        dInfo.Create();

                    string fileName =myUserID+"\\sendImage\\temp.gif";
                    image.Save(fileName);
                    string md5 = IMLibrary3.Security.Hasher.GetMD5Hash(fileName);
                    string Md5fileName =myUserID+"\\sendImage\\" + md5 + ".gif";

                    if (!System.IO.File.Exists(Md5fileName))
                    {
                        System.IO.File.Delete(fileName);
                        image.Save(Md5fileName);
                    }
                    this.txtSend.addGifControl(md5, image);
                }
            }
        }
        #endregion
    }
}
