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
    public class DELCommandTest
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
            _command = new DELCommand();
            _context = CommandHelper.CreateCommandContext();
        }

        #endregion

        [TestMethod]
        public void RemoveDir_RelativePath()
        {
            // Arrange            
            IFile child = CommandHelper.GetFSManager().CreateFile("child");
            _context.User.CurrentDirectory.AddFile(child);
            _context.Args = new string[] { child.Name };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.ReferenceEquals(0, _context.User.CurrentDirectory.GetFiles().Count());
        }

        [TestMethod]
        public void NavigateTo_AbsolutePath()
        {
            // Arrange            
            IFile child = CommandHelper.GetFSManager().CreateFile("child");
            _context.User.CurrentDirectory.AddFile(child);
            _context.Args = new string[] { @"c:\" + child.Name };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.ReferenceEquals(0, _context.User.CurrentDirectory.GetFiles().Count());
        }
    }
}
