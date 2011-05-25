using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Contract
{
    /// <summary>
    /// Interface of the remote console on the server
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IConsoleCallback))]  
    public interface IRemoteConsole
    {
        /// <summary>
        /// Authenticate on the server
        /// </summary>
        /// <param name="userName">user name for authentication</param>
        /// <returns><c>true</c> - authentication success</returns>
        [OperationContract(IsInitiating = true, IsOneWay = false, IsTerminating = false)]
        AuthenticationResult Authenticate(string userName);

        /// <summary>
        /// Exit from the server
        /// </summary>
        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = true)] 
        void Quite();

        /// <summary>
        /// Send command to handling on the server
        /// </summary>
        /// <param name="command">command in text format</param>
        /// <returns>Respoinse from server</returns>
        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = false)]  
        string SendCommand(string command);

        /// <summary>
        /// Return user position in file system of a server
        /// </summary>
        /// <returns>Path of the user position</returns>
        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = false)]
        string GetUserPosition();
    }
}
