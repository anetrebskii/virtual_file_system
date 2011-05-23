using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.Exceptions;

namespace VFS.Server.Core.FS
{
    /// <summary>
    /// Interface for file system engine
    /// </summary>
    interface IFSEngine
    {
        /// <summary>
        /// Create new directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void CreateDirectory(CommandContext context);

        /// <summary>
        /// Remove directory from file system
        /// </summary>   
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void RemoveDirectory(CommandContext context);

        /// <summary>
        /// Remove directory with child directories
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void RemoveTree(CommandContext context);

        /// <summary>
        /// Navigate to another directory
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void Navigate(CommandContext context);

        /// <summary>
        /// Create new file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void CreateFile(CommandContext context);

        /// <summary>
        /// Remove file from file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void RemoveFile(CommandContext context);

        /// <summary>
        /// Lock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void LockFile(CommandContext context);

        /// <summary>
        /// Unlock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void UnlockFile(CommandContext context);

        /// <summary>
        /// Move file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void Move(CommandContext context);

        /// <summary>
        /// Copy file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void Copy(CommandContext context);

        /// <summary>
        /// Print data in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        void Print(CommandContext context);
    }
}
