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
    
        }

        #endregion

        private IDirectory findParentDirectory(CommandContext context)
        {
            return null;
        }
    }
}
