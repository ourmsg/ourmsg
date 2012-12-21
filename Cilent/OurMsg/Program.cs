using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OurMsg
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(LoadFace));//加载表情图片
            t.Start();// 加载表情图片线程
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OurMsg.FormMain());
            ///初始化文件服务器主机信息
            //Application.Run(new OurMsg.Forms.Form1());
        }

        #region 加载表情图片线程
        /// <summary>
        /// 加载表情图片线程
        /// </summary>
        private static void LoadFace()
        {
            Global.ImageListFace.ImageSize = new System.Drawing.Size(24, 24);
            Global.ImageListFace.TransparentColor = System.Drawing.Color.White ;
            ///初始化表情
            for (int i = 0; i < 99; i++)
            {
                try
                {
                    string fileNamePath = Application.StartupPath + @"\face\" + i.ToString() + ".gif";
                    Global.ImageListFace.Images.Add(fileNamePath, System.Drawing.Image.FromFile(fileNamePath));
                }
                catch { }
            }
        }
        #endregion
    }
}
