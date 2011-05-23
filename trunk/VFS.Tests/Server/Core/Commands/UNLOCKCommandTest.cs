using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core.Commands.Impl;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;
using System.IO;

namespace VFS.Tests.Server.Core.Commands
{
    /// <summary>
    /// Summary description for MDCommandTest
    /// </summary>
    [TestClass]
    public class UNLOCKCommandTest
    {
        private ICommand _command;
        private CommandContext _context;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
       
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _command = new UNLOCKCommand();
            _context = CommandHelper.CreateCommandContext();
        }

        #endregion

        [TestMethod]
        public void UnlockFile()
        {
            // Arrange            
            IFile child = CommandHelper.GetFSManager().CreateFile("child");
            child.LockedUsers.Add(_context.User.UserName);
            _context.User.CurrentDirectory.AddFile(child);
            _context.Args = new string[] { child.Name };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.IsFalse(child.LockedUsers.Contains(_context.User.UserName));
        }

        [TestMethod]
        public void UnlockFile_NotThrowException_IfNotLocked()
        {
            // Arrange            
            IFile child = CommandHelper.GetFSManager().CreateFile("child");
            _context.User.CurrentDirectory.AddFile(child);
            _context.Args = new string[] { child.Name };

            // Act && Assert
            _command.Execute(_context);
        }
    }
}
