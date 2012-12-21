using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Security
{
    /// <summary>
    /// 哈希类
    /// </summary>
    public sealed class Hasher
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        private Hasher() { }

        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>返回字节数组</returns>
        private static byte[] ConvertStringToByteArray(string data)
        {
            return (new System.Text.UnicodeEncoding()).GetBytes(data);
        }

        /// <summary>
        /// 获得文件流
        /// </summary>
        /// <param name="pathName">文件路径（包括路径）</param>
        /// <returns>返回文件流</returns>
        private static System.IO.FileStream GetFileStream(string pathName)
        {
            return (new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                      System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite));
        }

        /// <summary>
        /// 获得文件SHA1值
        /// </summary>
        /// <param name="pathName">文件名（包括路径）</param>
        /// <returns>返回文件SHA1值</returns>
        public static string GetSHA1Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";

            byte[] arrbytHashValue;
            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.SHA1CryptoServiceProvider oSHA1Hasher =
                       new System.Security.Cryptography.SHA1CryptoServiceProvider();

            try
            {
                oFileStream = GetFileStream(pathName);
                arrbytHashValue = oSHA1Hasher.ComputeHash(oFileStream);
                oFileStream.Close();

                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",System.Exception ex
                //         System.Windows.Forms.MessageBoxButtons.OK,
                //         System.Windows.Forms.MessageBoxIcon.Error,
                //         System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }

            return (strResult);
        }

        /// <summary>
        /// 获得文件MD5值
        /// </summary>
        /// <param name="pathName">文件名（包括路径）</param>
        /// <returns>返回文件MD5值</returns>
        public static string GetMD5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";

            byte[] arrbytHashValue;
            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                       new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {
                oFileStream = GetFileStream(pathName);
                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);
                oFileStream.Close();

                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",System.Exception ex
                //           System.Windows.Forms.MessageBoxButtons.OK,
                //           System.Windows.Forms.MessageBoxIcon.Error,
                //           System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }

            return (strResult);
        }

        /// <summary>
        /// 获得字节数组MD5值
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>返回MD5值</returns>
        public static string GetMD5Hash(byte[] data)
        {
            string strResult = "";
            string strHashData = "";

            byte[] arrbytHashValue;


            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                       new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {

                arrbytHashValue = oMD5Hasher.ComputeHash(data);

                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",System.Exception ex
                //           System.Windows.Forms.MessageBoxButtons.OK,
                //           System.Windows.Forms.MessageBoxIcon.Error,
                //           System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }

            return (strResult);
        }
    }
}
