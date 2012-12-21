using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    #region UDP传输类型
    /// <summary>
    ///  UDP传输类型
    /// </summary>
    public enum TransmitType
    {
        /// <summary>
        /// 获得远程主机信息
        /// </summary>
        getRemoteEP =126,
        /// <summary>
        /// 传输文件数据包
        /// </summary>
        getFilePackage=235,
        /// <summary>
        /// 音频包
        /// </summary>
        Audio=237,
        /// <summary>
        /// 视频包
        /// </summary>
        Video=239,
        /// <summary>
        /// 打洞数据包
        /// </summary>
        Penetrate=114,
        /// <summary>
        /// 传输完成
        /// </summary>
        over=118,
    }
    #endregion

    /// <summary>
    /// UDP Packet传输协议
    /// </summary>
    public class UDPPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public UDPPacket()
        {

        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="data"></param>
        public UDPPacket(byte[] data)
        {
            this.BaseData = data; 
        }

        /// 协议格式：  协议类型  +  IP  + Port + lastpost + Block (BaseData)
        /// 数据长度：    1字节     8字节  4字节    8字节     动态
        //  
        
        /// <summary>
        /// 消息包头
        /// </summary>
        public  byte[] BaseData=new byte[21];

        /// <summary>
        /// 数据块
        /// </summary>
        private byte[] Data = new byte[0];

        #region 协议类型
        /// <summary>
        /// 协议类型
        /// </summary>
        public byte  type
        {
            set
            {
                Buffer.SetByte(BaseData, 0, value);
            }
            get
            {
                return Buffer.GetByte(BaseData, 0);
            }
        }
        #endregion

        #region 远程主机
        /// <summary>
        /// 远程主机
        /// </summary>
        public System.Net.IPAddress  RemoteIP
        {
            set
            {
               byte[] ip= value.GetAddressBytes();
               Buffer.BlockCopy(ip, 0, BaseData, 1, ip.Length);
            }
            get
            {
                byte[] ip = new byte[4];
                Buffer.BlockCopy(BaseData, 1,ip, 0,ip.Length);
                try
                {
                    System.Net.IPAddress IP = new System.Net.IPAddress(ip);
                    return IP;
                }
                catch { return null; }
            }
        }
        #endregion

        #region 设置或获取端口号
        /// <summary>
        /// 设置或获取端口号
        /// </summary>
        public int  Port
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, BaseData, 9, 4);
            }
            get
            {
                return BitConverter.ToInt32(BaseData, 9);
            }
        }
        #endregion

        #region 设置或获取最后一次数据发送位置
        /// <summary>
        /// 设置或获取最后一次数据发送位置
        /// </summary>
        public long LastPos
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, BaseData, 13,8);
            }
            get
            {
                return BitConverter.ToInt64(BaseData,13);
            }
        }
        #endregion

        #region 获取数据块长度
        /// <summary>
        /// 获取数据块长度
        /// </summary>
        private int BlockLength
        {
            get 
            {
                return BaseData.Length - 21;
            }
        }
        #endregion

        #region 获取或设置数据块内容
        /// <summary>
        /// 获取或设置数据块内容
        /// </summary>
        public byte[] Block
        { 
            set
            {
                Data = value;
            }
            get
            {
                Data = new byte[this.BlockLength];
                Buffer.BlockCopy(BaseData,21, this.Data , 0,BlockLength);
                return Data;
            }
        }
        #endregion

        #region 获取消息字节数组
        /// <summary>
        /// 获得消息字节数组
        /// </summary>
        public byte[] ToBytes()
        {
            if (Data.Length == 0)
                return BaseData;

            #region 算法1
            List<byte> result = new List<byte>();
            result.AddRange(BaseData);
            result.AddRange(Data);
            byte[] d = result.ToArray();
            return d;
            #endregion

            #region 算法2
            //byte[] bs = new byte[BaseData.Length + Data.Length];

            //Array.Copy(BaseData, 0, bs, 0, BaseData.Length);
            //Array.Copy(Data, 0, bs, BaseData.Length, Data.Length);
            //return bs;
            #endregion
        }
        #endregion

    }
}
