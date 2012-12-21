using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using IMLibrary3  ;
using IMLibrary3.Controls ;

namespace OurMsg.Controls
{
    public partial class FileTransmit : UserControl
    {
        public FileTransmit()
        {
            InitializeComponent();
            linkLabelCancel.Click += new EventHandler(linkLabelCancel_Click);
            linkLabelReceive.Click += new EventHandler(linkLabelReceive_Click);
            linkLabelResume.Click += new EventHandler(linkLabelResume_Click);
            linkLabelSaveAs.Click += new EventHandler(linkLabelSaveAs_Click);
            linkLabelOffline.Click += new EventHandler(linkLabelOffline_Click);
        }
 
        void linkLabelOffline_Click(object sender, EventArgs e)
        {
             
        } 

        #region 属性
        p2pFileClient  _P2PFileTransmit = null;
        /// <summary>
        /// P2P文件传输组件
        /// </summary>
        public p2pFileClient  P2PFileTransmit
        {
            set
            {
                _P2PFileTransmit = value;
                if (value != null)
                {
                    this.labelFileLengthString.Text = value.TFileInfo.LengthStr;
                    this.labelFileName.Text = value.TFileInfo.Name;
                    this.isSend = value.IsSend;

                    if (isSend)
                        this.icon = FileIcon.GetFileIcon(value.TFileInfo.fullName).ToBitmap();
                    else
                        this.icon = FileIcon.GetFileIcon(value.TFileInfo.Extension).ToBitmap();

                    value.fileTransmitConnected += new FileTransmitBase.fileTransmitEventHandler(value_fileTransmitConnected);
                    value.fileTransmitting += new FileTransmitBase.fileTransmitEventHandler(value_fileTransmitting);
                    value.fileTransmitOutTime += new FileTransmitBase.fileTransmitEventHandler(value_fileTransmitOutTime);
                    value.fileTransmitted += new FileTransmitBase.fileTransmitEventHandler(value_fileTransmitted);
                    value.fileAllowResume += new FileTransmitBase.fileTransmitEventHandler(value_fileAllowResume);

                    if (!isSend)
                    {
                        string CacheFile = "FileCache\\" + value.TFileInfo.MD5;
                        if (File.Exists(CacheFile))//如果缓存文件存在，则触发可断点续传事件
                        {
                            this.linkLabelReceive.Visible = false;
                            this.linkLabelResume.Visible = true;
                        }
                    }

                }
            }
            get { return _P2PFileTransmit; }
        }

        void value_fileAllowResume(object sender, fileTransmitEvnetArgs e)
        {
            this.linkLabelReceive.Visible = false;
            this.linkLabelResume .Visible = true ;
        }


        bool _isSend = false;
        /// <summary>
        /// 发送方
        /// </summary>
        public bool isSend
        {
            set
            {
                _isSend = value;
                if (value)
                {
                    this.labelRequest.Text = "发送文件请求：";
                    this.linkLabelReceive.Visible = false;
                    this.linkLabelSaveAs.Visible = false;
                    this.linkLabelOffline.Visible = true;
                }
                else
                {
                    this.labelRequest.Text = "接收文件请求：";
                }
            }
            get { return _isSend; }
        }

        public Image icon
        {
            set
            {
                if (value != null)
                    pictureBox1.Image = value;
            }
            get { return pictureBox1.Image; }
        }
        #endregion

        #region 文件传输事件
        public delegate void fileTransmitEventHandler(object sender);
        /// <summary>
        /// 取消文件传输
        /// </summary>
        public event fileTransmitEventHandler fileTransmitCancel;
        /// <summary>
        /// 文件传输结束
        /// </summary>
        public event fileTransmitEventHandler fileTransmitted;
        /// <summary>
        /// 文件传输状态
        /// </summary>
        private enum TransmitState
        {
            Connected,
            Transmitted,
            OutTime,
            Transmitting,
            Over,
        }

        void value_fileTransmitted(object sender, fileTransmitEvnetArgs e)
        {
            delegateSetRecControl d = new delegateSetRecControl(SetRecControl);
            this.Invoke(d, TransmitState.Over, e.fileInfo);
        }

        void value_fileTransmitOutTime(object sender, fileTransmitEvnetArgs e)
        {

        }

        void value_fileTransmitting(object sender, fileTransmitEvnetArgs e)
        {
            delegateSetRecControl d = new delegateSetRecControl(SetRecControl);
            this.Invoke(d, TransmitState.Transmitting, e.fileInfo);
        }

        delegate void delegateSetRecControl(TransmitState State, TFileInfo fileinfo);
        private void SetRecControl(TransmitState State, TFileInfo fileinfo)
        {
            if (State == TransmitState.Transmitting)
            {
                this.progressBar1.Value = Convert.ToInt32(((decimal)fileinfo.CurrLength / fileinfo.Length) * 100);
                this.labelFileLengthString.Text = IMLibrary3.Operation.Calculate.GetSizeStr(fileinfo.CurrLength) + "/" + fileinfo.LengthStr;
            }
            else if (State == TransmitState.Connected)
            {
                linkLabelOffline.Visible = false;

                if (isSend)
                    this.labelRequest.Text = "正在发送文件...";
                else
                    this.labelRequest.Text = "正在接收文件...";

                if (fileinfo.connectedType == ConnectedType.UDPLocal)
                    labelConType.Text = "[UDP直连]";
                else if (fileinfo.connectedType == ConnectedType.UDPRemote)
                    labelConType.Text = "[UDP直连]";
                else
                    labelConType.Text = "[服务器中转连接]";
            }
            else if (State == TransmitState.Over)
            {
                if (fileTransmitted != null) fileTransmitted(this);//触发文件
            }
        }


        void value_fileTransmitConnected(object sender, fileTransmitEvnetArgs e)
        {
            try
            {
                delegateSetRecControl d = new delegateSetRecControl(SetRecControl);
                this.Invoke(d, TransmitState.Connected, e.fileInfo);
            }
            catch
            {

            }
        }
        #endregion
         
        #region 接收文件单击事件
        void linkLabelSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = P2PFileTransmit.TFileInfo.Name;
            sf.Filter = "*" + P2PFileTransmit.TFileInfo.Extension + "|";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (sf.FileName.IndexOf(P2PFileTransmit.TFileInfo.Extension, 0) < 0)
                    P2PFileTransmit.TFileInfo.fullName = sf.FileName + P2PFileTransmit.TFileInfo.Extension;
                else
                    P2PFileTransmit.TFileInfo.fullName = sf.FileName;

                P2PFileTransmit.Start(true);//接收文件，并执行断点续传

                linkLabelSaveAs.Visible = false;
                linkLabelReceive.Visible = false;
                linkLabelResume.Visible = false;
            }

        }

        void linkLabelReceive_Click(object sender, EventArgs e)
        {
            linkLabelSaveAs.Visible = false;
            linkLabelReceive.Visible = false;

            P2PFileTransmit.TFileInfo.fullName = Application.StartupPath + "\\ReceivedFile\\" + P2PFileTransmit.TFileInfo.Name;
            P2PFileTransmit.Start(false);//接收文件，不执行断点续传 

        }
        #endregion

        #region 取消文件传输
        void linkLabelCancel_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定要取消文件“" + P2PFileTransmit.TFileInfo.Name + "”传输吗?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
                CancelTransmit();
                if (fileTransmitCancel != null)
                    fileTransmitCancel(this);
            //}
        }
        #endregion

        #region 续传单击事件
        private void linkLabelResume_Click(object sender, EventArgs e)
        {
            linkLabelSaveAs.Visible = false;
            linkLabelReceive.Visible = false;
            linkLabelResume.Visible = false;

            P2PFileTransmit.TFileInfo.fullName = Application.StartupPath + "\\ReceivedFile\\" + P2PFileTransmit.TFileInfo.Name;
            P2PFileTransmit.Start(true);//接收文件，执行断点续传 
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 取消文件传输
        /// </summary>
        public void CancelTransmit()
        {
            P2PFileTransmit.CancelTransmit();//取消文件传输
        }
        #endregion

   
    }
}
