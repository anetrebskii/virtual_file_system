using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Exceptions;

namespace VFS.Server.Core.FS.Impl
{
    /// <summary>
    /// Represent directory of a virtual file system
    /// </summary>
    [Serializable]
    sealed class VFSDirectory : IDirectory
    {
        /// <summary>
        /// Child directories <see cref="IDirectory"/>
        /// </summary>
        private List<VFSDirectory> _directories = new List<VFSDirectory>();

        /// <summary>
        /// Child files <see cref="IFile"/>
        /// </summary>
        private List<IFile> _files = new List<IFile>();

        /// <summary>
        /// Initialize new instance of class <see cref="VFSDirectory"/>
        /// </summary>
        public VFSDirectory()
        {
            Root = this;
        }

        #region IDirectory Members

        /// <summary>
        /// Parent directory
        /// </summary>
        public IDirectory Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Root directory
        /// </summary>
        public IDirectory Root
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the current directory
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Represent full path to directory
        /// </summary>
        /// <remarks>
        /// User recursive call to present the full path
        /// </remarks>
        public string FullPath
        {
            get
            {
                if (Parent == null)
                {
                    return Name;
                }
                return String.Format("{0}{1}{2}", Parent.FullPath, VFSEngine.SEPARATOR, Name);
            }
        }               

        /// <summary>
        /// Return child directories <see cref="IDirectory"/>
        /// </summary>
        public IEnumerable<IDirectory> GetDirectories()
        {
            return _directories.Cast<IDirectory>();
        }

        /// <summary>
        /// Return child files <see cref="IFile"/>
        /// </summary>
        public IEnumerable<IFile> GetFiles()
        {
            return _files;
        }

        /// <summary>
        /// Add file to current directory
        /// </summary>
        /// <param name="file">file to add</param>
        /// 
        /// <exception cref="FSException">
        /// If already exists file or directory with name of the new file <paramref name="directory"/>
        /// </exception>
        public void AddFile(IFile file)
        {
            if (_directories.Exists(d => d.Name.EqualByOrdinalIgnoreCase(file.Name)))
            {
                throw new FSException("Директория с данным именем уже существует");
            }
            if (_files.Exists(f => f.Name.EqualByOrdinalIgnoreCase(file.Name)))
            {
                throw new FSException("Файл с данным именем уже существует");
            }
            file.Directory = this;
            _files.Add(file);
        }

        /// <summary>
        /// Remove file from current directory
        /// </summary>
        /// <param name="file">file to remove</param>
        public void RemoveFile(IFile file)
        {
            file.Directory = null;
            _files.Remove(file);
        }

        /// <summary>
        /// Add directory to current directory
        /// </summary>
        /// <param name="directory">directory to add</param>
        /// 
        /// <exception cref="FSException">
        /// If already exists file or directory with name of the new directory <paramref name="directory"/>
        /// </exception>
        public void AddDirectory(IDirectory directory)
        {
            if (_directories.Exists(d => d.Name.EqualByOrdinalIgnoreCase(directory.Name)))
            {
                throw new FSException("Директория с данным именем уже существует");
            }
            if (_files.Exists(f => f.Name.EqualByOrdinalIgnoreCase(directory.Name)))
            {
                throw new FSException("Файл с данным именем уже существует");
            }
            ((VFSDirectory)directory).Parent = this;
            ((VFSDirectory)directory).Root = this.Root;
            _directories.Add((VFSDirectory)directory);
        }

        /// <summary>
        /// Remove directory from current directory
        /// </summary>
        /// <param name="directory">directory to remove</param>
        public void RemoveDirectory(IDirectory directory)
        {
            ((VFSDirectory)directory).Parent = null;
            _directories.Remove((VFSDirectory)directory);
        }

        #endregion
    }
}
