using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace IMLibrary3
{ 
    public class GifBox : Control
    {
        #region 变量

        private Image _image;
        private Rectangle _imageRectangle;
        private EventHandler _eventAnimator;
        private bool _canAnimate;
        private Color _borderColor = Color.Transparent;

        #endregion

        #region 构造函数

        public GifBox()
            : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText |
                ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.Opaque, false);
        }

        #endregion

        #region 属性

        public Image Image
        {
            get { return _image; }
            set
            {
                StopAnimate();
                _image = value;
                _imageRectangle = Rectangle.Empty;
                if (value != null)
                    _canAnimate = ImageAnimator.CanAnimate(_image);
                else
                    _canAnimate = false;
                Size = Image.Size;
                Invalidate(ImageRectangle);
                if (!DesignMode)
                    StartAnimate();
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        private Rectangle ImageRectangle
        {
            get
            {
                if (_imageRectangle == Rectangle.Empty &&
                    _image != null)
                {
                    _imageRectangle.X = (Width - _image.Width) / 2;
                    _imageRectangle.Y = (Height - _image.Height) / 2;
                    _imageRectangle.Width = _image.Width;
                    _imageRectangle.Height = _image.Height;
                }
                return _imageRectangle;
            }
        }

        private bool CanAnimate
        {
            get { return _canAnimate; }
        }

        private EventHandler EventAnimator
        {
            get
            {
                if (_eventAnimator == null)
                    _eventAnimator = delegate(object sender, EventArgs e)
                    {
                        Invalidate(ImageRectangle);
                    };
                return _eventAnimator;
            }
        }

        #endregion

        #region Override

        protected override void OnSizeChanged(EventArgs e)
        {
            _imageRectangle = Rectangle.Empty;
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (_image != null)
            {
                //每次画之前更新到图片的下一帧。
                UpdateImage();
                e.Graphics.DrawImage(
                    _image,
                    ImageRectangle,
                    0,
                    0,
                    _image.Width,
                    _image.Height,
                    GraphicsUnit.Pixel);
            }

            ControlPaint.DrawBorder(
                    e.Graphics,
                    ClientRectangle,
                    _borderColor,
                    ButtonBorderStyle.Solid);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _eventAnimator = null;
                _canAnimate = false;
                if (_image != null)
                    _image = null;
            }

        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            StopAnimate();
        }

        #endregion

        #region Private Method

        private void StartAnimate()
        {
            if (CanAnimate)
            {
                ImageAnimator.Animate(_image, EventAnimator);
            }
        }

        private void StopAnimate()
        {
            if (CanAnimate)
            {
                ImageAnimator.StopAnimate(_image, EventAnimator);
            }
        }

        private void UpdateImage()
        {
            if (CanAnimate)
            {
                ImageAnimator.UpdateFrames(_image);
            }
        }

        #endregion
    }
}
