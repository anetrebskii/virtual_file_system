using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS.Impl;

namespace VFS.Server.Core.Commands
{
    abstract class BaseCommand : ICommand
    {
        protected IFSManager _fsManager = new VFSManager();

        #region ICommand Members

        public abstract void Execute(CommandContext context);

        #endregion

        protected static bool hasOneParameter(string[] args)
        {
            return args != null && args.Length > 0;
        }

        protected static bool hasTwoParameters(string[] args)
        {
            return args != null && args.Length > 1;
        }
    }
}
