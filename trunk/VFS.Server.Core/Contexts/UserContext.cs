using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;

namespace VFS.Server.Core.Commands
{
    /// <summary>
    /// User context
    /// </summary>
    public sealed class UserContext
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Direcory, where user
        /// </summary>
        public IDirectory CurrentDirectory { get; internal set; }

        /// <summary>
        /// Initialize new instance of class <see cref="UserContext"/>
        /// </summary>
        /// <param name="userName">User name</param>
        /// 
        /// <exception cref="ArgumentNullException">if <paramref name="userName"/> is null or empty</exception>
        public UserContext(string userName)
        {
            if (userName == null || userName.Trim() == String.Empty)
            {
                throw new ArgumentNullException("userName");
            }
            UserName = userName;
        }
    }
}
