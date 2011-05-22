using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS
{
    interface IDirectory
    {
        IDirectory Parent { get; }
        IDirectory Root { get; }

        IEnumerable<IDirectory> GetDirectories();
        IEnumerable<IFile> GetFiles();

        string Name { get; set; }
        string FullPath { get; }

        void AddFile(IFile file);
        void RemoveFile(IFile file);

        void AddDirectory(IDirectory directory);
        void RemoveDirectory(IDirectory directory);
    }
}
