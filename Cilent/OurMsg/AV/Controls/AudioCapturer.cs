using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary.AV.Controls
{
    /// <summary>
    ///  音频采集组件
    /// </summary>
    public class AudioCapturer
    {
        #region 变量

        /// <summary>
        /// 声卡音量控制器
        /// </summary>
        private TrackBar trackBarOut=new TrackBar ();
        /// <summary>
        /// 麦克锋音量控制器
        /// </summary>
        private TrackBar trackBarIn=new TrackBar();
        /// <summary>
        /// Mixer类
        /// </summary>
        private Mixer mixerF=new Mixer(new Control());
        /// <summary>
        /// MixerControlDetail类
        /// </summary>
        private Mixer.MixerControlDetail indtl, outdtl;

        /// <summary>
        /// 声音采集器
        /// </summary>
        private LumiSoft.Media.Wave.WaveIn m_pWaveIn = null;

        #endregion

        #region 事件

        /// <summary>
        /// 音频数据捕获事件
        /// </summary>
        /// <param name="sender">对像</param>
        /// <param name="e">音频采集事件参数类</param>
        public delegate void AudioDataCaptureredEventHandler(object sender, AudioCapturedEventArgs e);

        /// <summary>
        /// 音频数据捕获事件
        /// </summary>
        public event AudioDataCaptureredEventHandler AudioDataCapturered;

        #endregion

        #region 初始化
        /// <summary>
        /// 初始化音频采集组件
        /// </summary>
        public AudioCapturer()
        {}

        /// <summary>
        /// 初始化音频采集组件
        /// </summary>
        /// <param name="trackBarOut">声音音量调制器</param>
        /// <param name="trackBarIn">话筒音量调制器</param>
        public AudioCapturer(TrackBar trackBarOut, TrackBar trackBarIn)
        {
            this.trackBarIn = trackBarIn;//初始化麦克风控制控件
            this.trackBarOut = trackBarOut;//初始化声卡声音控制控件
            InitializeMixing() ;
        }
        #endregion

        #region 初始化声卡麦克风控制器以及声音采集器
        /// <summary>
        /// 初始化声卡麦克风
        /// </summary>
        private void InitializeMixing()
        {
            try
            {
                this.trackBarIn.Scroll += new EventHandler(trackBarOut_Scroll);
                this.trackBarOut.Scroll += new EventHandler(trackBarOut_Scroll);

                mixerF.MixerControlChange += new EventHandler(mixer_MixerControlChange);

                this.outdtl = new Mixer.MixerControlDetail(mixerF, Mixer.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS);
                this.trackBarOut.Minimum = this.outdtl.Min;
                this.trackBarOut.Maximum = this.outdtl.Max;
                this.trackBarOut.Value = this.outdtl.Volume;

                this.indtl = new Mixer.MixerControlDetail(mixerF, Mixer.MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE);
                this.trackBarIn.Minimum = indtl.Min;
                this.trackBarIn.Maximum = indtl.Max;
                this.trackBarIn.Value = this.indtl.Volume;

                m_pWaveIn=new LumiSoft.Media.Wave.WaveIn(LumiSoft.Media.Wave.WaveIn.Devices[0],8000,16,1,400);
                m_pWaveIn.BufferFull += new  LumiSoft.Media.Wave.BufferFullHandler(m_pWaveIn_BufferFull);
                m_pWaveIn.Start();
            }
            catch { }
        }
        #endregion

        #region 关闭并释放资源
        /// <summary>
        /// 关闭并释放资源
        /// </summary>
        public void Close()
        {
            if (m_pWaveIn != null)
            {
                m_pWaveIn.Stop();
                m_pWaveIn.Dispose();
            }
            trackBarIn.Dispose(); trackBarIn=  null;
            trackBarOut.Dispose(); trackBarOut = null;
            mixerF.Close(); mixerF = null;
            if (outdtl != null) outdtl = null;
            if (indtl != null) indtl = null;
        }
        #endregion

        #region 音量控制器调节事件
        private void mixer_MixerControlChange(object sender, EventArgs e)
        {
            if (trackBarIn != null)
                trackBarIn.Value = this.indtl.Volume;
            if (trackBarOut != null)
                trackBarOut.Value = this.outdtl.Volume;
        }

        private void trackBarOut_Scroll(object sender, System.EventArgs e)
        {
            if (outdtl != null && trackBarOut!=null )
                outdtl.Volume = trackBarOut.Value;
            if (indtl != null && trackBarIn != null)
                indtl.Volume = trackBarIn.Value;
        }
        #endregion

        #region method m_pWaveIn_BufferFull
        /// <summary>
        /// This method is called when recording buffer is full and we need to process it.
        /// </summary>
        /// <param name="buffer">Recorded data.</param>
        private void m_pWaveIn_BufferFull(byte[] buffer)
        {
            if (AudioDataCapturered != null)
                this.AudioDataCapturered(this,new AudioCapturedEventArgs(buffer));
        }
        #endregion
    }

    #region 音频捕获事件参数
    /// <summary>
    ///  音频捕获事件参数
    /// </summary>
    public class AudioCapturedEventArgs : System.EventArgs
    {

        /// <summary>
        /// 捕获到的音频数据
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 初始化事件参数
        /// </summary>
        public AudioCapturedEventArgs()
        {
        }

        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="data">捕获的音频数据</param>
        public AudioCapturedEventArgs(byte[] data)
        {
            this.Data = data;
        }

        
    }
    #endregion
}
