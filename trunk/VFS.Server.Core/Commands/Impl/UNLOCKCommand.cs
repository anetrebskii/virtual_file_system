using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    sealed class UNLOCKCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];

            IFile file = _fsManager.FindFile(pathToDelete, context.User.CurrentDirectory);
            if (file.LockedUsers.Contains(context.User.UserName))
            {
                file.LockedUsers.Remove(context.User.UserName);
            }
        }

        #endregion
    }
}
