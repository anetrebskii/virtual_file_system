using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core.Commands.Impl;
using VFS.Server.Core.Commands;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;

namespace VFS.Tests.Server.Core.Commands
{
    /// <summary>
    /// Summary description for MDCommandTest
    /// </summary>
    [TestClass]
    public class MDCommandTest
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
            _command = new MDCommand();
            _context = CommandHelper.CreateCommandContext();
        }

        #endregion

        [TestMethod]
        public void CreateFolder_WithRelativePath()
        {
            // Arrange
            string newFolderName = "newfolder";
            _context.Args = new string[] { newFolderName };

            // Act
            _command.Execute(_context);

            // Assert
            Assert.AreEqual(newFolderName, 
                _context.User.CurrentDirectory.GetDirectories().First().Name);
        }

        [TestMethod]
        public void CreateFolder_WithAbsolutePath()
        {
            // Arrange
            string newFolderName = @"c:\child\newfolder";
            IDirectory child = CommandHelper.GetFSManager().CreateDirectory("child");
            _context.User.CurrentDirectory.AddDirectory(child);

            _context.Args = new string[] { newFolderName };            

            // Act
            _command.Execute(_context);

            // Assert
            Assert.AreEqual("newfolder",
                child.GetDirectories().First().Name);
        }
    }
}
