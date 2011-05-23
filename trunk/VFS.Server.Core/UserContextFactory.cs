using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;

namespace VFS.Server.Core
{
    sealed class UserContextFactory
    {
        private IDirectory _rootDirectory;
        private VFSEngine _fsFactory;

        public UserContextFactory()
        {
            _fsFactory = new VFSEngine();
            _rootDirectory = _fsFactory.CreateDirectory(@"c:\");
        }

        public UserContext Create(string userName)
        {
            return new UserContext(userName)
                {
                    CurrentDirectory = _rootDirectory
                };
        }
    }
}
