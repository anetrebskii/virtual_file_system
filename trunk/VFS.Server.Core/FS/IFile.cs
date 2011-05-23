using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS
{
    interface IFile
    {
        IDirectory Directory { get; set; }
        string Name { get; set; }

        List<string> LockedUsers { get; }
    }
}
