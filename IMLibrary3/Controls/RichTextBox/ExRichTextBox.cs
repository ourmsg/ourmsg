using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace IMLibrary3
{ 
    public class MyExtRichTextBox : RichTextBox
    {
        private RichEditOle richEditOle;
        public Dictionary<string, MyPicture>  Pictures = new Dictionary<string, MyPicture>();

        public MyExtRichTextBox()
            : base()
        {
        }

        #region 属性
        private RichEditOle RichEditOle
        {
            get
            {
                if ( richEditOle == null)
                {
                    if (base.IsHandleCreated)
                    {
                         richEditOle = new RichEditOle(this);
                    }
                }

                return  richEditOle;
            }
        }
        #endregion 

        #region RichTextBoxPlus Members
        protected IRichEditOle IRichEditOleValue = null;
        protected IntPtr IRichEditOlePtr = IntPtr.Zero;
        public IRichEditOle GetRichEditOleInterface()
        {
            if (this.IRichEditOleValue == null)
            {
                // Allocate the ptr that EM_GETOLEINTERFACE will fill in.
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));	// Alloc the ptr.
                Marshal.WriteIntPtr(ptr, IntPtr.Zero);	// Clear it.
                try
                {
                    if (0 != API.SendMessage(this.Handle, RichEditOle.EM_GETOLEINTERFACE, IntPtr.Zero, ptr))
                    {
                        // Read the returned pointer.
                        IntPtr pRichEdit = Marshal.ReadIntPtr(ptr);
                        try
                        {
                            if (pRichEdit != IntPtr.Zero)
                            {
                                // Query for the IRichEditOle interface.
                                Guid guid = new Guid("00020D00-0000-0000-c000-000000000046");
                                Marshal.QueryInterface(pRichEdit, ref guid, out this.IRichEditOlePtr);

                                // Wrap it in the C# interface for IRichEditOle.
                                this.IRichEditOleValue = (IRichEditOle)Marshal.GetTypedObjectForIUnknown(this.IRichEditOlePtr, typeof(IRichEditOle));
                                if (this.IRichEditOleValue == null)
                                {
                                    throw new Exception("Failed to get the object wrapper for the interface.");
                                }
                            }
                            else
                            {
                                throw new Exception("Failed to get the pointer.");
                            }
                        }
                        finally
                        {
                            Marshal.Release(pRichEdit);
                        }
                    }
                    else
                    {
                        throw new Exception("EM_GETOLEINTERFACE failed.");
                    }
                }
                catch
                {
                    this.ReleaseRichEditOleInterface();
                }
                finally
                {
                    // Free the ptr memory.
                    Marshal.FreeCoTaskMem(ptr);
                    //Marshal.DestroyStructure(ptr, typeof(REOBJECT));
                }
            }
            return this.IRichEditOleValue;
        }

        /// <summary>
        /// Releases the IRichEditOle interface if it hasn't been already.
        /// </summary>
        /// <remarks>This is automatically called in Dispose if needed.</remarks>
        public void ReleaseRichEditOleInterface()
        {
            if (this.IRichEditOlePtr != IntPtr.Zero)
            {
                Marshal.Release(this.IRichEditOlePtr);
            }

            this.IRichEditOlePtr = IntPtr.Zero;
            this.IRichEditOleValue = null;
        }

        #endregion
      
        #region 扩展
        private System.Windows.Forms.Panel panelGif = new Panel();
         
        #region 添加GIF图片控件到richtextbox
        /// <summary>
        /// 添加GIF图片控件到richtextbox
        /// </summary>
        /// <param name="MD5">gifID</param>
        /// <param name="image"></param>
        /// <returns>返回gif控件</returns>
        public IMLibrary3.MyPicture addGifControl(string MD5, Image image)
        {
            MyPicture pic = new MyPicture();
            
            if (Pictures.ContainsKey(MD5))
            {
                MyPicture exPic = null;
                Pictures.TryGetValue(MD5, out exPic);
                pic.Image = exPic.Image;
            }
            else
            {
                pic.Image = image;
                Pictures.Add(MD5, pic);
            }
            panelGif.Controls.Add(pic);
            pic.MD5 = MD5; 
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize; 
            pic.BackColor = this.BackColor; 
            pic.Invalidate(); 
             
            RichEditOle.InsertControl(pic);
            Invalidate();
            return pic;
        }
        #endregion

        #region 获得richtextBox中现有的图片集合
        /// <summary>
        /// 获得richtextBox中现有的图片集合
        /// </summary>
        /// <returns></returns>
        public List<MyPicture> GetExistGifs()
        {
            List<MyPicture> tempGifs = new List<MyPicture>();
            REOBJECT reObject = new REOBJECT();
            for (int i = 0; i < this.GetRichEditOleInterface().GetObjectCount(); i++) 
            {
                this.GetRichEditOleInterface().GetObject(i, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                object t = Marshal.GetObjectForIUnknown(reObject.poleobj);
                MyPicture pic = t as MyPicture;
                if (pic != null)
                {
                    pic.Pos = reObject.cp; 
                    pic.IsSend = true;//标识发送当前图片
                    tempGifs.Add(pic); 
                } 
            }
            return tempGifs;
        }
        #endregion

        #region 获得richtextBox中现有的图片集合文本信息
        /// <summary>
        /// 获得richtextBox中现有的图片集合
        /// </summary>
        /// <returns></returns>
        public string GetImageInfo()
        {
            string imageInfo = "";
            //try
            {
                REOBJECT reObject =new REOBJECT() ;
                for (int i = 0; i <  GetRichEditOleInterface().GetObjectCount(); i++) 
                {
                    this.GetRichEditOleInterface().GetObject(i, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                    object t = Marshal.GetObjectForIUnknown(reObject.poleobj);
                    MyPicture pic = t as MyPicture;
                    if (pic != null)
                        imageInfo += reObject.cp.ToString() + "," + pic.MD5 + "|";
                }
            }
            //catch { }
            return imageInfo;
        }
        #endregion

        #region 获得图片控件
        /// <summary>
        /// 获得图片控件
        /// </summary>
        /// <param name="MD5">图片MD5值</param>
        /// <returns></returns>
        public MyPicture GetPicture(string MD5)
        {
            MyPicture exPic = null;
            if (Pictures.ContainsKey(MD5))
                Pictures.TryGetValue(MD5, out exPic);
            return exPic;
        }
        #endregion

        #endregion
         
    }

    #region RichEditOle
    internal class RichEditOle
    {
        private MyExtRichTextBox _richEdit;
        private IRichEditOle _richEditOle;
        public const int WM_USER = 0x0400;
        public const int EM_GETOLEINTERFACE = WM_USER + 60;

        public RichEditOle(MyExtRichTextBox richEdit)
        {
            _richEdit = richEdit;
        }

        public IRichEditOle IRichEditOle
        {
            get
            {
                if (_richEditOle == null)
                {
                    _richEditOle = NativeMethods.SendMessage(
                        _richEdit.Handle, NativeMethods.EM_GETOLEINTERFACE, 0);
                }
                return _richEditOle;
            }
        }

        public void InsertControl(Control control)
        {
            if (control == null) return;


            ILockBytes bytes;
            IStorage storage;
            IOleClientSite site;
            Guid guid = Marshal.GenerateGuidForType(control.GetType());
            NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out bytes);
            NativeMethods.StgCreateDocfileOnILockBytes(bytes, 0x1012, 0, out storage);
            IRichEditOle.GetClientSite(out site);
            REOBJECT lpreobject = new REOBJECT();
            lpreobject.cp = _richEdit.SelectionStart;
            lpreobject.clsid = guid;
            lpreobject.pstg = storage;
            lpreobject.poleobj = Marshal.GetIUnknownForObject(control);
            lpreobject.polesite = site;
            lpreobject.dvAspect = 1;
            lpreobject.dwFlags = 2;
            lpreobject.dwUser = 1;
            IRichEditOle.InsertObject(lpreobject);
            Marshal.ReleaseComObject(bytes);
            Marshal.ReleaseComObject(site);
            Marshal.ReleaseComObject(storage);

        }

        public bool InsertImageFromFile(string strFilename)
        {
            ILockBytes bytes;
            IStorage storage;
            IOleClientSite site;
            object obj2;
            NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out bytes);
            NativeMethods.StgCreateDocfileOnILockBytes(bytes, 0x1012, 0, out storage);
            IRichEditOle.GetClientSite(out site);
            FORMATETC pFormatEtc = new FORMATETC();
            pFormatEtc.cfFormat = (CLIPFORMAT)0;
            pFormatEtc.ptd = IntPtr.Zero;
            pFormatEtc.dwAspect = DVASPECT.DVASPECT_CONTENT;
            pFormatEtc.lindex = -1;
            pFormatEtc.tymed = TYMED.TYMED_NULL;
            Guid riid = new Guid("{00000112-0000-0000-C000-000000000046}");
            Guid rclsid = new Guid("{00000000-0000-0000-0000-000000000000}");
            NativeMethods.OleCreateFromFile(ref rclsid, strFilename, ref riid, 1, ref pFormatEtc, site, storage, out obj2);
            if (obj2 == null)
            {
                Marshal.ReleaseComObject(bytes);
                Marshal.ReleaseComObject(site);
                Marshal.ReleaseComObject(storage);
                return false;
            }
            IOleObject pUnk = (IOleObject)obj2;
            Guid pClsid = new Guid();
            pUnk.GetUserClassID(ref pClsid);
            NativeMethods.OleSetContainedObject(pUnk, true);
            REOBJECT lpreobject = new REOBJECT();
            lpreobject.cp = _richEdit.TextLength;
            lpreobject.clsid = pClsid;
            lpreobject.pstg = storage;
            lpreobject.poleobj = Marshal.GetIUnknownForObject(pUnk);
            lpreobject.polesite = site;
            lpreobject.dvAspect = 1;
            lpreobject.dwFlags = 2;
            lpreobject.dwUser = 0;
            IRichEditOle.InsertObject(lpreobject);
            Marshal.ReleaseComObject(bytes);
            Marshal.ReleaseComObject(site);
            Marshal.ReleaseComObject(storage);
            Marshal.ReleaseComObject(pUnk);
            return true;
        }

        public REOBJECT InsertOleObject(
            IOleObject oleObject,
            int index)
        {
            if (oleObject == null)
            {
                return null;
            }

            ILockBytes pLockBytes;
            NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

            IStorage pStorage;
            NativeMethods.StgCreateDocfileOnILockBytes(
                pLockBytes,
                (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE),
                0,
                out pStorage);

            IOleClientSite pOleClientSite;
            IRichEditOle.GetClientSite(out pOleClientSite);

            Guid guid = new Guid();

            oleObject.GetUserClassID(ref guid);
            NativeMethods.OleSetContainedObject(oleObject, true);

            REOBJECT reoObject = new REOBJECT();

            reoObject.cp = _richEdit.TextLength;
            reoObject.clsid = guid;
            reoObject.pstg = pStorage;
            reoObject.poleobj = Marshal.GetIUnknownForObject(oleObject);
            reoObject.polesite = pOleClientSite;
            reoObject.dvAspect = (uint)DVASPECT.DVASPECT_CONTENT;
            reoObject.dwFlags = (uint)REOOBJECTFLAGS.REO_BELOWBASELINE;
            reoObject.dwUser = (uint)index;

            IRichEditOle.InsertObject(reoObject);

            Marshal.ReleaseComObject(pLockBytes);
            Marshal.ReleaseComObject(pOleClientSite);
            Marshal.ReleaseComObject(pStorage);

            return reoObject;
        }

        public void UpdateObjects()
        {
            int objectCount = this.IRichEditOle.GetObjectCount();
            for (int i = 0; i < objectCount; i++)
            {
                REOBJECT lpreobject = new REOBJECT();
                IRichEditOle.GetObject(i, lpreobject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                Point positionFromCharIndex = this._richEdit.GetPositionFromCharIndex(lpreobject.cp);
                Rectangle rc = new Rectangle(positionFromCharIndex.X, positionFromCharIndex.Y, 50, 50);
                _richEdit.Invalidate(rc, false);
            }
        }

        public void UpdateObjects(int pos)
        {
            REOBJECT lpreobject = new REOBJECT();
            IRichEditOle.GetObject(
                pos,
                lpreobject,
                GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
            UpdateObjects(lpreobject);
        }

        public void UpdateObjects(REOBJECT reObj)
        {
            Point positionFromCharIndex = _richEdit.GetPositionFromCharIndex(
                    reObj.cp);
            Size size = GetSizeFromMillimeter(reObj);
            Rectangle rc = new Rectangle(positionFromCharIndex, size);
            _richEdit.Invalidate(rc, false);
        }

        private Size GetSizeFromMillimeter(REOBJECT lpreobject)
        {
            using (Graphics graphics = Graphics.FromHwnd(_richEdit.Handle))
            {
                Point[] pts = new Point[1];
                graphics.PageUnit = GraphicsUnit.Millimeter;

                pts[0] = new Point(
                    lpreobject.sizel.Width / 100,
                    lpreobject.sizel.Height / 100);
                graphics.TransformPoints(
                    CoordinateSpace.Device,
                    CoordinateSpace.Page,
                    pts);
                return new Size(pts[0]);
            }
        }
    }
    #endregion
}
