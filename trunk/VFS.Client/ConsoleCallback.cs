using System;
using VFS.Common;

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
            Console.WriteLine();
            Console.WriteLine(msg);
        }

        #endregion
    }
}
