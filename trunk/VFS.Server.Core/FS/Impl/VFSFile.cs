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
        /// Represent full path to directory
        /// </summary>
        /// <remarks>
        /// User recursive call to present the full path
        /// </remarks>
        public string FullPath
        {
            get
            {
                return String.Format("{0}{1}{2}", Directory.FullPath, VFSEngine.SEPARATOR, Name);
            }
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
