using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using VFS.Server.Core.Exceptions;
using VFS.Server.Core.Contexts;
using System.Threading;

namespace VFS.Server.Core.FS.Impl
{
    /// <summary>
    /// Represent virtual file system
    /// </summary>
    sealed class VFSEngine : IFSEngine
    {
        /// <summary>
        /// Separator for path
        /// </summary>
        public static readonly char SEPARATOR = '\\';

        /// <summary>
        /// Root directory
        /// </summary>
        private IDirectory _rootDirectory = new VFSDirectory() { Name = "C:" };

        /// <summary>
        /// Return default directory for this file system
        /// </summary>
        /// <returns></returns>
        public IDirectory GetDefaultDirectory()
        {
            return _rootDirectory;
        }

        #region Commands

        #region MD Command

        /// <summary>
        /// Create new directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response CreateDirectory(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                Response.Failed("1 аргумент - путь к директории");
            }
            string directoryPath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                return Response.Failed("Не найдена директория для создания папки");
            }
            string directoryName = Path.GetFileName(directoryPath);
            IDirectory newDirectory = new VFSDirectory { Name = directoryName };
            parentDirectory.AddDirectory(newDirectory);
            return Response.Success(true);
        }

        #endregion

        #region RD Command

        /// <summary>
        /// Remove directory from file system
        /// </summary>   
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response RemoveDirectory(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к директории");
            }
            string pathToDelete = context.Args[0];
            IDirectory directoryToDelete = FindDirectory(pathToDelete, context.User.CurrentDirectory);
            if (directoryToDelete == null)
            {
                return Response.Failed("Не найдена директория для удаления");
            }
            if (Object.ReferenceEquals(directoryToDelete, context.User.CurrentDirectory))
            {
                return Response.Failed("Нельзя удалить текущую директорию");
            }
            if (directoryToDelete.GetDirectories().Count() > 0)
            {
                return Response.Failed("Данная директория содержит дочерние директории");
            }
            if (directoryToDelete.GetFiles().Count(f => f.LockedUsers.Count > 0) > 0)
            {
                return Response.Failed("Невозможно удалить директорию с заблокированными файлами");
            }
            if (((VFSDirectory)directoryToDelete).CountUsers > 0)
            {
                return Response.Failed("Невозможно удалить директорию, т.к. она используется другими пользователями");
            }
            directoryToDelete.Parent.RemoveDirectory(directoryToDelete);
            return Response.Success(true);
        }

        #endregion

        #region DELTree Command

        /// <summary>
        /// Remove directory with child directories
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response RemoveTree(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к директории");
            }
            string pathToDelete = context.Args[0];
            IDirectory directoryToDelete = FindDirectory(pathToDelete, context.User.CurrentDirectory);
            if (directoryToDelete == null)
            {
                return Response.Failed("Не найдена директория для удаления");
            }
            if (Object.ReferenceEquals(directoryToDelete, context.User.CurrentDirectory))
            {
                return Response.Failed("Нельзя удалить текущую директорию");
            }
            if (directoryToDelete.Exists(
                d => d.GetDirectories(), 
                d => d.GetFiles().Count(f => f.LockedUsers.Count > 0) > 0))
            {
                return Response.Failed("Невозможно удалить директорию с заблокированными файлами");
            }
            if (directoryToDelete.Exists(
                d => d.GetDirectories(),
                d => ((VFSDirectory)d).CountUsers > 0))
            {
                return Response.Failed("Невозможно удалить директорию, т.к. она или ее поддиректории используется другими пользователями");
            }
            directoryToDelete.Parent.RemoveDirectory(directoryToDelete);
            return Response.Success(true);
        }

        #endregion

        #region MF Command

        /// <summary>
        /// Create new file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response CreateFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к файлу");
            }
            string filePath = context.Args[0];
            IDirectory parentDirectory = findParentDirectory(context);
            if (parentDirectory == null)
            {
                return Response.Failed("Не найдена директория для создания файла");
            }
            string newFileName = Path.GetFileName(filePath);
            parentDirectory.AddFile(new VFSFile() { Name = newFileName });
            return Response.Success(true);
        }

        #endregion

        #region DEL Command

        /// <summary>
        /// Remove file from file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response RemoveFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к файлу");
            }
            string pathToDelete = context.Args[0];
            IFile fileToDelete = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (fileToDelete == null)
            {
                return Response.Failed("Не найден файл для удаления");
            }
            if (fileToDelete.LockedUsers.Count > 0)
            {
                return Response.Failed("Невозможно удалить заблокированный файл");
            }
            fileToDelete.Directory.RemoveFile(fileToDelete);
            return Response.Success(true);
        }

        #endregion

        #region LOCK Command

        /// <summary>
        /// Lock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response LockFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к файлу");
            }
            string pathToDelete = context.Args[0];
            IFile file = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (file == null)
            {
                return Response.Failed("Не найден файл для блокировки");
            }
            if (!file.LockedUsers.Contains(context.User.UserName))
            {
                file.LockedUsers.Add(context.User.UserName);
            }
            return Response.Success(true);
        }

        #endregion

        #region UNLOCK Command

        /// <summary>
        /// Unlock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response UnlockFile(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к файлу");
            }
            string pathToDelete = context.Args[0];
            IFile file = FindFile(pathToDelete, context.User.CurrentDirectory);
            if (file == null)
            {
                return Response.Failed("Не найден файл для разблокировки");
            }
            if (file.LockedUsers.Contains(context.User.UserName))
            {
                file.LockedUsers.Remove(context.User.UserName);
            }
            return Response.Success(true);
        }

        #endregion

        #region CD Command

        /// <summary>
        /// Navigate to another directory
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response Navigate(CommandContext context)
        {
            if (!hasOneParameter(context.Args))
            {
                return Response.Failed("1 аргумент - путь к директории");
            }
            string pathToNavigate = context.Args[0];
            IDirectory directoryToNavigate = FindDirectory(pathToNavigate, context.User.CurrentDirectory);
            if (directoryToNavigate == null)
            {
                return Response.Failed("Не найдена указанная директория");
            }
            Interlocked.Increment(ref ((VFSDirectory)directoryToNavigate).CountUsers);
            context.User.CurrentDirectory = directoryToNavigate;
            Interlocked.Increment(ref ((VFSDirectory)directoryToNavigate).CountUsers);
            return Response.Success(false);
        }

        #endregion

        #region MOVE Command

        /// <summary>
        /// Move file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response Move(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                return Response.Failed("1 аргумент - путь к директории или файлу, 2 - путь к директории");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (destDirectory == null)
            {
                return Response.Failed("Не найдена директория, куда нужно переместить объект");
            }
            if (sourceDirectory == null)
            {
                IFile sourceFile = FindFile(sourcePath, context.User.CurrentDirectory);
                if (sourceFile == null)
                {
                    return Response.Failed("Не найден объект для перемещения");
                }
                sourceFile.Directory.RemoveFile(sourceFile);
                destDirectory.AddFile(sourceFile);
            }
            else
            {
                if (sourceDirectory.Parent == null)
                {
                    return Response.Failed("Нельзя перемещать корневую директорию");
                }
                if (sourceDirectory.Exists(
                    d => d.GetDirectories(),
                    d => d.GetFiles().Count(f => f.LockedUsers.Count > 0) > 0))
                {
                    return Response.Failed("Невозможно переместить директорию с заблокированными файлами");
                }
                if (sourceDirectory.Exists(
                    d => d.GetDirectories(),
                    d => ((VFSDirectory)d).CountUsers > 0))
                {
                    return Response.Failed("Невозможно переместить директорию, т.к. она или ее поддиректории используется другими пользователями");
                }
                sourceDirectory.Parent.RemoveDirectory(sourceDirectory);
                destDirectory.AddDirectory(sourceDirectory);
            }
            return Response.Success(true);
        }

        #endregion

        #region COPY Command

        /// <summary>
        /// Copy file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response Copy(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                return Response.Failed("1 аргумент - путь к директории или файлу, 2 - путь к директории");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (destDirectory == null)
            {
                return Response.Failed("Не найдена директория, куда нужно скопировать объект");
            }
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
                    return Response.Failed("Нельзя скопировать корневую директорию");
                }
                IDirectory copiedDirectory = (IDirectory)sourceDirectory.DeepCopy();
                destDirectory.AddDirectory(copiedDirectory);
            }
            return Response.Success(true);
        }

        #endregion

        #region PRINT Command

        /// <summary>
        /// Print data in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        public Response Print(CommandContext context)
        {
            return Response.Success(representAsString(context.User.CurrentDirectory.Root, 0), false);
        }

        /// <summary>
        /// Represent directory in string format
        /// </summary>
        /// <param name="rootDirectory">directory to represent in string format</param>
        /// <param name="deepLevel">Deep level of the directory. Use for recursive invoke.</param>
        /// <returns>String represent of a directory</returns>
        /// 
        /// <remarks>
        /// Use recursive invoke to represent as string all child directories
        /// </remarks>
        private string representAsString(IDirectory rootDirectory, int deepLevel)
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
                returnValue.Append(representAsString(directory, deepLevel + 1));
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

        /// <summary>
        /// Find parent directory for current directory in user context <see cref="UserContext"/>
        /// </summary>
        /// <param name="context">user context</param>
        /// <returns><c>null</c> - if parent directory not found</returns>
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

        /// <summary>
        /// Return path on one directory above
        /// </summary>
        /// <param name="path">curret path</param>
        public string UpPath(string path)
        {
            path = RemoveOddSymbols(path);
            if (path.LastIndexOf(SEPARATOR) == -1)
            {
                return path;
            }
            return path.Remove(path.LastIndexOf(SEPARATOR));
        }

        /// <summary>
        /// Find file in current directory
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="currentDirectory">current directory</param>
        /// <returns><c>null</c> - if file not found</returns>
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

        /// <summary>
        /// Find directory in current directory
        /// </summary>
        /// <param name="filePath">directory path</param>
        /// <param name="currentDirectory">current directory</param>
        /// <returns><c>null</c> - if directory not found</returns>
        public IDirectory FindDirectory(string directoryPath, IDirectory currentDirectory)
        {
            if (IsAbsolutePath(directoryPath))
            {
                if (RemoveOddSymbols(directoryPath).EqualByOrdinalIgnoreCase(currentDirectory.Root.Name))
                {
                    return currentDirectory.Root;
                }
                return findDirectoryInDirectory(removeRootPath(directoryPath), currentDirectory.Root);
            }
            else
            {
                return findDirectoryInDirectory(directoryPath, currentDirectory);
            }
        }

        /// <summary>
        /// Check that path <paramref name="path"/> is absolute
        /// </summary>
        /// <param name="path">path to check</param>
        /// <returns><c>true</c> - path is absolute</returns>
        public bool IsAbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }

        /// <summary>
        /// Remove root path from path <paramref name="path"/>
        /// </summary>
        /// <param name="path">Path to perform</param>
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
            return path = path.Trim(' ', SEPARATOR);
        }

        /// <summary>
        /// Split path to array of the folder names
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Array of folder and file names</returns>
        private string[] splitPath(string path)
        {
            return path.Split(SEPARATOR);
        }

        /// <summary>
        /// Check, that args contain one argument
        /// </summary>
        /// <param name="args">arguments</param>
        /// <returns><c>true</c> - one argument contain</returns>
        protected static bool hasOneParameter(string[] args)
        {
            return args != null && args.Length > 0;
        }

        /// <summary>
        /// Check, that args contain two argument
        /// </summary>
        /// <param name="args">arguments</param>
        /// <returns><c>true</c> - two argument contain</returns>
        protected static bool hasTwoParameters(string[] args)
        {
            return args != null && args.Length > 1;
        }

        #endregion
    }
}
