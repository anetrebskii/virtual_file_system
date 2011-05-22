using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VFS.Server.Core.FS.Impl
{
    sealed class VFSManager : IFSManager
    {
        #region IFSManager Members

        public string UpPath(string path)
        {
            path = RemoveOddSymbols(path);
            if (path.LastIndexOf('\\') == -1)
            {
                return path;
            }
            return path.Remove(path.LastIndexOf('\\'));
        }

        public IDirectory FindDirectory(string directoryPath, IDirectory currentDirectory)
        {
            if (IsAbsolutePath(directoryPath))
            {
                return findDirectoryInDirectory(removeRootPath(directoryPath), currentDirectory.Root);
            }
            else
            {
                return findDirectoryInDirectory(directoryPath, currentDirectory);
            }
        }        

        public IFile CreateFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public IDirectory CreateDirectory(string directoryName)
        {
            return new VFSDirectory() { Name = directoryName };
        }

        public bool IsAbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }

        #endregion

        private string removeRootPath(string path)
        {
            return RemoveOddSymbols(path.Replace(Path.GetPathRoot(path), String.Empty));
        }

        /// <summary>
        /// Find directory by path in current directory <paramref name="place"/>
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="place">Place to searching</param>
        /// <returns><c>null</c> - if directory not finded</returns>
        private IDirectory findDirectoryInDirectory(string path, IDirectory place)
        {
            string[] directoryNames = splitPath(path);
            foreach (string directoryName in directoryNames)
            {
                IDirectory findedDirectory = place.GetDirectories().FirstOrDefault(d =>
                    String.Compare(d.Name, directoryName, StringComparison.OrdinalIgnoreCase) == 0);
                if (findedDirectory == null)
                {
                    return null;
                }
                place = findedDirectory;
            }
            return place;
        }

        /// <summary>
        /// Remove odd symbols from path <paramref name="path"/>
        /// </summary>
        /// <param name="path">path to remove odd symbols</param>
        /// <returns>Clear path, without odd symbols</returns>
        private string RemoveOddSymbols(string path)
        {
            return path = path.Trim(' ', '\\');
        }

        /// <summary>
        /// Split path to array of the folder names
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Array of folder and file names</returns>
        private string[] splitPath(string path)
        {
            return path.Split('\\');
        }  
    }
}
