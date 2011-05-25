using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;

namespace VFS.Client
{
    class ConsoleCallback : IConsoleCallback
    {
        #region IConsoleCallback Members

        public void Receive(string msg)
        {
            Console.WriteLine(msg);
        }

        #endregion
    }
}
