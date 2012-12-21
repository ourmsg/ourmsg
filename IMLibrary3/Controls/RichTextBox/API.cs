using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace IMLibrary3
{
    public class API
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
    }
}
