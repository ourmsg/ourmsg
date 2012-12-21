using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Operation
{
    /// <summary>
    /// 文本编码类 
    /// </summary>
    public sealed class TextEncoder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TextEncoder()
        {
        }
        /// <summary>
        /// 将文本转换成字节数组
        /// </summary>
        /// <param name="text">要转换的文本</param>
        /// <returns>返回转换后的字节数组</returns>
        public static byte[] textToBytes(string text)
        {
            byte[] buf = new byte[0];
            if (!string.IsNullOrEmpty(text))
                buf = System.Text.Encoding.Default.GetBytes(text);
            return buf;
        }
        /// <summary>
        /// 将字节数组转换成文本
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>返回转换后的文本</returns>
        public static string bytesToText(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// 将字节数组转换成int数据类型
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>返回转换后的int</returns>
        public static int bytesToInt(byte[] bytes)
        {
            int i = 0;
            try
            {
                i = System.BitConverter.ToInt32(bytes, 0);
            }
            catch { }
            return i;
        }

        /// <summary>
        /// 将字节数组转换成int数据类型
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>返回转换后的int</returns>
        public static UInt32 bytesToUInt32(byte[] bytes)
        {
            uint i = 0;
            try
            {
                i = System.BitConverter.ToUInt32(bytes, 0);
            }
            catch { }
            return i;
        }

        /// <summary>
        /// 将int数据类型转换成字节数组
        /// </summary>
        /// <param name="i">要转换的int数据</param>
        /// <returns>返回转换后的字节数组</returns>
        public static byte[] intToBytes(int i)
        {
            return System.BitConverter.GetBytes(i);
        }


        /// <summary>
        /// 将int数据类型转换成字节数组
        /// </summary>
        /// <param name="i">要转换的int数据</param>
        /// <returns>返回转换后的字节数组</returns>
        public static byte[] uintToBytes(uint i)
        {
            return System.BitConverter.GetBytes(i);
        }
    }
}
