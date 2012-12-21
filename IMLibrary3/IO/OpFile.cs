using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IMLibrary3.IO
{
    #region 文件操作对像
    /// <summary>
    /// 文件操作
    /// </summary>
    public sealed class OpFile
    {
        /// <summary>
        /// 将文件一次性读入到内存
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static byte[] Read(string fileName)
        {
            byte[] data = null;

            if (File.Exists(fileName))
            {
                FileInfo f = new FileInfo(fileName);
                data = new byte[f.Length];
                ////////////////////////文件操作
                FileStream fw = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fw.Seek(0, SeekOrigin.Begin);
                fw.Read(data, 0, data.Length);
                fw.Close();
                fw.Dispose();
                ///////////////////////////
            }
            return data;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static bool Write(byte[] data, string fileName)
        {
            if (File.Exists(fileName)) return false;//如果文件存在，则返回
            ////////////////////////文件操作
            FileStream fw = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
            fw.Write(data, 0, data.Length);
            fw.Close();
            fw.Dispose();
            ///////////////////////////
            return true;
        }
    }
    #endregion
}
