using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OurMsg
{
    public partial class FormSendNotice : Form
    {
        public FormSendNotice()
        {
            InitializeComponent();
            this.tsButColor.Click +=new EventHandler(tsButColor_Click);
            this.tsButSetFont.Click +=new EventHandler(tsButSetFont_Click);
            this.contextMenuStrip1.ItemClicked +=new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this.contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
        }

        #region 变量
        /// <summary>
        /// 表情菜单
        /// </summary>
        private ToolStripDropDown DropDownFace = new ToolStripDropDown();
        #endregion 

        #region 事件

        /// <summary>
        /// 发送事件代理
        /// </summary>
        /// <param name="Msg">要发送的消息</param>
        public delegate void SendMsgEventHandler(IMLibrary3.Protocol.Message Msg);

        /// <summary>
        /// 登录密码错误事件
        /// </summary>
        public event SendMsgEventHandler SendNotice;

        #endregion

        #region 窗口加载事件
        private void FormSendNotice_Load(object sender, EventArgs e)
        {
            FaceSet();
        }
        #endregion

        #region 表情菜单操作
        /// <summary>
        /// 表情菜单初始化
        /// </summary>
        private void FaceSet()
        {
            this.DropDownFace.ItemClicked += delegate(object sender, ToolStripItemClickedEventArgs e)
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
                        //System.Windows.Forms.IDataObject data = Clipboard.GetDataObject();//从剪贴板中获取数据
                        //if (data.GetDataPresent(typeof(Bitmap)))//判断是否是图片类型

                        this.txtSend.Paste();
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
 
        #endregion

        #region 窗口关闭事件
        private void butClose_Click(object sender, EventArgs e)
        { 
            this.Close();
        }
        #endregion 

        #region 发送通知消息事件
        private void butSend_Click(object sender, EventArgs e)
        {
            IMLibrary3.Protocol.Message msg = GetSendMsg();
            msg.MessageType = IMLibrary3.Enmu.MessageType.Notice;
            msg.Title= txtTitle.Text.Trim();
            msg.remark  = txtRemark.Text.Trim();
            if (SendNotice != null)
                SendNotice(msg);//触发发送通知消息事件
            this.Close();
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
            return msg;
        }
        #endregion

    }
}
