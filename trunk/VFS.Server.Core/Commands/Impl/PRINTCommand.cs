using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;
using System.IO;

namespace VFS.Server.Core.Commands.Impl
{
    sealed class PRINTCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }            
        }

        #endregion
    }
}
