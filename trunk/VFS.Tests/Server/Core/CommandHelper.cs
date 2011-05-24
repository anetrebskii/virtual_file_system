using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core;

namespace VFS.Tests.Server.Core
{
    sealed class CommandHelper
    {
        private static VFSEngine _fsFactory = new VFSEngine();

        public static CommandContext CreateCommandContext()
        {
            UserContext currentUser = new UserContext("name");
            CommonContext common = new CommonContext(new List<UserContext>() { currentUser });
            CommandContext returnValue = new CommandContext(currentUser, common);
            returnValue.User.CurrentDirectory = GetFSManager().CreateDirectory(@"C:");
            return returnValue;
        }

        public static VFSEngine GetFSManager()
        {
            return _fsFactory;
        }
    }
}
