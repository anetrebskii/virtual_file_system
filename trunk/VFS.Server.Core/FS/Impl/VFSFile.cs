using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFS.Server.Core.FS.Impl
{
    /// <summary>
    /// Represent file of a virtual file system
    /// </summary>
    [Serializable]
    sealed class VFSFile : IFile
    {
        /// <summary>
        /// Initialize new instance of class <see cref="VFSFile"/>
        /// </summary>
        public VFSFile()
        {
            LockedUsers = new List<string>();
        }

        #region IFile Members

        /// <summary>
        /// Name of the current file
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Parent directory of the current file
        /// </summary>
        public IDirectory Directory
        {
            get;
            set;
        }

        /// <summary>
        /// Users, who lock current file
        /// </summary>
        public List<string> LockedUsers { get; private set; }

        #endregion
    }
}
