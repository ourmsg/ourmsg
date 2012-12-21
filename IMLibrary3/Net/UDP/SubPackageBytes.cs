using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Net
{
    #region UDP底层数据包分包类
    /// <summary>
    /// UDP底层数据包分包类
    /// </summary>
    public class SubPackageBytes
    {
        #region 初始化消息类
        /// <summary>
        /// 初始化消息类
        /// </summary>
        /// <param name="Data">字节数组</param>
        public SubPackageBytes(byte[] Data)
        {
            if (Data.Length != BitConverter.ToUInt16(Data, 0))//如果消息长度不等于消息中前两个字节记录的长度则是非法消息
            {
                IsMsg = false;
            }
            else
            { IsMsg = true; }
            this.data = Data;
        }
        #endregion

        #region 初始化消息类
        /// <summary>
        /// 初始化消息类
        /// </summary>
        public SubPackageBytes()
        {

        }

        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="InfoClass">消息协议类型</param>
        /// <param name="MsgContent">消息内容实体</param>
        public SubPackageBytes(byte InfoClass, byte[] MsgContent)
        {
            this.InfoClass = InfoClass;
            this.Content = MsgContent;
        }


        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="InfoClass">消息协议类型</param>
        public SubPackageBytes(byte InfoClass)
        {
            this.InfoClass = InfoClass;

          
        }
        #endregion

        #region 属性

        //private byte[] length=new byte[2];//消息长度 0 
        //private byte[] infoClass = new byte[1];//消息类型 2
        //private byte[] DontFragment = new byte[1];//数据包是否分包 3
        //private byte[] UserIndex=new byte[8];//用户索引 4
        //private byte[] PacketIndex =new byte[8];//数据包索引 12
        //private byte[] FragmentationCount = new byte[4];//片总数 20
        //private byte[] FragmentationIndex = new byte[4];//片索引 24

        private byte[] content = new byte[0];//消息内容实体
        private byte[] data = new byte[28];//消息转换后的字节数组


        #region 获取消息长度
        /// <summary>
        /// 获取消息长度 
        /// </summary>
        public ushort Length
        {
            get
            {
                return BitConverter.ToUInt16(this.data, 0);
            }
        }
        #endregion

        #region 设置或获取消息协议
        /// <summary>
        /// 设置或获取消息协议
        /// </summary>
        public byte InfoClass
        {
            set
            {
                Buffer.SetByte(this.data, 2, value);
            }
            get
            {
                return Buffer.GetByte(this.data, 2);
            }
        }
        #endregion

        #region 用户包编号
        /// <summary>
        /// 用户包编号
        /// </summary>
        public string UserPacketID
        {
            get { return UserIndex.ToString() + PacketIndex.ToString(); }
        }
        #endregion

        #region 一个数据包的关键值
        /// <summary>
        /// 一个数据包的关键值
        /// </summary>
        public string packetKey
        {
            get { return UserPacketID + "-" + FragmentationIndex.ToString(); }
        }
        #endregion

        #region 数据包是否分包
        /// <summary>
        /// 数据包是否分包
        /// </summary>
        public bool DontFragment
        {
            set
            {
                Buffer.SetByte(this.data, 3, Convert.ToByte(value));
            }
            get
            {
                return Convert.ToBoolean(Buffer.GetByte(this.data, 3));
            }
        }
        #endregion

        #region 用户索引
        /// <summary>
        /// 用户索引
        /// </summary>
        public long UserIndex
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, this.data, 4, 8);
            }
            get
            {
                return BitConverter.ToInt64(this.data, 4);
            }
        }
        #endregion

        #region 数据包索引
        /// <summary>
        /// 数据包索引 
        /// </summary>
        public long PacketIndex
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, this.data, 12, 8);
            }
            get
            {
                return BitConverter.ToInt64(this.data, 12);
            }
        }
        #endregion

        #region 片总数
        /// <summary>
        /// 片总数
        /// </summary>
        public int FragmentationCount
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, this.data, 20, 4);
            }
            get
            {
                return BitConverter.ToInt32(this.data, 20);
            }
        }
        #endregion

        #region 片索引
        /// <summary>
        /// 片索引
        /// </summary>
        public int FragmentationIndex
        {
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, this.data, 24, 4);
            }
            get
            {
                return BitConverter.ToInt32(this.data, 24);
            }
        }
        #endregion

        #region 获取或设置消息内容
        /// <summary>
        /// 获取或设置消息内容
        /// </summary>
        public byte[] Content
        {
            set
            {
                this.content = value;
            }
            get
            {
                byte[] buf = new byte[this.Length - 28];
                Buffer.BlockCopy(this.data, 28, buf, 0, buf.Length);
                return buf;
            }
        }
        #endregion

        #region 获取消息是否合法
        /// <summary>
        /// 获取消息是否合法 
        /// </summary>
        public bool IsMsg
        {
            get;
            private set;
        }
        #endregion

        #region 获取消息字节数组
        /// <summary>
        /// 获取消息字节数组 
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set
            {
                if (value.Length != BitConverter.ToUInt16(value, 0))//如果消息长度不等于消息中前两个字节记录的长度则是非法消息
                    IsMsg = false;
                else
                    IsMsg = true;

                data = value;
            }
        }
        #endregion

        #endregion

        #region 方法

        #region 获取消息字节数组
        /// <summary>
        /// 获取消息字节数组
        /// </summary>
        public byte[] getBytes()
        {
            List<byte> result = new List<byte>();
            result.AddRange(this.data);
            result.AddRange(this.content);
            this.data = result.ToArray();
            Buffer.BlockCopy(BitConverter.GetBytes(((ushort)this.data.Length)), 0, this.data, 0, 2);
            return this.data;
        }
        #endregion

        #endregion
    }
    #endregion
}
