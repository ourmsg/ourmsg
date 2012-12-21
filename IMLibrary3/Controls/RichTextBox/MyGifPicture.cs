using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace IMLibrary3 
{
	/// <summary> 
	/// MyPicture 的摘要说明。
	/// </summary>
	public class MyPicture : System.Windows.Forms.PictureBox
    {
        #region 组件设计器生成的代码
        /// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// 
        /// </summary>
		public MyPicture()
		{
			// 该调用是 Windows.Forms 窗体设计器所必需的。
			InitializeComponent();
			// TODO: 在 InitializeComponent 调用后添加任何初始化
            BackColor  = Color.Transparent;
		}


		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// 设计器支持所需的方法 - 不要使用代码编辑器 
		/// 修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion


        /// <summary>
        /// 记录当前图片是否需要发送到对方
        /// </summary>
		public bool IsSend=false;//标识此图片是否需要发送到对方,默认不发送

        /// <summary>
        /// 当前图片在richtextBox中的位置
        /// </summary>
        public int Pos = 0;

        /// <summary>
        /// 图片文件的MD5值
        /// </summary>
        public string  MD5 = ""; 

        /// <summary>
        /// 图片是否加载
        /// </summary>
        public bool IsLoad = false;


        /// <summary>
        /// 播放图片动画
        /// </summary>
		public void playGif()
		{
			System.Drawing.ImageAnimator.Animate(this.Image,new System.EventHandler(this.OnFrameChanged));
		}
 
		private void OnFrameChanged(object sender, EventArgs e) 
		{
			this.Invalidate();
		}


	}
}
