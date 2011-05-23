using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    internal class MOVECommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = _fsManager.FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = _fsManager.FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (sourceDirectory == null)
            {
                IFile sourceFile = _fsManager.FindFile(sourcePath, context.User.CurrentDirectory);
                sourceFile.Directory.RemoveFile(sourceFile);
                destDirectory.AddFile(sourceFile);
            }
            else
            {                
                if (sourceDirectory.Parent == null)
                {
                    throw new IOException();
                }
                sourceDirectory.Parent.RemoveDirectory(sourceDirectory);
                destDirectory.AddDirectory(sourceDirectory);
            }
            
        }

        #endregion
    }
}
