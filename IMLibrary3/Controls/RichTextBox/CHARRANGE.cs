using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace IMLibrary3
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CHARRANGE
    {
        public int cpMin;
        public int cpMax;
    }
}
