using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS.Impl
{
    [Serializable]
    sealed class VFSDirectory : IDirectory
    {
        private List<VFSDirectory> _directories = new List<VFSDirectory>();
        private List<IFile> _files = new List<IFile>();

        public VFSDirectory()
        {
            Root = this;
        }

        #region IDirectory Members

        public IDirectory Parent
        {
            get;
            set;
        }

        public IDirectory Root
        {
            get;
            set;
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            return _directories.Cast<IDirectory>();
        }

        public IEnumerable<IFile> GetFiles()
        {
            return _files;
        }

        public string Name
        {
            get;
            set;
        }

        public void AddFile(IFile file)
        {
            file.Directory = this;
            _files.Add(file);
        }

        public void RemoveFile(IFile file)
        {
            file.Directory = null;
            _files.Remove(file);
        }

        public void AddDirectory(IDirectory directory)
        {
            if (_directories.Exists(d =>
                String.Compare(d.Name, directory.Name, StringComparison.OrdinalIgnoreCase) == 0))
            {
                throw new DirectoryAlreadyExistsException();
            }
            ((VFSDirectory)directory).Parent = this;
            ((VFSDirectory)directory).Root = this.Root;
            _directories.Add((VFSDirectory)directory);
        }

        public void RemoveDirectory(IDirectory directory)
        {
            ((VFSDirectory)directory).Parent = null;
            _directories.Remove((VFSDirectory)directory);
        }

        public string FullPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
