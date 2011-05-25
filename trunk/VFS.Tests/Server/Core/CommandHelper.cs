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
        public static List<UserContext> _users = new List<UserContext>();

        public static CommandContext CreateCommandContext()
        {
            UserContext currentUser = new UserContext("name");
            CommandContext returnValue = new CommandContext(currentUser, _users);
            returnValue.User.CurrentDirectory = CreateDirectory(@"C:");
            return returnValue;
        }

        public static IDirectory CreateDirectory(string name)
        {
            return new VFSDirectory() { Name = name };
        }

        public static IFile CreateFile(string name)
        {
            return new VFSFile() { Name = name };
        }


    }
}
