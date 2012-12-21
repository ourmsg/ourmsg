using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace IMLibrary3
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Global
    {
        /// <summary>
        /// 资源文件管理器
        /// </summary>
        public static System.Resources.ResourceManager ResMan = new System.Resources.ResourceManager("IMLibrary3.Resource1", System.Reflection.Assembly.GetExecutingAssembly());

        #region 显示消息
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        public static void MsgShow(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
