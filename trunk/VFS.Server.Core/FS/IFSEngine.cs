using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.Contexts;

namespace VFS.Server.Core.FS
{
    /// <summary>
    /// Interface for file system engine
    /// </summary>
    public interface IFSEngine
    {
        /// <summary>
        /// Return default directory
        /// </summary>
        IDirectory GetDefaultDirectory();

        /// <summary>
        /// Create new directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response CreateDirectory(CommandContext context);

        /// <summary>
        /// Remove directory from file system
        /// </summary>   
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response RemoveDirectory(CommandContext context);

        /// <summary>
        /// Remove directory with child directories
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response RemoveTree(CommandContext context);

        /// <summary>
        /// Navigate to another directory
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response Navigate(CommandContext context);

        /// <summary>
        /// Create new file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response CreateFile(CommandContext context);

        /// <summary>
        /// Remove file from file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response RemoveFile(CommandContext context);

        /// <summary>
        /// Lock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response LockFile(CommandContext context);

        /// <summary>
        /// Unlock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response UnlockFile(CommandContext context);

        /// <summary>
        /// Move file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response Move(CommandContext context);

        /// <summary>
        /// Copy file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response Copy(CommandContext context);

        /// <summary>
        /// Print data in file system
        /// </summary>
        /// <param name="context">context for command</param>
        /// <exception cref="FSException"></exception>
        Response Print(CommandContext context);
    }
}
