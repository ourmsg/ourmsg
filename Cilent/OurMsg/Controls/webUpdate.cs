using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IMLibrary3.Components;
using IMLibrary3;

namespace OurMsg.controls
{
    public partial class webUpdate : UserControl
    {
        public webUpdate()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 版本号文件的URI
        /// </summary>
        string VersionFileURI = "http://www.ourmsg.net/version.txt";

        /// <summary>
        /// 当前要下载的升级包web地址
        /// </summary>
        string FileURI = "http://www.ourmsg.net/update.exe";

        /// <summary>
        /// 下载升级包文件要保存的位置
        /// </summary>
        string SaveAsFileName = Application.StartupPath + "\\update.exe";

        /// <summary>
        /// 下载文件的长度
        /// </summary>
        int FileLen = 0;

        /// <summary>
        /// 当前已经下载的文件长度
        /// </summary>
        int currTransmittedLen = 0;

        /// <summary>
        /// 标识当前是否在下载
        /// </summary>
        bool isDownLoad = false;


        /// <summary>
        /// 下载线程
        /// </summary>
        Thread th;
        #endregion

        #region 开始下载更新升级包
        /// <summary>
        /// 开始下载更新升级包
        /// </summary>
        public void DownLoad()
        {
            System.Net.WebClient webC = new System.Net.WebClient();//用于读取文件最新版信息的WebClient
            string versionStr = "";

            try
            {
                versionStr = webC.DownloadString(this.VersionFileURI);
            }
            catch { }

            if (versionStr.Trim() == "")//如果没有获得升级文件版本信息
            {
                webC.Dispose();
                this.webFile1.Dispose();
                this.Dispose();
            }
            else
            {
                webC.Dispose();
                if (versionStr.Trim() != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())//如果软件版本与现有的客户端不同，则下载最新版本
                    if (MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                        + "已有更新版本，是否下载并安装？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Visible = true;
                        this.Width = 334;
                        this.Height = 154;
                        this.label1.Text = "正在下载更新程序，请稍等...";
                        this.timer1.Enabled = true;//开始执行下载最新版升级程序任务
                    }
                    else
                    {
                        this.webFile1.Dispose();
                        this.Dispose();
                    }
                else
                {
                    this.webFile1.Dispose();
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// 下载升级包
        /// </summary>
        /// <param name="fileURI">文件所在WEB 服务器 URL</param>
        /// <param name="saveAsFileName">文件要保存的位置及文件名</param>
        //public void downLoad(string fileURI,string saveAsFileName)
        //{

        //} 
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.isDownLoad)
            {
                this.isDownLoad = true;
                try
                {
                    this.FileLen = (int)this.webFile1.getDownloadFileLen(this.FileURI);//获得要下载文件的长度
                }
                catch { }
                if (this.FileLen > 0)//如果文件存在，则下载
                {
                    this.ProgressBar1.Maximum = this.FileLen;
                    th = new Thread(new ThreadStart(BeginDownLoad));
                    th.Start();
                }
                else
                {
                    this.timer1.Enabled = false;//停止状态检测
                    this.webFile1.Dispose();
                    this.Dispose();
                }
            }
            else
            {
                this.ProgressBar1.Value = this.currTransmittedLen;
                if (this.currTransmittedLen == this.FileLen)//如果已经下载完成
                {
                    this.timer1.Enabled = false;//停止状态检测
                    System.Diagnostics.Process.Start(SaveAsFileName);//执行升级程序
                }
            }
        }

        private void BeginDownLoad()
        {
            this.webFile1.DownloadFile(this.FileURI, this.SaveAsFileName);
        }

        private void webFile1_fileTransmitting(object sender, fileTransmitEvnetArgs e)
        {
            this.currTransmittedLen = (int)e.fileInfo.CurrLength;//获得当前已经下载的文件长度
        }

        private void webFile1_fileTransmitted(object sender, fileTransmitEvnetArgs e)
        {

        }

        private void webFile1_fileTransmitError(object sender, fileTransmitEvnetArgs e)
        {
            MessageBox.Show(e.fileInfo.Message);
        }

    }
}
