using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core
{
    /// <summary>
    /// Represent handle result 
    /// </summary>
    public struct HandleResult
    {
        /// <summary>
        /// Response after handle command
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Represent information about that system was changed after handle command
        /// </summary>
        public bool SystemChanged { get; set; }

        /// <summary>
        /// Represent empty handle result
        /// </summary>
        public static HandleResult Empty { get; private set; }
    }
}
