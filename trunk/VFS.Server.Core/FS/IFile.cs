using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS
{
    interface IFile
    {
        string Name { get; set; }

        void Lock();
        void Unlock();
    }
}
