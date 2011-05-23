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
    sealed class CommandContext
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
        /// Answer from system to user
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Initialize new instance of class <see cref="CommandContext"/>
        /// </summary>
        /// <param name="user">User, who execute command</param>
        /// 
        /// <exception cref="ArgumentNullException">if <paramref name="user"/> is null</exception>
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
