using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;

namespace VFS.Server.Core.Commands
{
    /// <summary>
    /// Context for execute user command
    /// </summary>
    public sealed class CommandContext
    {
        /// <summary>
        /// Arguments for commands
        /// </summary>
        public string[] Args { get; set; }

        /// <summary>
        /// User, who execute command
        /// </summary>
        public UserContext User { get; set; }

        /// <summary>
        /// Initialize new instance of class <see cref="CommandContext"/>
        /// </summary>
        /// <param name="user">User, who execute command</param>
        /// <param name="otherUsers">Other users</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="user"/> or <paramref name="otherUsers"/> is null
        /// </exception>
        public CommandContext(UserContext user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            User = user;
        }
    }
}
