using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;

namespace VFS.Server.Core.Commands
{
    sealed class CommandContext
    {
        public string[] Args { get; set; }
        public UserContext User { get; set; }

        public CommandContext(UserContext user)
        {
            User = user;
        }
    }
}
