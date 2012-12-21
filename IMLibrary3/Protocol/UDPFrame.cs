using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// udp侦数据包
    /// </summary>
    public class UDPFramePacket
    {

        /// <summary>
        /// 
        /// </summary>
        public UDPFramePacket()
        {

        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="data"></param>
        public UDPFramePacket(byte[] data)
        {
            this.BaseData = data; 
        }

        /// 协议格式：  协议类型(type) +时间戳(Timestamp) +包总数（PacketCount）+包索引（PacketIndex） +  IP  + Port + 包内容(Block=BaseData)
        /// 数据长度：     1字节       +      8字节       +        4字节        +       4字节          + 8字节+ 4字节+       动态
        //


        /// <summary>
        /// 消息包头
        /// </summary>
        public byte[] BaseData = new byte[29];

        /// <summary>
        /// 数据块
        /// </summary>
        private byte[] Data = new byte[0];

        #region 协议类型
        /// <summary>
        /// 协议类型
        /// </summary>
        public byte type
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

        #region 设置或获取时间戳
        /// <summary>
        /// 设置或获取时间戳
        /// </summary>
        public long Timestamp
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, BaseData, 1, 8);
            }
            get
            {
                return BitConverter.ToInt64(BaseData, 1);
            }
        }
        #endregion

        #region 设置或获取PacketCount
        /// <summary>
        /// 设置或获取PacketCount
        /// </summary>
        public int PacketCount
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

        #region 设置或获取PacketIndex
        /// <summary>
        /// 设置或获取PacketIndex
        /// </summary>
        public int PacketIndex
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, BaseData, 13, 4);
            }
            get
            {
                return BitConverter.ToInt32(BaseData, 13);
            }
        }
        #endregion

        #region 远程主机
        /// <summary>
        /// 远程主机
        /// </summary>
        public System.Net.IPAddress RemoteIP
        {
            set
            {
                byte[] ip = value.GetAddressBytes();
                Buffer.BlockCopy(ip, 0, BaseData,17, ip.Length);
            }
            get
            {
                byte[] ip = new byte[4];
                Buffer.BlockCopy(BaseData, 17, ip, 0, ip.Length);
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
        public int Port
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, BaseData, 25, 4);
            }
            get
            {
                return BitConverter.ToInt32(BaseData, 25);
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
                return BaseData.Length - 29;
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
                Buffer.BlockCopy(BaseData, 29, this.Data, 0, BlockLength);
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

            List<byte> result = new List<byte>();
            result.AddRange(BaseData);
            result.AddRange(Data);
            byte[] d = result.ToArray();
            return d;
        }
        #endregion
    }
}
