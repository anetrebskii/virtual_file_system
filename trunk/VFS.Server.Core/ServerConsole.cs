using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.Contexts;

namespace VFS.Server.Core
{
    /// <summary>
    /// Represent server console for handle commands with file system
    /// </summary>
    public sealed class ServerConsole
    { 
        /// <summary>
        /// File system engine
        /// </summary>
        private readonly IFSEngine _engine;

        /// <summary>
        /// Available commands
        /// <para></para>
        /// key - command name
        /// <para></para>
        /// value - command handler
        /// </summary>
        private readonly Dictionary<string, Func<CommandContext, Response>> _commands 
            = new Dictionary<string, Func<CommandContext, Response>>();

        /// <summary>
        /// Initialize instance of a class <see cref="ServerConsole"/>
        /// </summary>
        public ServerConsole()
            : this(new VFSEngine())
        {
            
        }

        /// <summary>
        /// Initialize instance of a class <see cref="ServerConsole"/>
        /// </summary>
        /// <param name="engine">File system engine</param>
        public ServerConsole(IFSEngine engine)
        {
            _engine = engine;
            _commands.Add("CD", engine.Navigate);
            _commands.Add("MD", engine.CreateDirectory);
            _commands.Add("RD", engine.RemoveDirectory);
            _commands.Add("DELTREE", engine.RemoveTree);
            _commands.Add("MF", engine.CreateFile);
            _commands.Add("DEL", engine.RemoveFile);
            _commands.Add("LOCK", engine.LockFile);
            _commands.Add("UNLOCK", engine.UnlockFile);
            _commands.Add("MOVE", engine.Move);
            _commands.Add("COPY", engine.Copy);
            _commands.Add("PRINT", engine.Print);
        }

        /// <summary>
        /// Authenticate user by <paramref name="userName"/>
        /// </summary>
        /// <param name="userName">user name for authentication</param>
        /// <returns>User context of the autenticated user</returns>
        public UserContext Authenticate(string userName)
        {
            return new UserContext(userName)
            {
                CurrentDirectory = _engine.GetDefaultDirectory()
            };
        }

        /// <summary>
        /// Hanle command <paramref name="textCommand"/> from user <paramref name="user"/>
        /// </summary>
        /// <param name="textCommand">command in text format</param>
        /// <param name="user">command sender</param>
        /// <returns>Response after handle</returns>
        public Response HandleCommand(string textCommand, UserContext user)
        {
            if (textCommand == null || textCommand.Trim() == String.Empty)
            {
                return Response.Success(false);
            }

            string[] commandItems = textCommand.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string commandName = commandItems[0].ToUpper();

            CommandContext commandContext = new CommandContext(user)
            {
                Args = commandItems.Skip(1).ToArray()
            };
            try
            {
                return _commands[commandName].Invoke(commandContext);
            }           
            catch (KeyNotFoundException ex)
            {
                return Response.Failed("Указанная команда не найдена");
            }
            catch (Exception)
            {
                //TODO: Log
                throw;
            }
        }
    }
}
