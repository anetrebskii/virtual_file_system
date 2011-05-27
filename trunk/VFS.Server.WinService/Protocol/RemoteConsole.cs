using System;
using System.Collections.Generic;
using VFS.Common;
using System.ServiceModel;
using VFS.Server.Core;

namespace VFS.Server.WinService.Protocol
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
        private static readonly List<ConnectedUser> ConnectedUsers = new List<ConnectedUser>();

        /// <summary>
        /// Server console to handle user commands
        /// </summary>
        private static readonly ServerConsole ServerConsole = new ServerConsole();

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
        public AuthenticationResult Authenticate(string userName)
        {
            if (ConnectedUsers.Exists(u =>
                                      String.Compare(u.Context.UserName, userName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                return AuthenticationResult.Failed();
            }
            IConsoleCallback callback = OperationContext.Current.GetCallbackChannel<IConsoleCallback>();
            _currentUser = new ConnectedUser(ServerConsole.Authenticate(userName), callback);
            ConnectedUsers.Add(_currentUser);
            return AuthenticationResult.Success(ConnectedUsers.Count - 1);
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
            ConnectedUsers.Remove(_currentUser);
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
            Response handleResult = ServerConsole.HandleCommand(command, _currentUser.Context);
            if (handleResult.SystemChanged)
            {
                ConnectedUsers.ForEach(u =>
                                           {
                                               if (!ReferenceEquals(_currentUser, u))
                                               {
                                                   u.Callback.Receive(
                                                       String.Format("{0} performs command: {1}", _currentUser.Context.UserName, command));
                                               }
                                           });
            }
            return handleResult.Text;
        }

        #endregion
    }
}