using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.Exceptions;

namespace VFS.Server.Core
{
    sealed class ServerConsole
    { 
        private IFSEngine _engine;
        private Dictionary<string, Action<CommandContext>> _commands = new Dictionary<string, Action<CommandContext>>();

        public ServerConsole()
            : this(new VFSEngine())
        {
            
        }

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

        public string InputCommand(string textCommand, UserContext user)
        {
            string[] commandItems = textCommand.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string commandName = commandItems[0].ToUpper();

            CommandContext commandContext = new CommandContext(user, null)
            {
                Args = commandItems.Skip(1).ToArray()
            };
            try
            {
                _commands[commandName].Invoke(commandContext);
                return commandContext.Response;
            }
            catch (FSException ex)
            {
                return ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                return "Указанная команда не найдена";
            }
        }
    }
}
