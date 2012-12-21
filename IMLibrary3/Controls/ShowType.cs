using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace IMLibrary3.Controls
{
    public partial class ShowType : UserControl
    {
        #region 初始化
        public ShowType()
        {
            InitializeComponent();
            this.tMenuItemStateOnline.Tag = IMLibrary3.Enmu.ShowType.NONE ;
            this.tMenuItemStateAway.Tag = IMLibrary3.Enmu.ShowType.away;
            this.tMenuItemStateBusy.Tag = IMLibrary3.Enmu.ShowType.xa ;
            this.tMenuItemStateBlocked.Tag = IMLibrary3.Enmu.ShowType.dnd ;
            this.tMenuItemStateHiding.Tag = IMLibrary3.Enmu.ShowType.Invisible ;
            this.tMenuItemStateOffline.Tag = IMLibrary3.Enmu.ShowType.Offline;

            this.tMenuItemStateOnline.Click += new System.EventHandler(this.tMenuItemShowType_Click);
            this.tMenuItemStateAway.Click += new System.EventHandler(this.tMenuItemShowType_Click);
            this.tMenuItemStateBusy.Click += new System.EventHandler(this.tMenuItemShowType_Click);
            this.tMenuItemStateBlocked.Click += new System.EventHandler(this.tMenuItemShowType_Click);
            this.tMenuItemStateHiding.Click += new System.EventHandler(this.tMenuItemShowType_Click);
            this.tMenuItemStateOffline.Click += new EventHandler(tMenuItemShowType_Click);
        }
        #endregion

        #region 事件

        #region 在线状态更改事件参数类
        /// <summary>
        /// 在线状态更改事件参数类
        /// </summary>
        public class ShowTypeEventArgs : System.EventArgs
        {
            public ShowTypeEventArgs(IMLibrary3.Enmu.ShowType  ShowType,string ShowTypeString )
            {
                this.ShowType = ShowType;
                this.ShowTypeString = ShowTypeString;
            }

            /// <summary>
            /// 在线状态描述
            /// </summary>
            public string ShowTypeString = "我在线上";
            /// <summary>
            /// 在线状态
            /// </summary>
            public IMLibrary3.Enmu.ShowType ShowType = IMLibrary3.Enmu.ShowType.NONE;
        }
        #endregion

        public delegate void ShowTypeChangedHandler(object sender, ShowTypeEventArgs e);

        /// <summary>
        /// 状态改变
        /// </summary>
        public event ShowTypeChangedHandler ShowTypeChanged;

        /// <summary>
        /// 退出程序
        /// </summary>
        public event ShowTypeChangedHandler ShowTypeExitApp;

        /// <summary>
        /// 关于
        /// </summary>
        public event ShowTypeChangedHandler ShowTypeAbout;

        #endregion 
               
        #region 属性

        /// <summary>
        /// 是否显示关于信息
        /// </summary>
        public bool  IsShowAbout=true ;

        private bool _IsShowOffline = false;
        /// <summary>
        /// 是否显示离线菜单
        /// </summary>
        public bool IsShowOffline
        {
            set
            {
                _IsShowOffline = value;
                tMenuItemStateOffline.Visible = value;
                tMenuItemStateOfflineSp.Visible = value;
            }
            get
            {
                return _IsShowOffline;
            }
        }

        private IMLibrary3.Enmu.ShowType OldState = IMLibrary3.Enmu.ShowType.NONE;


        private IMLibrary3.Enmu.ShowType _State = IMLibrary3.Enmu.ShowType.NONE;
        /// <summary>
        /// 设置或获取在线状态(默认为在线)
        /// </summary>
        public IMLibrary3.Enmu.ShowType State
        {
            get { return _State; }
            set {
                 _State = value;
                ToolStripItem sender=null;
                switch (value)
                {
                    case IMLibrary3.Enmu.ShowType.away:
                        sender=tMenuItemStateAway;
                        break;
                    case IMLibrary3.Enmu.ShowType.dnd:
                        sender=tMenuItemStateBlocked;
                        break;
                    case IMLibrary3.Enmu.ShowType.xa :
                        sender=tMenuItemStateBusy;
                        break;
                    case IMLibrary3.Enmu.ShowType.Invisible:
                        sender=tMenuItemStateHiding;
                        break;
                    case IMLibrary3.Enmu.ShowType.NONE:
                        sender=tMenuItemStateOnline;
                        break;
                    case IMLibrary3.Enmu.ShowType.Offline:
                        OldState = State;
                        sender=tMenuItemStateOffline;
                        break;
                }

                tButShowType.Image = sender.Image;
                tButShowType.ToolTipText = sender.Text;
                labShowType.Text = sender.Text;
                
             }
        }
        #endregion

        #region 状态菜单单击事件
        private void tMenuItemShowType_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsMenuItem = (sender as ToolStripMenuItem);
            if (tsMenuItem == null) return;
            if (this.State == IMLibrary3.Enmu.ShowType.Offline)
            {
                tButShowType.Image = tMenuItemStateOffline.Image;
                labShowType.Text = tMenuItemStateOffline.Text;
            }
            else if (OldState == IMLibrary3.Enmu.ShowType.Offline && //如果原值为离线,并且新值不为在线
                (IMLibrary3.Enmu.ShowType)tsMenuItem.Tag != IMLibrary3.Enmu.ShowType.Offline) 
            {
                tButShowType.Image = tMenuItemStateOffline.Image;
                labShowType.Text = tMenuItemStateOffline.Text;
            }
            else if (OldState != (IMLibrary3.Enmu.ShowType)tsMenuItem.Tag) //如果原值为在线，现值为离线
            {
                tButShowType.Image = tsMenuItem.Image;
                this.labShowType.Text = tsMenuItem.Text;
            }
            OldState = (IMLibrary3.Enmu.ShowType)tsMenuItem.Tag;

            tButShowType.ToolTipText = tsMenuItem.Text;

            if (ShowTypeChanged != null)
                ShowTypeChanged(this, new ShowTypeEventArgs((IMLibrary3.Enmu.ShowType)tsMenuItem.Tag, tsMenuItem.Text));
        }
        #endregion

        #region 关于与退出程序菜单事件
        private void tMenuItemAbout_Click(object sender, EventArgs e)
        {
            if (IsShowAbout)
                IMLibrary3.Global.MsgShow("版权所有：租李叶(QQ:25348855)\n  Copyright © 2003 - 2012 ");

            if(this.ShowTypeAbout!=null )
                ShowTypeAbout(this,new ShowTypeEventArgs(this.State,this.State.ToString()));
        }

        private void tMenuItemExitApp_Click(object sender, EventArgs e)
        {
             if(this.ShowTypeExitApp!=null )
                ShowTypeExitApp(this,new ShowTypeEventArgs(this.State,this.State.ToString()));
        }
        #endregion

        private void labShowType_DoubleClick(object sender, EventArgs e)
        {
            this.textBox1.Text = labShowType.Text;
            this.textBox1.Visible = true;
            
        }
 

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.textBox1.Visible = false ;

        }
    }
}
