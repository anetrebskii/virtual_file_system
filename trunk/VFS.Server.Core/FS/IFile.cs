using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS
{
    /// <summary>
    /// Interface for file
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Parent directory of the current file
        /// </summary>
        IDirectory Directory { get; set; }

        /// <summary>
        /// Name of the current file
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Represent full path to file
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Users, who lock current file
        /// </summary>
        List<string> LockedUsers { get; }
    }
}
