using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using System.Security.Authentication;

namespace VFS.Server.Core
{
    sealed class AuthenticationManager
    {
        private List<UserContext> _users = new List<UserContext>();

        public bool Authenticate(string userName)
        {
            if (userName == null || userName.Trim() == String.Empty)
            {
                return false;
            }
            if (_users.Exists(u => u.UserName == userName))
            {
                throw new AuthenticationException("User with this name already authenticated");
            }
            _users.Add(new UserContext(userName));
            return true;
        }

        public void Quite(string userName)
        {
            UserContext userContext = _users.Find(u => u.UserName == userName);
            if (userContext != null)
            {
                _users.Remove(userContext);
            }
        }
    }
}
