using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;

namespace VFS.Server.Core
{
    public sealed class CommonContext
    {
        public IEnumerable<UserContext> AuthenticatedUsers { get; private set; }

        public CommonContext(IEnumerable<UserContext> authenticatedUsers)
        {
            AuthenticatedUsers = authenticatedUsers;
        }
    }
}
