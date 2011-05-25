using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Contract
{
    /// <summary>
    /// Interface to callback invoke from server to client
    /// </summary>
    public interface IConsoleCallback
    {
        /// <summary>
        /// Handle receive message from server
        /// </summary>
        /// <param name="msg">message from server</param>
        [OperationContract(IsOneWay = true)]
        void Receive(string msg);
    }
}
