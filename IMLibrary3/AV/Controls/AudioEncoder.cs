using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.AV 
{
    /// <summary>
    /// 音频编解码器
    /// </summary>
    public class AudioEncoder
    {
        /// <summary>
        /// LumiSoft音频编解码器
        /// </summary>
        private LumiSoft.Net.Media.Codec.Audio.AudioCodec   m_pActiveCodec = null;
        private G729 g729=null;

       /// <summary>
       /// 初始化音频编解码器
       /// </summary>
        public AudioEncoder( )
        {
            m_pActiveCodec = new LumiSoft.Net.Media.Codec.Audio.G711_alaw();
            //g729 = new G729();
            //g729.InitalizeEncode();
            //g729.InitalizeDecode();
        }

        /// <summary>
        /// 编码(压缩)
        /// </summary>
        /// <param name="data">要压缩的数据</param>
        public byte[] Encode(byte[] data)
        {
            return m_pActiveCodec.Encode(data, 0, data.Length);
            //return g729.Encode(data);
        }

        /// <summary>
        /// 解码(解压缩)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decode(byte[] data)
        {
            return m_pActiveCodec.Decode(data, 0, data.Length);
            //return g729.Decode(data);
        }

    }

}
