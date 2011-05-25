using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VFS.Server.Core.Exceptions
{
    /// <summary>
    /// Represent exception, where arguments are not enough
    /// </summary>
    [Serializable]
    public class FewArgumentsException : Exception
    {
        public FewArgumentsException() { }

        public FewArgumentsException(string message) : base(message) 
        {
        }

        public FewArgumentsException(string message, Exception inner) : base(message, inner) 
        { 

        }

        protected FewArgumentsException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
