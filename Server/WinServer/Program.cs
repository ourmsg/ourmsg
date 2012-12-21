using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OurMsgServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine(IMLibrary3.Security.Hasher.GetMD5Hash(IMLibrary3.Operation.TextEncoder.textToBytes("123456")));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

 
    }



}
