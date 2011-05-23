using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VFS.Server.Core.Exceptions
{
    /// <summary>
    /// Represent exception from file system
    /// </summary>
    [Serializable]
    public class FSException : Exception
    {
        public FSException() { }

        public FSException(string message) : base(message) 
        {
        }

        public FSException(string message, Exception inner) : base(message, inner) 
        { 

        }

        protected FSException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
