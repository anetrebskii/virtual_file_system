using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    internal class MDCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string directoryPath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                return;
            }
            string directoryName = Path.GetFileName(directoryPath);
            IDirectory newDirectory = _fsManager.CreateDirectory(directoryName);
            parentDirectory.AddDirectory(newDirectory);
        }

        #endregion

        private IDirectory findParentDirectory(CommandContext context)
        {
            
            string directoryPath = context.Args[0];
            if (_fsManager.IsAbsolutePath(directoryPath))
            {
                string pathToParentDirectory = _fsManager.UpPath(directoryPath);
                return _fsManager.FindDirectory(pathToParentDirectory, context.User.CurrentDirectory);
            }
            else
            {                
                return context.User.CurrentDirectory;
            }
        }
    }
}
