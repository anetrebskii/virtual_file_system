using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS.Impl
{
    interface IFSManager
    {
        string UpPath(string path);
        bool IsAbsolutePath(string path);
        IFile FindFile(string filePath, IDirectory currentDirectory);
        IDirectory FindDirectory(string directoryPath, IDirectory currentDirectory);

        IFile CreateFile(string fileName);
        IDirectory CreateDirectory(string directoryName);
    }
}
