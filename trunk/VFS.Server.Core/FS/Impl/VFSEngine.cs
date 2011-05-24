using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using VFS.Server.Core.Exceptions;

namespace VFS.Server.Core.FS.Impl
{
    sealed class VFSEngine : IFSEngine
    {
        private IDirectory _rootDirectory = new VFSDirectory();

        #region Commands

        #region MD Command

        public void CreateDirectory(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string directoryPath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                throw new FSException("Не найдена директория для создания папки");
            }
            string directoryName = Path.GetFileName(directoryPath);
            IDirectory newDirectory = CreateDirectory(directoryName);
            parentDirectory.AddDirectory(newDirectory);
        }

        #endregion

        #region RD Command

        public void RemoveDirectory(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IDirectory directoryToDelete = FindDirectory(pathToDelete, context.User.CurrentDirectory);
            if (Object.ReferenceEquals(directoryToDelete, context.User.CurrentDirectory))
            {
                throw new FSException("Нельзя удалить текущую директорию");
            }
            if (directoryToDelete.GetDirectories().Count() > 0)
            {
                throw new FSException("Данная директория содержит дочерние директории");
            }
            if (directoryToDelete.GetFiles().Count(f => f.LockedUsers.Count > 0) > 0)
            {
                throw new FSException("Невозможно удалить директорию с заблокированными файлами");
            }
            directoryToDelete.Parent.RemoveDirectory(directoryToDelete);
        }

        #endregion

        #region DELTree Command

        public void RemoveTree(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IDirectory directoryToDelete = FindDirectory(pathToDelete, context.User.CurrentDirectory);
            if (Object.ReferenceEquals(directoryToDelete, context.User.CurrentDirectory))
            {
                throw new FSException("Нельзя удалить текущую директорию");
            }
            if (directoryToDelete.Exists(
                d => d.GetDirectories(), 
                d => d.GetFiles().Count(f => f.LockedUsers.Count > 0) > 0))
            {
                throw new FSException("Невозможно удалить директорию с заблокированными файлами");
            }

            directoryToDelete.Parent.RemoveDirectory(directoryToDelete);
        }

        #endregion

        #region MF Command

        public void CreateFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string filePath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                throw new FSException("Не найдена директория для создания файла");
            }
            string newFileName = Path.GetFileName(filePath);
            parentDirectory.AddFile(CreateFile(newFileName));
        }

        #endregion

        #region DEL Command

        public void RemoveFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IFile fileToDelete = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (fileToDelete == null)
            {
                throw new FSException("Не найден файл для удаления");
            }
            if (fileToDelete.LockedUsers.Count > 0)
            {
                throw new FSException("Невозможно удалить заблокированный файл");
            }
            fileToDelete.Directory.RemoveFile(fileToDelete);
        }

        #endregion

        #region LOCK Command

        public void LockFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IFile file = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (file == null)
            {
                throw new FSException("Не найден файл для блокировки");
            }
            if (!file.LockedUsers.Contains(context.User.UserName))
            {
                file.LockedUsers.Add(context.User.UserName);
            }
        }

        #endregion

        #region UNLOCK Command

        public void UnlockFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToDelete = context.Args[0];
            IFile file = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (file.LockedUsers.Contains(context.User.UserName))
            {
                file.LockedUsers.Remove(context.User.UserName);
            }
        }

        #endregion

        #region CD Command

        public void Navigate(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string pathToNavigate = context.Args[0];
            IDirectory directoryToNavigate = FindDirectory(pathToNavigate, context.User.CurrentDirectory);
            if (directoryToNavigate == null)
            {
                throw new FSException("Не найдена указанная директория");
            }
            context.User.CurrentDirectory = directoryToNavigate;
        }

        #endregion

        #region MOVE Command

        public void Move(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (sourceDirectory == null)
            {
                IFile sourceFile = FindFile(sourcePath, context.User.CurrentDirectory);
                if (sourceFile == null)
                {
                    throw new FSException("Не найден объект для перемещения");
                }
                sourceFile.Directory.RemoveFile(sourceFile);
                destDirectory.AddFile(sourceFile);
            }
            else
            {
                if (sourceDirectory.Parent == null)
                {
                    throw new FSException("Нельзя перемещать корневую директорию");
                }
                sourceDirectory.Parent.RemoveDirectory(sourceDirectory);
                destDirectory.AddDirectory(sourceDirectory);
            }
        }

        #endregion

        #region COPY Command

        public void Copy(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (sourceDirectory == null)
            {
                IFile sourceFile = FindFile(sourcePath, context.User.CurrentDirectory);
                IFile copiedFile = (IFile)sourceFile.DeepCopy();
                destDirectory.AddFile(copiedFile);
            }
            else
            {
                if (sourceDirectory.Parent == null)
                {
                    throw new IOException();
                }
                IDirectory copiedDirectory = (IDirectory)sourceDirectory.DeepCopy();
                destDirectory.AddDirectory(copiedDirectory);
            }
        }

        #endregion

        #region PRINT Command

        public void Print(CommandContext context)
        {
            context.Response = toStringFormat(context.User.CurrentDirectory.Root, 0);
        }

        private string toStringFormat(IDirectory rootDirectory, int deepLevel)
        {
            StringBuilder returnValue = new StringBuilder();
            if (deepLevel == 0)
            {
                returnValue.Append(rootDirectory.Name);
            }
            else
            {
                returnValue.AppendFormat("{0}|_{1}", "| ".Repeat(deepLevel - 1), rootDirectory.Name);
            }
            foreach (IDirectory directory in rootDirectory.GetDirectories().OrderBy(s => s.Name))
            {
                returnValue.AppendLine();
                returnValue.Append(toStringFormat(directory, deepLevel + 1));
            }
            foreach (IFile file in rootDirectory.GetFiles().OrderBy(s => s.Name))
            {
                returnValue.AppendLine();
                string lockedInformation = file.LockedUsers.Count > 0
                    ? String.Format(" [LOCKED by {0}]", String.Join(",", file.LockedUsers.ToArray()))
                    : String.Empty;
                returnValue.AppendFormat("{0}|_{1}", "| ".Repeat(deepLevel), file.Name + lockedInformation);
            }            
            return returnValue.ToString();
        }

        #endregion

        #endregion

        #region Help methods

        private IDirectory findParentDirectory(CommandContext context)
        {
            string filePath = context.Args[0];
            if (IsAbsolutePath(filePath))
            {
                string pathToParentDirectory = UpPath(filePath);
                return FindDirectory(pathToParentDirectory, context.User.CurrentDirectory);
            }
            else
            {
                return context.User.CurrentDirectory;
            }
        }

        public string UpPath(string path)
        {
            path = RemoveOddSymbols(path);
            if (path.LastIndexOf('\\') == -1)
            {
                return path;
            }
            return path.Remove(path.LastIndexOf('\\'));
        }

        public IFile FindFile(string filePath, IDirectory currentDirectory)
        {
            if (IsAbsolutePath(filePath))
            {
                return findFileInDirectory(removeRootPath(filePath), currentDirectory.Root);
            }
            else
            {
                return findFileInDirectory(filePath, currentDirectory);
            }
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
            return new VFSFile() { Name = fileName };
        }

        public IDirectory CreateDirectory(string directoryName)
        {
            return new VFSDirectory() { Name = directoryName };
        }

        public bool IsAbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }

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
        private IFile findFileInDirectory(string path, IDirectory place)
        {
            string[] directoryNames = splitPath(path);
            foreach (string directoryName in directoryNames.Take(directoryNames.Length - 2))
            {
                IDirectory findedDirectory = place.GetDirectories().FirstOrDefault(d =>
                    String.Compare(d.Name, directoryName, StringComparison.OrdinalIgnoreCase) == 0);
                if (findedDirectory == null)
                {
                    return null;
                }
                place = findedDirectory;
            }
            if (place != null)
            {
                return place.GetFiles().FirstOrDefault(f =>
                    String.Compare(f.Name, directoryNames[directoryNames.Length - 1], StringComparison.OrdinalIgnoreCase) == 0);
            }
            return null;
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

        protected static bool hasOneParameter(string[] args)
        {
            return args != null && args.Length > 0;
        }

        protected static bool hasTwoParameters(string[] args)
        {
            return args != null && args.Length > 1;
        }

        #endregion
    }
}
