using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.Contexts
{
    /// <summary>
    /// Represent handle result 
    /// </summary>
    public struct Response
    {
        /// <summary>
        /// Text represent of a response
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Request handle success
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Represent information about that system changed after handle command
        /// </summary>
        public bool SystemChanged { get; set; }

        public static Response Success(string textResponse, bool systemChanged)
        {
            return new Response { SystemChanged = systemChanged, IsSuccess = true, Text = textResponse };
        }

        public static Response Success(bool systemChanged)
        {
            return new Response { SystemChanged = systemChanged, IsSuccess = true };
        }

        public static Response Failed(string textResponse)
        {
            return new Response() { Text = textResponse };
        }
    }
}
