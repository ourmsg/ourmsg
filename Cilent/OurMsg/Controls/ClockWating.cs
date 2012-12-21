using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace OurMsg.Components
{
    /* 
    * 作者：Starts_2000
    * 日期：2009-08-09
    * 网站：http://www.csharpwin.com CS 程序员之窗。
    * 你可以免费使用或修改以下代码，但请保留版权信息。
    * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
    */

    /// <summary>
    /// 时钟类等待组件
    /// </summary>
    public class ClockWating : Control
    {
        #region Fields

        private Color _baseColor = Color.FromArgb(0, 89, 179);
        private Color _borderColor = Color.FromArgb(98, 98, 98);
        private Color _tickColor = Color.FromArgb(215, 241, 251);
        private Color _fingerColor = Color.FromArgb(1, 250, 8);
        private Timer _timer;
        private int _tickVlue;
        private int _fingerValue;
        private bool _active;
        private int _rotationSpeed = 15;
        private int _ingerPerAngle = 4;
        private int _tickPerAngle = 3;
        private int _fingerThickness = 30;

        private static readonly int ClockWidth = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ClockWating()
            : base()
        {
            SetStyles();
        
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Color), "0, 89, 179")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "98, 98, 98")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "215, 241, 251")]
        public Color TickColor
        {
            get { return _tickColor; }
            set
            {
                _tickColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "1, 250, 8")]
        public Color FingerColor
        {
            get { return _fingerColor; }
            set
            {
                _fingerColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(15)]
        public int RotationSpeed
        {
            get { return _rotationSpeed; }
            set
            {
                if (value != _rotationSpeed)
                {
                    _rotationSpeed = value <= 10 ? 10 : value;
                    Timer.Interval = _rotationSpeed;
                }
            }
        }

        [DefaultValue(3)]
        public int TickPerAngle
        {
            get { return _tickPerAngle; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("TickPerAngle");
                }
                if (_tickPerAngle != value)
                {
                    _tickPerAngle = value > 15 ? 15 : value;
                }
            }
        }

        [DefaultValue(4)]
        public int FingerPerAngle
        {
            get { return _ingerPerAngle; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("FingerPerAngle");
                }
                if (_ingerPerAngle != value)
                {
                    _ingerPerAngle = value > 36 ? 36 : value;
                }
            }
        }

        [DefaultValue(30)]
        public int FingerThickness
        {
            get { return _fingerThickness; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("FingerThickness");
                }
                if (_fingerThickness != value)
                {
                    _fingerThickness = value > 100 ? 100 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    if (value)
                    {
                        Start();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(75, 75); }
        }

        private Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Timer();
                    _timer.Interval = _rotationSpeed;
                    _timer.Tick += new EventHandler(TimerTick);
                }
                return _timer;
            }
        }


        #endregion

        #region Public Methods

        public void Start()
        {
            if (!_active && !DesignMode)
            {
                _active = true;
                Timer.Start();
            }
        }

        public void Stop()
        {
            if (_active)
            {
                Timer.Stop();
                _tickVlue = 0;
                _fingerValue = 0;
                _active = false;
                base.Invalidate();
            }
        }

        #endregion

        #region Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;
            rect.Width--;
            rect.Height--;

            Rectangle innerRect = rect;
            innerRect.Inflate(-ClockWidth, -ClockWidth);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(innerRect);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    innerRect, 
                    Color.White,
                    _baseColor,
                    70f))
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0f, .2f, .3f, .5f, .7f, .8f, 1f };
                    blend.Factors = new float[] { 0f, .15f, .5f, .8f, .5f, .15f, 0f };

                    brush.Blend = blend;

                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(_borderColor, 5f))
                {
                    g.DrawPath(pen, path);
                }

                using (GraphicsPath outerPath = new GraphicsPath())
                {
                    outerPath.AddEllipse(rect);
                    using (Pen pen = new Pen(_borderColor))
                    {
                        g.DrawPath(pen, outerPath);
                    }

                    outerPath.AddPath(path, false);
                    outerPath.CloseFigure();
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect,
                        SystemColors.ControlDark,
                        Color.White,
                        135f))
                    {
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .1f, .3f, .5f, .7f, .9f, 1f };
                        blend.Factors = new float[] { 0f, .2f, .9f, 1f, .9f, .2f, 0f };

                        brush.Blend = blend;
                        g.FillPath(brush, outerPath);
                    }
                }
            }

            float radius = innerRect.Width / 2.0f;
            DrawTick(g, _tickVlue, radius, radius - ClockWidth);

            DrawFinger(g, innerRect, _fingerValue, radius);
        }

        #endregion

        #region Help Methods

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        private void DrawTick(
            Graphics g, int value, float radiusLarge, float radiusSmall)
        {
            double startAngle = 0;
            PointF pointStart;
            PointF pointEnd;
            PointF centerPoint = new PointF((Width - 1) / 2.0f, (Height - 1) / 2.0f);

            using (Pen pen = new Pen(_tickColor, 2f))
            {
                for (int index = 0; index < 12; index++)
                {
                    startAngle = (30 * index - value * TickPerAngle) / 180.0 * Math.PI;
                    pointStart = new PointF(
                        centerPoint.X + radiusLarge * (float)Math.Cos(startAngle),
                        centerPoint.Y + radiusLarge * (float)Math.Sin(startAngle));
                    pointEnd = new PointF(
                        centerPoint.X + radiusSmall * (float)Math.Cos(startAngle),
                        centerPoint.Y + radiusSmall * (float)Math.Sin(startAngle));
                    g.DrawLine(pen, pointStart, pointEnd);
                }
            }
        }

        private void DrawFinger(
            Graphics g,
            Rectangle rect,
            int value,
            float radius)
        {
            int addAngle = _fingerThickness;

            PointF centerPoint = new PointF((Width - 1) / 2.0f, (Height - 1) / 2.0f);
            double startAngle = ((float)value * FingerPerAngle - addAngle) / 180 * Math.PI;
            double endAngle = ((float)value * FingerPerAngle) / 180 * Math.PI;
            PointF pointStart = new PointF(
                       centerPoint.X + radius * (float)Math.Cos(startAngle),
                       centerPoint.Y + radius * (float)Math.Sin(startAngle));
            PointF pointEnd = new PointF(
                centerPoint.X + radius * (float)Math.Cos(endAngle),
                centerPoint.Y + radius * (float)Math.Sin(endAngle));
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLine(centerPoint, pointStart);
                path.AddArc(rect, value * FingerPerAngle - addAngle, addAngle);
                path.AddLine(pointEnd, centerPoint);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                   path.GetBounds(),
                   Color.FromArgb(20, _fingerColor),
                   _fingerColor,
                   value * FingerPerAngle + 90))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _tickVlue = ++_tickVlue % (30 / TickPerAngle);
            _fingerValue = ++_fingerValue % (360 / FingerPerAngle);
            base.Invalidate();
        }

        #endregion
    }
}
