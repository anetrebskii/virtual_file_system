using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;

namespace VFS.Server.Core.Commands
{
    sealed class UserContext
    {
        public string UserName { get; private set; }
        public IDirectory CurrentDirectory { get; set; }

        public UserContext(string userName)
        {
            UserName = userName;
        }
    }
}
