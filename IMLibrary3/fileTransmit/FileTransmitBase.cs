using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

#region IMLibrary3
using IMLibrary3.Enmu;
using IMLibrary3.Security;
using IMLibrary3.Organization;
using IMLibrary3.Operation;
using IMLibrary3.Protocol;

using IMLibrary3.Net;
using IMLibrary3.Net.TCP;
#endregion


namespace IMLibrary3 
{
    /// <summary>
    /// 文件传输基础类
    /// </summary>
    public class FileTransmitBase
    {
        #region 构造
        /// <summary>
        /// 构造(上传文件)
        /// </summary>
        /// <param name="serverEP">服务器主机信息</param>
        /// <param name="fullFileName">上传文件（含路径）</param>
        public FileTransmitBase(IPEndPoint serverEP, string fullFileName)
        {
            FileInfo f = new FileInfo(fullFileName);
            if (f.Exists)
            {
                TFileInfo.Name = f.Name;
                TFileInfo.fullName = fullFileName;
                TFileInfo.Extension = f.Extension;
                TFileInfo.Length = f.Length;
                TFileInfo.LengthStr = IMLibrary3.Operation.Calculate.GetSizeStr(f.Length);
                TFileInfo.MD5 = IMLibrary3.Security.Hasher.GetMD5Hash(fullFileName);
            }

            ServerEP = serverEP;
            _IsSend = true;

        }

        /// <summary>
        /// 构造(下载文件)
        /// </summary>
        /// <param name="serverEP">服务器主机信息</param>
        /// <param name="tFileInfo">下载文件信息</param>
        public FileTransmitBase(IPEndPoint serverEP, TFileInfo tFileInfo)
        {
            TFileInfo = tFileInfo;
            TFileInfo.LengthStr = Operation.Calculate.GetSizeStr(TFileInfo.Length);//计算文件长度
            ServerEP = serverEP;
            _IsSend = false;

            ///创建接收文件夹
            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\ReceivedFile");
            if (!dInfo.Exists)
                dInfo.Create();
            dInfo = null;

            ///创建接收缓存文件夹
            dInfo = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\FileCache");
            if (!dInfo.Exists)
                dInfo.Create();
            dInfo = null;
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (tcpClient != null)
            {
                if (tcpClient.IsConnected)
                    tcpClient.Disconnect();
                tcpClient.Dispose();
            }

            if (sockUDP != null && sockUDP.Listened)
            {
                sockUDP.CloseSock();
                sockUDP = null;
            }

            if (timer1 != null)
                this.timer1.Enabled = false;
            this.timer1 = null;
             
            if (FS != null)
            { 
                 FS.Close(); FS.Dispose(); FS = null;
            }

            this.TFileInfo = null;
            this.fileBlock = null;
        }
        #endregion
         
        #region 变量
        /// <summary>
        /// UDP客户端
        /// </summary>
        protected SockUDP sockUDP = null;

        /// <summary>
        /// TCP客户端
        /// </summary>
        protected TCPClient tcpClient = null;

        /// <summary>
        /// 代理服务器主机信息
        /// </summary>
        protected IPEndPoint ServerEP = null;
        /// <summary>
        /// 本端公网主机信息
        /// </summary>
        protected IPEndPoint myRemoteEP = null;
        /// <summary>
        /// 对端主机信息
        /// </summary>
        protected IPEndPoint toEP = null;
 
        /// <summary>
        /// 标记文件是否在传输过程中
        /// </summary>
        protected bool isTransmit = false; 
        /// <summary>
        /// 当前获得文件的数据长度
        /// </summary>
        protected long currGetPos = 0;

        /// <summary>
        /// 传输文件的信息
        /// </summary>
        public TFileInfo TFileInfo = new TFileInfo();


        /// <summary>
        /// 标识文件是否已经传输完成
        /// </summary>
        protected bool IsConnected = false;

        /// <summary>
        /// 超时检测器
        /// </summary>
        protected System.Timers.Timer timer1;

        /// <summary>
        /// 缓冲文件保存位置
        /// </summary>
        protected string CacheFile = "";

        /// <summary>
        /// 文件操作流
        /// </summary>
        protected FileStream FS = null;

        /// <summary>
        /// 当前收到的数据包长度,断点续传的继点记忆
        /// </summary>
        protected  int  CurrRecLength=0;
        #endregion

        #region 事件

        /// <summary>
        /// 文件传输结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void fileTransmitEventHandler(object sender, fileTransmitEvnetArgs e);



        /// <summary>
        /// 文件传输结束
        /// </summary>
        public event fileTransmitEventHandler fileTransmitted;
        /// <summary>
        /// 触发文件传输结束事件
        /// </summary>
        protected virtual void  OnFileTransmitted()
        {
            if(fileTransmitted!=null)
                fileTransmitted(this,new fileTransmitEvnetArgs(TFileInfo));
        }


        /// <summary>
        /// 允许断点续传
        /// </summary>
        public event fileTransmitEventHandler fileAllowResume;
        /// <summary>
        /// 触发允许文件断点续传事件
        /// </summary>
        protected virtual void OnFileAllowResume()
        {
            if (fileAllowResume != null)
                fileAllowResume(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 取消文件传输
        /// </summary>
        public event fileTransmitEventHandler fileTransmitCancel;
        /// <summary>
        /// 触发文件传输取消事件
        /// </summary>
        protected virtual void OnFileTransmitCancel()
        {
            if (fileTransmitCancel != null)
                fileTransmitCancel(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 文件传输开始
        /// </summary>
        public event fileTransmitEventHandler fileTransmitBefore;
        /// <summary>
        /// 触发文件传输前事件
        /// </summary>
        protected virtual void OnFileTransmitBefore()
        {
            if (fileTransmitBefore != null)
                fileTransmitBefore(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        ///  文件传输超时 
        /// </summary>
        public event fileTransmitEventHandler fileTransmitOutTime;
        /// <summary>
        /// 触发文件传输超时事件
        /// </summary>
        protected virtual void OnFileTransmitOutTime()
        {
            if (fileTransmitOutTime != null)
                fileTransmitOutTime(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 文件传输错误事件
        /// </summary>
        public event fileTransmitEventHandler fileTransmitError;
        /// <summary>
        /// 触发文件传输错误事件
        /// </summary>
        protected virtual void OnFileTransmitError()
        {
            if (fileTransmitError != null)
                fileTransmitError(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 事件：发送或收到文件数据
        /// </summary>
        public event fileTransmitEventHandler fileTransmitting;
        /// <summary>
        /// 触发文件传输中事件
        /// </summary>
        protected virtual void OnFileTransmitting()
        {
            if (fileTransmitting != null)
                fileTransmitting(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 文件传输成功联接到服务器事件
        /// </summary>
        public event fileTransmitEventHandler fileTransmitConnected;
        /// <summary>
        /// 触发文件传输成功联接到服务器事件
        /// </summary>
        protected virtual void OnFileTransmitConnected()
        {
            if (fileTransmitConnected != null)
                fileTransmitConnected(this, new fileTransmitEvnetArgs(TFileInfo));
        }

        /// <summary>
        /// 获得IPEndPoint事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="local">本地主机信息</param>
        /// <param name="remote">远程主机信息</param>
        public delegate void GetIPEndPointEventHandler(object sender, IPEndPoint local, IPEndPoint remote);
        public event GetIPEndPointEventHandler GetIPEndPoint;
        protected virtual void OnGetIPEndPoint(object sender, IPEndPoint local, IPEndPoint remote)
        {
            if (GetIPEndPoint != null)
                GetIPEndPoint(this, local, remote);//触发获取本机主机事件
        }
        #endregion

        #region 属性

        /// <summary>
        /// 每次发送的数据包缓冲容量
        /// </summary>
        protected int mtu  //标记一次传输文件数据块的大小，不能超过网络最大传输单元 MTU 576-1492 限制，否则在因特网上的数据发送将不成功
        {
            set { buffer = new byte[value]; }
            get { return buffer.Length; }
        }

        /// <summary>
        /// 要发送的缓冲区
        /// </summary>
        protected byte[] buffer = new byte[0];

        /// <summary>
        ///  一次读写文件的缓冲区大小，定义为5M
        /// </summary>
        protected byte[] fileBlock = null ;

        private int _maxReadWriteFileBlock = 1024000 *2;//一次读写文件的大小2M
        /// <summary>
        /// 一次读写文件的大小
        /// </summary>
        public int maxReadWriteFileBlock
        {
            set { _maxReadWriteFileBlock = value ; }
            get { return _maxReadWriteFileBlock; }
        }


        private int outTime = 3;
        /// <summary>
        /// 超时秒数
        /// </summary>
        public int OutTime
        {
            set { outTime = value; }
            get { return outTime; }
        }

         bool _IsSend = false;
        /// <summary>
        /// 标记本机是发送端还是接收端
        /// </summary>
        public bool IsSend
        {
            get {return  _IsSend; }
        }
        #endregion

        #region 方法

        #region 判断当前是否需要读写文件
        /// <summary>
        /// 判断当前是否需要读写文件
        /// </summary>
        /// <returns></returns>
        protected bool IsReadWriteFile()
        {
            if (CurrRecLength  % maxReadWriteFileBlock == 0)
                return true;
            else
                return false;
        }
        #endregion

        #endregion
    }
}
