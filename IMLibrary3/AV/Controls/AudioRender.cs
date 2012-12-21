using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.AV 
{
    /// <summary>
    /// 声音回放组件
    /// </summary>
    public  class AudioRender
    {
        #region 变量
        /// <summary>
        /// 声音输出器
        /// </summary>
        private LumiSoft.Media.Wave.WaveOut m_pWaveOut = null;

        /// <summary>
        /// 初始化声音回放组件
        /// </summary>
        public AudioRender()
        {
            m_pWaveOut = new LumiSoft.Media.Wave.WaveOut(LumiSoft.Media.Wave.WaveOut.Devices[0], 8000, 16, 1);
        }

        /// <summary>
        /// 播放声音 
        /// </summary>
        /// <param name="data">声音数据</param>
        public void play(byte [] data)
        {
            m_pWaveOut.Play(data, 0, data.Length);
        }

        #endregion

    }
}
