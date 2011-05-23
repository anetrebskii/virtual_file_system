using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;

namespace VFS.Tests.Server.Core
{
    sealed class CommandHelper
    {
        private static VFSEngine _fsFactory = new VFSEngine();

        public static CommandContext CreateCommandContext()
        {
            CommandContext returnValue = new CommandContext(new UserContext("name"));
            returnValue.User.CurrentDirectory = GetFSManager().CreateDirectory(@"C:");
            return returnValue;
        }

        public static VFSEngine GetFSManager()
        {
            return _fsFactory;
        }
    }
}
