using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;
using System.ServiceModel;
using VFS.Server.Core.Commands;
using VFS.Server.Core;
using System.Security.Authentication;

namespace VFS.Server.Console.Protocol
{
    /// <summary>
    /// Represent remote console to work with file system
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    sealed class RemoteConsole : IRemoteConsole
    {
        /// <summary>
        /// All connected and authenticated users
        /// </summary>
        private static List<ConnectedUser> _connectedUsers = new List<ConnectedUser>();

        /// <summary>
        /// Server console to handle user commands
        /// </summary>
        private static ServerConsole _console = new ServerConsole();

        /// <summary>
        /// Current connected user
        /// </summary>
        private ConnectedUser _currentUser;

        #region IRemoteConsole Members

        /// <summary>
        /// Authenticate on the server
        /// </summary>
        /// <param name="userName">user name for authentication</param>
        /// <returns><c>true</c> - authentication success</returns>
        public bool Authenticate(string userName)
        {
            if (_connectedUsers.Exists(u =>
                String.Compare(u.Context.UserName, userName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                return false;
            }
            IConsoleCallback callback = OperationContext.Current.GetCallbackChannel<IConsoleCallback>();
            _currentUser = new ConnectedUser(_console.Authenticate(userName), callback);
            _connectedUsers.Add(_currentUser);
            return true;
        }

        /// <summary>
        /// Return user position in file system of a server
        /// </summary>
        /// <returns>Path of the user position</returns>
        public string GetUserPosition()
        {
            return _currentUser.Context.CurrentDirectory.FullPath;
        }

        /// <summary>
        /// Exit from the server
        /// </summary>
        public void Quite()
        {
            _connectedUsers.Remove(_currentUser);
            _currentUser = null;
            //OperationContext.Current.Channel.Abort();
        }

        /// <summary>
        /// Handle command from client
        /// </summary>
        /// <param name="command">command in text format</param>
        /// <returns>Respoinse from server</returns>
        public string SendCommand(string command)
        {
            string response = _console.InputCommand(command, _currentUser.Context, _connectedUsers.Select(u => u.Context));
            _connectedUsers.ForEach(u => 
                {
                    if (!ReferenceEquals(_currentUser, u))
                    {
                        u.Callback.Receive(
                            String.Format("{0} performs command: {1}", _currentUser.Context.UserName, command));
                    }
                });
            return response;
        }

        #endregion
    }
}
