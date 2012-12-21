using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using IMLibrary3;


namespace OurMsg
{
    public partial class FormTalkUser : Form
    {
        #region 构造函数
 
        internal FormTalkUser(IMLibrary3.Organization.User User)
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(FormTalkUser_FormClosing);
            tsButSendFile.Click += new EventHandler(tsButSendFile_Click);
            tsButAV.Click += new EventHandler(tsButAV_Click);
            video1.Cancel += new OurMsg.Controls.video.CancelEventHandler(video1_Cancel);
            video1.GetIPEndPoint +=new OurMsg.Controls.video.GetIPEndPointEventHandler(video1_GetIPEndPoint);
            flowLayoutPanel1.ControlAdded += new ControlEventHandler(flowLayoutPanel1_ControlAdded);
            flowLayoutPanel1.ControlRemoved += new ControlEventHandler(flowLayoutPanel1_ControlRemoved);
            this.butClose.Click +=new EventHandler(butClose_Click);
            this.butSend.Click +=new EventHandler(butSend_Click);

            this.MessagePanel1.CreateMsgAfter += new OurMsg.Controls.MessagePanel.CreateMsgEventHandler(MessagePanel1_CreateMsgAfter);
            this.MessagePanel1.DragDrop += new DragEventHandler(MessagePanel1_DragDrop);
            this.MessagePanel1.DragEnter += new DragEventHandler(MessagePanel1_DragEnter);
            this.MessagePanel1.SendFile += delegate(object sender, string filename) { sendFile(filename); };
            this.Tag = User.UserID;
            this.User = User; 
        } 
        #endregion

        #region 变量

        /// <summary>
        /// 对方信息
        /// </summary>
        public IMLibrary3.Organization.User User = null;

        string _myUserID = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public string myUserID
        {
            set
            {
                _myUserID = value;
                MessagePanel1.myUserID = value;
            }
            get { return _myUserID; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string myUserName { get; set; }

        #endregion

        #region 事件
        /// <summary>
        /// 发送事件代理
        /// </summary>
        /// <param name="Msg">要发送的消息</param>
        /// <param name="userID">接收消息的用户帐号</param>
        public delegate void SendMsgEventHandler(IMLibrary3.Protocol.Element  e, IMLibrary3.Organization.User User);

        /// <summary>
        /// 发送消息事件
        /// </summary>
        public event SendMsgEventHandler SendMsgToUser;

        #endregion

        #region 文件传输、视频对话事件
        //视频位图信息捕获事件
        void video1_VideoCapturerBefore(object sender, IMLibrary.AV.BITMAPINFO bitmapinfo)
        {
            IMLibrary3.Protocol.AVMsg msg = new IMLibrary3.Protocol.AVMsg();
            msg.type = IMLibrary3.Protocol.type.Else;//其他信息
            msg.biBitCount =bitmapinfo.bmiHeader.biBitCount ;
            msg.biClrImportant = bitmapinfo.bmiHeader.biClrImportant;
            msg.biClrUsed = bitmapinfo.bmiHeader.biClrUsed;
            msg.biCompression = bitmapinfo.bmiHeader.biCompression;
            msg.biHeight = bitmapinfo.bmiHeader.biHeight;
            msg.biPlanes = bitmapinfo.bmiHeader.biPlanes;
            msg.biSize = bitmapinfo.bmiHeader.biSize;
            msg.biSizeImage = bitmapinfo.bmiHeader.biSizeImage;
            msg.biWidth = bitmapinfo.bmiHeader.biWidth;
            msg.biXPelsPerMeter = bitmapinfo.bmiHeader.biXPelsPerMeter;
            msg.biYPelsPerMeter = bitmapinfo.bmiHeader.biYPelsPerMeter;
            
            if (SendMsgToUser != null)//触发消息发送事件
                SendMsgToUser(msg, User);
        } 

        //音视频通信端口获取事件
        void video1_GetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            Console.WriteLine(local.Address.ToString() + ":" + local.Port.ToString() + "=" + remote.Address.ToString() + ":" + remote.Port.ToString());

            IMLibrary3.Protocol.AVMsg msg = new IMLibrary3.Protocol.AVMsg();
            msg.type = IMLibrary3.Protocol.type.set;
            msg.LocalIP = local.Address;
            msg.LocalPort = local.Port;
            msg.remoteIP = remote.Address;
            msg.remotePort = remote.Port;

            if (SendMsgToUser != null)//触发消息发送事件
                SendMsgToUser(msg, User);
        }


        void video1_Cancel(object sender)
        {
            CancelAV(true);//自己取消

            //发送取消
            IMLibrary3.Protocol.AVMsg avMsg = new IMLibrary3.Protocol.AVMsg();
            avMsg.type = IMLibrary3.Protocol.type.cancel;

            if (SendMsgToUser != null)//触发视频消息事件
                SendMsgToUser(avMsg, User);
        }

        void tsButAV_Click(object sender, EventArgs e)
        {
            ReadyAV(true);//邀请对方并准备视频

            //发送邀请
            IMLibrary3.Protocol.AVMsg avMsg = new IMLibrary3.Protocol.AVMsg();
            avMsg.type = IMLibrary3.Protocol.type.New;

            if (SendMsgToUser != null)//触发视频消息事件
                SendMsgToUser(avMsg, User);
        }

        void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                xPanderPanel1.Visible = false;
                if (!xPanderPanel2.Visible)
                    panelDynamicInfo.Width = 150;
                else
                    xPanderPanel2.Expand = true;

                xPanderPanelList1.Refresh();

            }
        }

        void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (xPanderPanel2.Visible)
                panelDynamicInfo.Width = 345;
            else
                panelDynamicInfo.Width = 245;
            xPanderPanel1.Visible = true;
            xPanderPanel1.Expand = true;
        }

        void MessagePanel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.All;
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        void MessagePanel1_DragDrop(object sender, DragEventArgs e)
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
                    if (f.Exists)
                        sendFile(f.FullName);
                }
            }
        }
        #endregion

        #region 发送消息单击事件
        private void butSend_Click(object sender, EventArgs e)
        {
            butSend.Enabled = false;
            this.MessagePanel1.SendMsg();
            butSend.Enabled = true;            
        }
        #endregion

        #region 当消息创建事件
        private void MessagePanel1_CreateMsgAfter(object sender, IMLibrary3.Protocol.Message msg)
        {
            if (User.UserID == myUserID)
            {
                IMLibrary3.Global.MsgShow("不能给自己发送消息");
                return;
            }

            msg.MessageType = IMLibrary3.Enmu.MessageType.User;

            if (SendMsgToUser != null)
                SendMsgToUser(msg, User);
            this.MsgToRichTextBox(msg, true);

            #region 发送截图
            List<IMLibrary3.MyPicture> pictures = MessagePanel1.GetSendPicture();
            if (pictures != null )//如果文件是截图
                foreach (IMLibrary3.MyPicture pic in pictures)
                    if (pic.MD5.Length == 32)
                    {
                        string fileName = myUserID + "\\sendImage\\" + pic.MD5 + ".gif";
                        ImageFileClient tcpFile = new ImageFileClient(Global.ImageServerEP, fileName);//发送图片文件到服务器
                        tcpFile.fileTransmitted += delegate(object sender1, fileTransmitEvnetArgs e)
                        {
                            if (SendMsgToUser != null)
                            {
                                IMLibrary3.Protocol.ImageFileMsg fileMsg = new IMLibrary3.Protocol.ImageFileMsg();
                                fileMsg.MessageType = IMLibrary3.Enmu.MessageType.User;
                                fileMsg.Name = "";
                                fileMsg.MD5 = e.fileInfo.MD5;
                                fileMsg.Extension = e.fileInfo.Extension;
                                fileMsg.Length = e.fileInfo.Length;
                                SendMsgToUser(fileMsg, User);
                            }
                            (sender1 as ImageFileClient).Dispose();
                            sender1 = null;
                        };
                    }
            #endregion

            this.MessagePanel1.ClearMsgTextBox();
        }
        #endregion

        #region 图片上传到文件服务器成功事件
        private void tcpFile_fileTransmitted(object sender, fileTransmitEvnetArgs e)
        {
            if (SendMsgToUser != null)
            {
                IMLibrary3.Protocol.ImageFileMsg fileMsg = new IMLibrary3.Protocol.ImageFileMsg();
                fileMsg.MessageType = IMLibrary3.Enmu.MessageType.User;
                fileMsg.Name = "";
                fileMsg.MD5 = e.fileInfo.MD5;
                fileMsg.Extension = e.fileInfo.Extension;
                fileMsg.Length = e.fileInfo.Length;
                SendMsgToUser(fileMsg, User);
            }
        }
        #endregion

        #region 窗口事件

        #region 窗口加载事件
        private void FormTalkUser_Load(object sender, EventArgs e)
        {
            this.MessagePanel1. FaceSet();
        }
        #endregion

        #region 关闭窗口按钮单击事件
        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 窗口关闭事件
        void FormTalkUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!tsButAV.Enabled && flowLayoutPanel1.Controls.Count == 0)//如果视频对话中。。。
            {
                if (MessageBox.Show("如果关闭窗口，将会中止视频对话。是否关闭窗口？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    video1.CancelAV();//取消视频
                    IMLibrary3.Protocol.AVMsg msg = new IMLibrary3.Protocol.AVMsg();
                    msg.type = IMLibrary3.Protocol.type.cancel;
                    if (SendMsgToUser != null)//触发消息发送事件
                        SendMsgToUser(msg, User);
                    e.Cancel = false;
                } 
            }
            else if (tsButAV.Enabled && flowLayoutPanel1.Controls.Count > 0)
            {
                if (MessageBox.Show("如果关闭窗口，将会中止文件传输。是否关闭窗口？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    IMLibrary3.Protocol.P2PFileMsg pfile = new IMLibrary3.Protocol.P2PFileMsg();//文件传输协商协议
                    pfile.type = IMLibrary3.Protocol.type.cancel;//标记取消文件传输
                    pfile.MD5 = "";
                    if (SendMsgToUser != null)//触发消息发送事件
                        SendMsgToUser(pfile, User);
                    e.Cancel = false;
                }
            }
            else if (!tsButAV.Enabled && flowLayoutPanel1.Controls.Count > 0)
            {
                if (MessageBox.Show("如果关闭窗口，将会中止文件传输和视频对话。是否关闭窗口？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    video1.CancelAV();//取消视频
                    IMLibrary3.Protocol.AVMsg msg = new IMLibrary3.Protocol.AVMsg();
                    msg.type = IMLibrary3.Protocol.type.cancel;
                    if (SendMsgToUser != null)//触发消息发送事件
                        SendMsgToUser(msg, User);

                    IMLibrary3.Protocol.P2PFileMsg pfile = new IMLibrary3.Protocol.P2PFileMsg();//文件传输协商协议
                    pfile.type = IMLibrary3.Protocol.type.cancel;//标记取消文件传输
                    pfile.MD5 = "";
                    if (SendMsgToUser != null)//触发消息发送事件
                        SendMsgToUser(pfile, User);

                    e.Cancel = false;
                }
            } 
        }
        #endregion

        #endregion

        #region 公共方法

        #region 获取需要接收的图片文件集合
        public List<IMLibrary3.MyPicture> GetNeedRecPicture()
        {
            return MessagePanel1.needRecPictures;
        }
        #endregion

        #region 将消息加入 RichTextBox 控件
        /// <summary>
        /// 将消息加入 RichTextBox 控件
        /// </summary>
        /// <param name="msg">消息类</param>
        /// <param name="IsSend">标记是发送消息还是收到消息</param>
        public void MsgToRichTextBox(IMLibrary3.Protocol.Message msg, bool IsSend)//将发送的消息加入历史rich
        {
            string title =myUserName  +"(" + myUserID + ")";
            if (!IsSend)
                title = User.UserName + "(" + User.UserID + ")";

            this.MessagePanel1.MsgToRichTextBox(msg, IsSend, title);
        }
        #endregion

        #region 文件传输方法

        #region 发送文件单击事件
        void tsButSendFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sendFile(of.FileName);
            }
        }

        private void sendFile(string filename)
        {
            OurMsg.Controls.FileTransmit ft = new Controls.FileTransmit();
            ft.P2PFileTransmit = new p2pFileClient(Global.FileTransmitServerEP, filename);
            ft.fileTransmitCancel += new OurMsg.Controls.FileTransmit.fileTransmitEventHandler(ft_fileTransmitCancel);
            ft.P2PFileTransmit.GetIPEndPoint += new FileTransmitBase.GetIPEndPointEventHandler(P2PFileTransmit_GetIPEndPoint);
            ft.fileTransmitted += new OurMsg.Controls.FileTransmit.fileTransmitEventHandler(ft_fileTransmitted);
            flowLayoutPanel1.Controls.Add(ft);

            IMLibrary3.Protocol.P2PFileMsg pfile = new IMLibrary3.Protocol.P2PFileMsg();//文件传输协商协议
            pfile.type = IMLibrary3.Protocol.type.New;//标记发送新文件
            pfile.Name = ft.P2PFileTransmit.TFileInfo.Name;
            pfile.MD5 = ft.P2PFileTransmit.TFileInfo.MD5;
            pfile.Length = ft.P2PFileTransmit.TFileInfo.Length;
            pfile.Extension = ft.P2PFileTransmit.TFileInfo.Extension;

            if (SendMsgToUser != null)//触发消息发送事件
                SendMsgToUser(pfile, User);
        }

        void ft_fileTransmitted(object sender)
        {
            OurMsg.Controls.FileTransmit ft = sender as OurMsg.Controls.FileTransmit;
            if (ft.isSend)
                MessagePanel1.addRemarkTextToRecord(" 文件“" + ft.P2PFileTransmit.TFileInfo.Name + "”已经传输结束！");
            else
                MessagePanel1.addRemarkTextToRecord(" 文件“<file:\\\\" + ft.P2PFileTransmit.TFileInfo.fullName + ">”已经传输结束！");

            flowLayoutPanel1.Controls.Remove(ft);
            ft.Dispose();
            ft = null;
        }

        void ft_fileTransmitCancel(object sender)
        {
            OurMsg.Controls.FileTransmit ft = sender as OurMsg.Controls.FileTransmit;

            IMLibrary3.Protocol.P2PFileMsg pfile = new IMLibrary3.Protocol.P2PFileMsg();//文件传输协商协议
            pfile.type = IMLibrary3.Protocol.type.cancel ;//标记取消文件传输
            pfile.MD5 = ft.P2PFileTransmit.TFileInfo.MD5;

            if (SendMsgToUser != null)//触发消息发送事件
                SendMsgToUser(pfile, User);

            MessagePanel1.addRemarkTextToRecord(" 您取消了文件“" + ft.P2PFileTransmit.TFileInfo.Name + "”的传输！");

            flowLayoutPanel1.Controls.Remove(ft);
            ft.Dispose();
            ft = null;
        }

        void P2PFileTransmit_GetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            IMLibrary3.Protocol.P2PFileMsg pfile = new IMLibrary3.Protocol.P2PFileMsg();//文件传输协商协议
            pfile.type = IMLibrary3.Protocol.type.set;//标记要求对方设置当前用户的远程主机信息
            pfile.MD5 = (sender as p2pFileClient).TFileInfo.MD5;
            pfile.LocalIP = local.Address;
            pfile.LocalPort = local.Port;
            pfile.remoteIP = remote.Address;
            pfile.remotePort = remote.Port;

            if (SendMsgToUser != null)//触发消息发送事件
                SendMsgToUser(pfile, User);
        }
        #endregion

        #region 准备接收文件传输
        /// <summary>
        /// 准备接收文件传输
        /// </summary>
        /// <param name="pfile"></param>
        public void ReceiveFile(IMLibrary3.Protocol.P2PFileMsg  pfile)
        {
            TFileInfo tFileInfo = new TFileInfo();
            tFileInfo.Name = pfile.Name;
            tFileInfo.Length = pfile.Length;
            tFileInfo.Extension = pfile.Extension;
            tFileInfo.MD5 = pfile.MD5;

            OurMsg.Controls.FileTransmit ft = new Controls.FileTransmit();
            ft.P2PFileTransmit = new p2pFileClient(Global.FileTransmitServerEP, tFileInfo);
            ft.fileTransmitCancel += new OurMsg.Controls.FileTransmit.fileTransmitEventHandler(ft_fileTransmitCancel);
            ft.P2PFileTransmit.GetIPEndPoint += new FileTransmitBase.GetIPEndPointEventHandler(P2PFileTransmit_GetIPEndPoint);
            ft.fileTransmitted += new OurMsg.Controls.FileTransmit.fileTransmitEventHandler(ft_fileTransmitted);
            flowLayoutPanel1.Controls.Add(ft);
        }
        #endregion

        #region 取消文件传输
        /// <summary>
        /// 取消文件传输
        /// </summary>
        /// <param name="pfile"></param>
        public void CancelFile(IMLibrary3.Protocol.P2PFileMsg  pfile)
        {
            if (pfile.MD5 == "")
            {
                flowLayoutPanel1.Controls.Clear();
                MessagePanel1.addRemarkTextToRecord(" 对方取消了所有文件的传输！");
                return;
            }

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                OurMsg.Controls.FileTransmit ft = c as OurMsg.Controls.FileTransmit;
                if (ft != null && (ft.P2PFileTransmit.TFileInfo.MD5 == pfile.MD5))
                {
                    ft.CancelTransmit();
                    MessagePanel1.addRemarkTextToRecord(" 对方取消了文件“" + ft.P2PFileTransmit.TFileInfo.Name + "”的传输！");
                    flowLayoutPanel1.Controls.Remove(ft);
                    ft.Dispose();
                    ft = null;
                }
            }
        }
        #endregion

        #region 设置文件传输远程主机信息
        /// <summary>
        /// 设置文件传输远程主机信息
        /// </summary>
        public void setFileRometEP(IMLibrary3.Protocol.P2PFileMsg  pfile)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                OurMsg.Controls.FileTransmit ft = c as OurMsg.Controls.FileTransmit;
                if (ft != null && ft.P2PFileTransmit.TFileInfo.MD5 == pfile.MD5)
                {
                    ft.P2PFileTransmit.Start(true);//发送方参数须设置为真
                    ft.P2PFileTransmit.setRemoteIP(new IPEndPoint(pfile.LocalIP ,pfile.LocalPort ),
                                                   new IPEndPoint(pfile.remoteIP ,pfile.remotePort));//设置文件传输远程主机信息
                     
                } 
            }
        }
        #endregion 

        #endregion

        #region 视频
        /// <summary>
        /// 准备视频
        /// </summary>
        /// <param name="IsInvite">是否邀请方</param>
        public void ReadyAV(bool IsInvite)
        {
            video1.IsInvite = IsInvite;
            tsButAV.Enabled = false;
            panelDynamicInfo.Width = 345;
            xPanderPanel1.Expand = false;
            xPanderPanel2.Visible = true;
            xPanderPanel2.Expand = true;
            xPanderPanelList1.Refresh();
        }

        /// <summary>
        /// 取消视频
        /// </summary>
        /// <param name="IsSelf">是否自己取消</param>
        public void CancelAV(bool IsSelf)
        {
            if (!IsSelf)
                this.video1.CancelAV();//释放视频资源

            tsButAV.Enabled = true;

            xPanderPanel2.Visible = false;
            if (!xPanderPanel1.Visible)
                panelDynamicInfo.Width = 150;
            else
            {
                panelDynamicInfo.Width = 245;
                xPanderPanel1.Expand = true;
            }
            xPanderPanelList1.Refresh();
        }

        #region 设置视频对话远程主机信息
        /// <summary>
        /// 设置视频对话远程主机信息
        /// </summary>
        public void setAVRometEP(IMLibrary3.Protocol.AVMsg msg)
        {
            video1.SetRometEP(new IPEndPoint(msg.LocalIP, msg.LocalPort), new IPEndPoint(msg.remoteIP, msg.remotePort));
        }
        #endregion 

        #region 设置对方视频位图信息，以便构造解码器
        /// <summary>
        /// 设置对方视频位图信息，以便构造解码器
        /// </summary>
        /// <param name="msg"></param>
        public void SetVideoBitmapinfo(IMLibrary3.Protocol.AVMsg msg)
        {
            IMLibrary.AV.BITMAPINFO bitmapinfo = new IMLibrary.AV.BITMAPINFO();
            bitmapinfo.bmiHeader.biBitCount = msg.biBitCount;
            bitmapinfo.bmiHeader.biClrImportant = msg.biClrImportant;
            bitmapinfo.bmiHeader.biClrUsed = msg.biClrUsed;
            bitmapinfo.bmiHeader.biCompression = msg.biCompression;
            bitmapinfo.bmiHeader.biHeight = msg.biHeight;
            bitmapinfo.bmiHeader.biPlanes = msg.biPlanes;
            bitmapinfo.bmiHeader.biSize = msg.biSize;
            bitmapinfo.bmiHeader.biSizeImage = msg.biSizeImage;
            bitmapinfo.bmiHeader.biWidth = msg.biWidth;
            bitmapinfo.bmiHeader.biXPelsPerMeter = msg.biXPelsPerMeter;
            bitmapinfo.bmiHeader.biYPelsPerMeter = msg.biYPelsPerMeter;
            video1.SetVideoBitmapinfo(bitmapinfo);
        }
        #endregion

        #endregion

        #endregion
    }
}
