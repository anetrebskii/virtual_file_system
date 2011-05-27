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
        Response CreateDirectory(CommandContext context);

        /// <summary>
        /// Remove directory from file system
        /// </summary>   
        /// <param name="context">context for command</param>
        Response RemoveDirectory(CommandContext context);

        /// <summary>
        /// Remove directory with child directories
        /// </summary>
        /// <param name="context">context for command</param>
        Response RemoveTree(CommandContext context);

        /// <summary>
        /// Navigate to another directory
        /// </summary>
        /// <param name="context">context for command</param>
        Response Navigate(CommandContext context);

        /// <summary>
        /// Create new file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response CreateFile(CommandContext context);

        /// <summary>
        /// Remove file from file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response RemoveFile(CommandContext context);

        /// <summary>
        /// Lock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response LockFile(CommandContext context);

        /// <summary>
        /// Unlock file in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response UnlockFile(CommandContext context);

        /// <summary>
        /// Move file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response Move(CommandContext context);

        /// <summary>
        /// Copy file or directory to other directory in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response Copy(CommandContext context);

        /// <summary>
        /// Print data in file system
        /// </summary>
        /// <param name="context">context for command</param>
        Response Print(CommandContext context);
    }
}
