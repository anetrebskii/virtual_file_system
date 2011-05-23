using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS.Impl
{
    [Serializable]
    sealed class VFSFile : IFile
    {
        public VFSFile()
        {
            LockedUsers = new List<string>();
        }

        #region IFile Members

        public string Name
        {
            get;
            set;
        }

        public List<string> LockedUsers { get; private set; }

        #endregion

        #region IFile Members

        public IDirectory Directory
        {
            get;
            set;
        }

        #endregion
    }
}
