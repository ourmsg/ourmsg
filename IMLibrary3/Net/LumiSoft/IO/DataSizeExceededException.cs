using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Net.IO
{
    /// <summary>
    /// The exception that is thrown when maximum allowed data size has exceeded.
    /// </summary>
    public class DataSizeExceededException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataSizeExceededException() : base()
        {
        }
    }
}
