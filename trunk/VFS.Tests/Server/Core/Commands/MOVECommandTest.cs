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
    public class MOVECommandTest
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
            _command = new MOVECommand();
            _context = CommandHelper.CreateCommandContext();

            IDirectory child1 = CommandHelper.GetFSManager().CreateDirectory("child1");
            IDirectory child2 = CommandHelper.GetFSManager().CreateDirectory("child2");
            _context.User.CurrentDirectory.AddDirectory(child1);
            _context.User.CurrentDirectory.AddDirectory(child2);

            child2.AddDirectory(CommandHelper.GetFSManager().CreateDirectory("child21"));
        }

        #endregion

        [TestMethod]
        public void MoveDir()
        {
            // Arrange            
            _context.Args = new string[] { "child2", "child1" };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.AreEqual(1, _context.User.CurrentDirectory.GetDirectories().First().GetDirectories().Count());
            Assert.AreEqual(1, _context.User.CurrentDirectory.GetDirectories().Count());
        }

        [TestMethod]
        public void MoveFile()
        {
            // Arrange            
            IFile child = CommandHelper.GetFSManager().CreateFile("filename");
            _context.User.CurrentDirectory.AddFile(child);
            _context.Args = new string[] { child.Name, "child1" };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.AreEqual("filename", _context.User.CurrentDirectory.GetDirectories().First().GetFiles().First().Name);
            Assert.IsNull(_context.User.CurrentDirectory.GetFiles().FirstOrDefault(f => f.Name == "filename"));
        }
    }
}
