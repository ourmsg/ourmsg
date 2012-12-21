using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol
{
    #region 对话消息
    /// <summary>
    /// 对话消息 
    /// </summary>
    public class Message : Element
    {
        /// <summary>
        /// 
        /// </summary>
        public Message()
        {
            this.DateTime = System.DateTime.Now.ToString();
        }


        /// <summary>
        /// 消息类型
        /// </summary>
        public IMLibrary3.Enmu.MessageType MessageType
        {
            get;
            set;
        }

        /// <summary>
        /// 消息发送时间
        /// </summary>
        public string DateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 消息字体名称
        /// </summary>
        public string FontName
        {
            set;
            get;
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize
        {
            set;
            get;
        }

        /// <summary>
        /// 字体是否加粗
        /// </summary>
        public bool FontBold
        {
            set;
            get;
        }

        /// <summary>
        /// 字体是否斜体字
        /// </summary>
        public bool FontItalic
        {
            set;
            get;
        }

        /// <summary>
        /// 字体是否删除线
        /// </summary>
        public bool FontStrikeout
        {
            set;
            get;
        }

        /// <summary>
        /// 字体是否下划线
        /// </summary>
        public bool FontUnderline
        {
            set;
            get;
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public int FontColor
        {
            set;
            get;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content
        {
            set;
            get;
        }

        /// <summary>
        /// 图片信息
        /// </summary>
        public string ImageInfo
        {
            set;
            get;
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark
        {
            set;
            get;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title
        {
            set;
            get;
        }

        #region 设置或获取消息字体
        /// <summary>
        /// 设置或获取消息字体颜色
        /// </summary>
        public System.Drawing.Color Color
        {
            set { this.FontColor = value.ToArgb(); }
            get
            {
                if (this.FontColor == 0) this.FontColor = 16777216;
                return System.Drawing.Color.FromArgb(this.FontColor);
            }
        }
        #endregion

        #region 设置或获取消息字体
        /// <summary>
        /// 设置或获取消息字体 
        /// </summary>
        public System.Drawing.Font Font
        {
            set
            {
                ////获得消息字体各项值开始
                //if (this.FontSize <= 0) this.FontSize = 9f;
                this.FontName = value.Name;//获得字体名称
                this.FontSize = value.Size;//字体大小
                this.FontBold = value.Bold;//是否粗体
                this.FontItalic = value.Italic;//是否斜体
                this.FontStrikeout = value.Strikeout;//是否删除线
                this.FontUnderline = value.Underline;//是否下划线
                /////获得消息字体各项值结束
            }
            get
            {
                if (this.FontSize <= 0) this.FontSize = 9f;
                System.Drawing.FontStyle fontStyle = new System.Drawing.FontStyle();
                if (this.FontBold) fontStyle = System.Drawing.FontStyle.Bold;
                if (this.FontItalic) fontStyle = fontStyle | System.Drawing.FontStyle.Italic;
                if (this.FontStrikeout) fontStyle = fontStyle | System.Drawing.FontStyle.Strikeout;
                if (this.FontUnderline) fontStyle = fontStyle | System.Drawing.FontStyle.Underline;
                System.Drawing.Font ft = new System.Drawing.Font(this.FontName, this.FontSize, fontStyle);
                return ft;
            }
        }
        #endregion

    }
    #endregion 
}
