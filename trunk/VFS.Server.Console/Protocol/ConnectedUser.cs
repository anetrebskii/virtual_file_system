using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;
using VFS.Server.Core.Commands;

namespace VFS.Server.Console.Protocol
{
    /// <summary>
    /// Represent connected user
    /// </summary>
    sealed class ConnectedUser
    {
        /// <summary>
        /// Provide callback invoke from server
        /// </summary>
        public IConsoleCallback Callback { get; private set; }

        /// <summary>
        /// User context
        /// </summary>
        public UserContext Context { get; private set; }

        /// <summary>
        /// Initialize instance of the class <see cref="ConnectedUser"/>
        /// </summary>
        /// <param name="userContext">user context</param>
        /// <param name="callback">Provide callback invoke from server</param>
        public ConnectedUser(UserContext userContext, IConsoleCallback callback)
        {
            Context = userContext;
            Callback = callback;
        }
    }
}
