using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS
{
    /// <summary>
    /// Interface for directory
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Parent directory
        /// </summary>
        IDirectory Parent { get; }

        /// <summary>
        /// Root directory
        /// </summary>
        IDirectory Root { get; }

        /// <summary>
        /// Name of the current directory
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Represent full path to directory
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Return child directories <see cref="IDirectory"/>
        /// </summary>
        IEnumerable<IDirectory> GetDirectories();

        /// <summary>
        /// Return child files <see cref="IFile"/>
        /// </summary>
        IEnumerable<IFile> GetFiles();        

        /// <summary>
        /// Add file to current directory
        /// </summary>
        /// <param name="file">file to add</param>
        void AddFile(IFile file);

        /// <summary>
        /// Remove file from current directory
        /// </summary>
        /// <param name="file">file to remove</param>
        void RemoveFile(IFile file);

        /// <summary>
        /// Add directory to current directory
        /// </summary>
        /// <param name="directory">directory to add</param>
        void AddDirectory(IDirectory directory);

        /// <summary>
        /// Remove directory from current directory
        /// </summary>
        /// <param name="directory">directory to remove</param>
        void RemoveDirectory(IDirectory directory);
    }
}
