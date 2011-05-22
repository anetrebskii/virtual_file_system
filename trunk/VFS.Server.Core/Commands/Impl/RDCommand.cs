using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    sealed class RDCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IDirectory directoryToDelete = _fsManager.FindDirectory(pathToDelete, context.User.CurrentDirectory);            
            if (Object.ReferenceEquals(directoryToDelete, context.User.CurrentDirectory)
                || directoryToDelete.GetDirectories().Count() > 0)
            {
                throw new IOException();
            }
            directoryToDelete.Parent.RemoveDirectory(directoryToDelete);
        }

        #endregion
    }
}
