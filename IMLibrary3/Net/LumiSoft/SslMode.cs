using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Net
{
    /// <summary>
    /// This enum holds SSL modes.
    /// </summary>
    public enum SslMode
    {
        /// <summary>
        /// No SSL is used.
        /// </summary>
        None,

        /// <summary>
        /// Connection is SSL.
        /// </summary>
        SSL,

        /// <summary>
        /// Connection will be switched to SSL with start TLS.
        /// </summary>
        TLS
    }
}
