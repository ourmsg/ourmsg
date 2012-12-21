using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IMLibrary3.Operation
{
    /// <summary>
    /// 公共计算类
    /// </summary>
    sealed public class Calculate
    {
        #region 获得文件尺寸字符串 GetSizeStr()
        private const float GB = 1024 * 1024 * 1024;
        private const float MB = 1024 * 1024;
        private const float KB = 1024;

        /// <summary>
        /// 获得文件的尺寸字符串
        /// </summary>
        /// <param name="fileSize">文件长度(当前版本只支持2G)</param>
        /// <returns></returns>
        public static string GetSizeStr(long fileSize)
        {
            try
            {
                float TempSize = fileSize / GB;
                if (TempSize > 1)
                {
                    return TempSize.ToString("0.00") + "GB";
                }

                TempSize = fileSize / MB;
                if (TempSize > 1)
                {
                    return TempSize.ToString("0.00") + "MB";
                }

                TempSize = fileSize / KB;
                if (TempSize > 1)
                {
                    return TempSize.ToString("0.00") + "KB";
                }
                return fileSize + "字节";
            }
            catch { return fileSize + "字节"; }
        }
        #endregion

        #region 计算时间差
        /// <summary>
        /// 计算时间差,返回秒差
        /// </summary>
        /// <param name="DateTime1">开始时间</param>
        /// <param name="DateTime2">结束时间</param>
        /// <returns>返回秒差</returns>
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                dateDiff = ts2.Subtract(ts1).Seconds;
            }
            catch
            { }
            return dateDiff;
        }
        #endregion

        #region 获得并显示文件传输剩余时间
        /// <summary>
        /// 获得并显示文件传输剩余时间
        /// </summary>
        /// <param name="fileLen">文件长度</param>
        /// <param name="currTransmittedLen">当前传输完成的数据长度</param>
        /// <param name="lastTransmittedLen">上次传输完成的数据长度</param>
        /// <returns></returns>
        public static string getResidualTime(int fileLen, int currTransmittedLen, int lastTransmittedLen)
        {
            try
            {
                int speed = (fileLen - currTransmittedLen) / (currTransmittedLen - lastTransmittedLen) + 1;
                string s = "";

                int tempSpeed = speed / 3600;
                if (tempSpeed > 0)
                {
                    s = tempSpeed.ToString() + "小时";
                    speed = speed % 3600;
                }

                tempSpeed = speed / 60;//获得分钟
                if (tempSpeed > 0)
                {
                    s += tempSpeed.ToString() + "分";
                    speed = speed % 60;
                }

                s += speed.ToString() + "秒";

                return s.ToString();
            }
            catch { return lastTransmittedLen.ToString(); }
        }
        #endregion

        #region 将消息写入日志文件 WirteLog(string str)
        /// <summary>
        /// LanMsg写日志函数
        /// </summary>
        /// <param name="str">要写入的日志内容字符串</param>
        public static void WirteLog(string str)
        {
            try
            {
                FileStream fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter _streamWriter = new StreamWriter(fs);
                _streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                _streamWriter.WriteLine(str + " " + DateTime.Now.ToString() + "\n");
                _streamWriter.Flush();
                _streamWriter.Close();
                fs.Close();
            }
            catch
            {
            }
        }
        #endregion

        #region 返回时间字符串
        /// <summary>
        /// 返回时间字符串
        /// </summary>
        /// <param name="dateLength">时间长度（秒为单位）</param>
        /// <returns>返回时间字符串</returns>
        public static string getDateConverToStr(int dateLength)
        {
            string dateStr = "";

            int mod = dateLength;//获得余数

            int year = mod / 31536000;//获得年
            if (year > 0)
            {
                dateStr += year.ToString() + "年";
            }
            mod = mod % 31536000;

            int mon = mod / 2592000;//获得月 
            if (mon > 0)
            {
                dateStr += mon.ToString() + "月";
            }
            mod = mod % 2592000;


            int day = mod / 86400;
            if (day > 0)
            {
                dateStr += day.ToString() + "天";
            }
            mod = mod % 86400;


            int hour = mod / 3600;//获得小时
            if (hour > 0)
            {
                dateStr += hour.ToString() + "小时";
                mod = mod % 3600;
            }
            mod = mod % 3600;


            int mu = mod / 60;//获得分
            if (mu > 0)
            {
                dateStr += mu.ToString() + "分";
            }
            mod = mod % 60;


            int ss = mod / 1;
            if (ss > 0)
                dateStr += ss.ToString() + "秒";

            return dateStr;
        }
        #endregion

     

    }

}
