using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;
using VFS.Server.Core.Commands;

namespace VFS.Server.Console.Protocol
{
    sealed class ConnectedUser
    {
        public IConsoleCallback Callback { get; set; }
        public UserContext Context { get; set; }
    }
}
