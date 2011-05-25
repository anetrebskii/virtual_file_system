using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;

namespace VFS.Client
{
    /// <summary>
    /// Provide callback invoke from server
    /// </summary>
    sealed class ConsoleCallback : IConsoleCallback
    {
        #region IConsoleCallback Members

        /// <summary>
        /// Handle receive message from server
        /// </summary>
        /// <param name="msg">message from server</param>
        public void Receive(string msg)
        {
            Console.WriteLine(msg);
        }

        #endregion
    }
}
