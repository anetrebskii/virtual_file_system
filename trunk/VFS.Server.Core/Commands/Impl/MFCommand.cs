using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    internal class MFCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string filePath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                return;
            }
            string newFileName = Path.GetFileName(filePath);
            parentDirectory.AddFile(_fsManager.CreateFile(newFileName));
        }

        #endregion

        private IDirectory findParentDirectory(CommandContext context)
        {
            string filePath = context.Args[0];
            if (_fsManager.IsAbsolutePath(filePath))
            {
                string pathToParentDirectory = _fsManager.UpPath(filePath);
                return _fsManager.FindDirectory(pathToParentDirectory, context.User.CurrentDirectory);
            }
            else
            {
                return context.User.CurrentDirectory;
            }
        }
    }
}
