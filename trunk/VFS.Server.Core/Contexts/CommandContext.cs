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
        /// Answer from system to user
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Keep common information
        /// </summary>
        public IEnumerable<UserContext> OtherUsers { get; private set; }

        /// <summary>
        /// Initialize new instance of class <see cref="CommandContext"/>
        /// </summary>
        /// <param name="user">User, who execute command</param>
        /// <param name="otherUsers">Other users</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="user"/> or <paramref name="otherUsers"/> is null
        /// </exception>
        public CommandContext(UserContext user, IEnumerable<UserContext> otherUsers)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (otherUsers == null)
            {
                throw new ArgumentNullException("otherUsers");
            }
            User = user;
            OtherUsers = otherUsers;
        }
    }
}
