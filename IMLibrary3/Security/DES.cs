using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IMLibrary3.Security
{
    /// <summary>
    /// 字符串加解密
    /// </summary>
    public sealed class DES
    {
        //默认密钥向量
        private static byte[] IV = { 0x65, 0x88, 0x35, 0x71, 0x60, 0x1B, 0x2D, 0x7F };
        private static string key = "ligy@163";
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string Encrypt(string toEncryptString)
        {
            try
            {
                if (toEncryptString == null || toEncryptString == "" || toEncryptString == string.Empty) return "";
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] rgbIV = IV;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(toEncryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return toEncryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="toDecryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string Decrypt(string toDecryptString)
        {
            try
            {
                if (toDecryptString == null || toDecryptString == "" || toDecryptString == string.Empty) return "";
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = IV;
                byte[] inputByteArray = Convert.FromBase64String(toDecryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return toDecryptString;
            }
        }

        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="toEncryptString">明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="IV">向量</param>
        /// <returns>密文</returns>
        public static string Encrypt(string toEncryptString, string Key, string IV)
        {
            //把密钥转换成字节数组
            byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(Key);
            //把向量转换成字节数组
            byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(IV);
            //声明1个新的DES对象
            DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
            //开辟一块内存流
            MemoryStream msEncrypt = new MemoryStream();
            //把内存流对象包装成加密流对象
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Write);
            //把加密流对象包装成写入流对象
            StreamWriter swEncrypt = new StreamWriter(csEncrypt);
            //写入流对象写入明文
            swEncrypt.WriteLine(toEncryptString);
            //写入流关闭
            swEncrypt.Close();
            //加密流关闭
            csEncrypt.Close();
            //把内存流转换成字节数组，内存流现在已经是密文了
            byte[] bytesCipher = msEncrypt.ToArray();
            //内存流关闭
            msEncrypt.Close();
            //把密文字节数组转换为字符串，并返回
            return Encoding.UTF8.GetString(bytesCipher);
        }


        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="toDecryptString">密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="IV">向量</param>
        /// <returns>明文</returns>
        public static string Decrypt(string toDecryptString, string Key, string IV)
        {
            //把密钥转换成字节数组
            byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(Key);
            //把向量转换成字节数组
            byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(IV);
            //把密文转换成字节数组
            byte[] bytesCipher = UnicodeEncoding.Unicode.GetBytes(toDecryptString);
            //声明1个新的DES对象
            DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
            //开辟一块内存流，并存放密文字节数组
            MemoryStream msDecrypt = new MemoryStream(bytesCipher);
            //把内存流对象包装成解密流对象
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, desDecrypt.CreateDecryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Read);
            //把解密流对象包装成读出流对象
            StreamReader srDecrypt = new StreamReader(csDecrypt);
            //明文=读出流的读出内容
            string strPlainText = srDecrypt.ReadLine();
            //读出流关闭
            srDecrypt.Close();
            //解密流关闭
            csDecrypt.Close();
            //内存流关闭
            msDecrypt.Close();
            //返回明文
            return strPlainText;
        }

    }
}
