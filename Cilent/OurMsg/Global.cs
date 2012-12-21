using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO ;
using System.Diagnostics;
using System.Reflection;
using System.Net;


namespace OurMsg
{
    /// <summary>
    /// 全局访问类
    /// </summary>
    public sealed class Global
    {
        #region 变量
        /// <summary>
        /// 服务器域名 
        /// </summary>
        public static string ServerDomain ="127.0.0.1";
      
        /// <summary>
        /// 消息服务器端口
        /// </summary>
        public static int ServerMsgPort = 6500;

        /// <summary>
        /// 服务器文件服务主机信息
        /// </summary>
        public static IPEndPoint FileTransmitServerEP=null;
        /// <summary>
        /// 
        /// </summary>
        public static IPEndPoint AVTransmitServerEP = null;

        /// <summary>
        /// 图片服务器主机信息
        /// </summary>
        public static IPEndPoint ImageServerEP = null;

        /// <summary>
        /// 会议服务器 
        /// </summary>
        public static string ServerConference = "@";

        /// <summary>
        /// 表情图片列表缓存
        /// </summary>
        public static System.Windows.Forms.ImageList ImageListFace = new ImageList();

        /// <summary>
        /// 消息管理窗
        /// </summary>
        public static  FormDataManage FormDataManageage = null;
 
        #endregion
         
        #region 判断系统进程中程序是否已经运行有进行，有返回进程，无返回空值
        /// <summary>
        /// 判断系统进程中程序是否已经运行有进行，有返回进程，无返回空值
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name
            foreach (Process process in processes)
            {
                //Ignore the current process
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.

                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.
                        return process;
                    }
                }
            }
            return null;
        }
        #endregion

    }
}
