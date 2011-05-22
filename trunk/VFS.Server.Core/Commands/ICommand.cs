using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.Commands
{
    interface ICommand
    {
        void Execute(CommandContext context);
    }
}
