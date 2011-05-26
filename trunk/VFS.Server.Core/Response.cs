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

        /// <summary>
        /// Initialize instance of a <see cref="Response"/> to represent success handle
        /// </summary>
        /// <param name="textResponse">text of the response</param>
        /// <param name="systemChanged">sytem was changed</param>
        /// <returns>Represent success handle</returns>
        public static Response Success(string textResponse, bool systemChanged)
        {
            return new Response { SystemChanged = systemChanged, IsSuccess = true, Text = textResponse };
        }

        /// <summary>
        /// Initialize instance of a <see cref="Response"/> to represent success handle
        /// </summary>
        /// <param name="systemChanged">sytem was changed</param>
        /// <returns>Represent success handle</returns>
        public static Response Success(bool systemChanged)
        {
            return new Response { SystemChanged = systemChanged, IsSuccess = true };
        }

        /// <summary>
        /// Initialize instance of a <see cref="Response"/> to represent failed handle
        /// </summary>
        /// <param name="textResponse">text of the response</param>
        /// <returns>Represent failed handle</returns>
        public static Response Failed(string textResponse)
        {
            return new Response() { Text = textResponse };
        }
    }
}
