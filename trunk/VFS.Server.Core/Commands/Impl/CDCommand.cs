using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;

namespace VFS.Server.Core.Commands.Impl
{
    sealed class CDCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToNavigate = context.Args[0];
            IDirectory directoryToNavigate = _fsManager.FindDirectory(pathToNavigate, context.User.CurrentDirectory);
            if (directoryToNavigate == null)
            {
                throw new Exception();
            }
            context.User.CurrentDirectory = directoryToNavigate;
        }

        #endregion
    }
}
