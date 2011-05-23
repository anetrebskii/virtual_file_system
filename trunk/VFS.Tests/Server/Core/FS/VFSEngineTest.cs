using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.Commands;
using FluentAssertions;
using System.IO;
using VFS.Server.Core.Exceptions;

namespace VFS.Tests.Server.Core.FS
{
    /// <summary>
    /// Summary description for VFSEngineTest
    /// </summary>
    [TestClass]
    public class VFSEngineTest
    {
        private IFSEngine _engine;
        private CommandContext _context;


        private IDirectory _rootDirectory;
        private IFile _file1;
        private IDirectory _child1Directory;
        private IDirectory _child2Directory;
        private IDirectory _child3Directory;

        public VFSEngineTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _engine = new VFSEngine();
            _context = CommandHelper.CreateCommandContext();
            _rootDirectory = _context.User.CurrentDirectory;

            _file1 = CommandHelper.GetFSManager().CreateFile("file1");
            _rootDirectory.AddFile(_file1);

            _child1Directory = CommandHelper.GetFSManager().CreateDirectory("child1");
            _child2Directory = CommandHelper.GetFSManager().CreateDirectory("child2");
            _child3Directory = CommandHelper.GetFSManager().CreateDirectory("child3");
            _rootDirectory.AddDirectory(_child1Directory);
            _rootDirectory.AddDirectory(_child2Directory);
            _rootDirectory.AddDirectory(_child3Directory);

            _child1Directory.AddDirectory(CommandHelper.GetFSManager().CreateDirectory("child11"));
            _child1Directory.AddDirectory(CommandHelper.GetFSManager().CreateDirectory("child12"));
            _child2Directory.AddDirectory(CommandHelper.GetFSManager().CreateDirectory("child21"));
            _child2Directory.AddDirectory(CommandHelper.GetFSManager().CreateDirectory("child22"));
        }
        
        #endregion

        #region Commands

        #region MD Command

        [TestMethod]
        public void CreateFolder_WithRelativePath()
        {
            // Arrange
            string newFolderName = "newfolder";
            _context.Args = new string[] { newFolderName };

            // Act
            _engine.CreateDirectory(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories().Should().Contain(s => s.Name == newFolderName);
        }

        [TestMethod]
        public void CreateFolder_WithAbsolutePath()
        {
            // Arrange
            string newFolderPath = @"c:\child1\newfolder";
            _context.Args = new string[] { newFolderPath };

            // Act
            _engine.CreateDirectory(_context);

            // Assert
            _child1Directory.GetDirectories().Should().Contain(s => s.Name == "newfolder");
        }

        #endregion

        #region RD Command

        [TestMethod]
        [ExpectedException(typeof(FSException))]
        public void DontRemoveDir_WithChildDirectories()
        {
            // Arrange            
            _context.Args = new string[] { _child1Directory.Name };

            // Act
            _engine.RemoveDirectory(_context);
        }

        [TestMethod]
        public void RemoveDir_RelativePath()
        {
            // Arrange            
            _context.Args = new string[] { _child3Directory.Name };

            // Act
            _engine.RemoveDirectory(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories()
                .Should().NotContain(f => f.Name == _child3Directory.Name);
        }

        [TestMethod]
        public void RemoveDir_AbsolutePath()
        {
            // Arrange            
            _context.Args = new string[] { @"c:\" + _child3Directory.Name };

            // Act
            _engine.RemoveDirectory(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories()
                .Should().NotContain(f => f.Name == _child3Directory.Name);
        }

        #endregion

        #region DELTree Command

        [TestMethod]
        public void RemoveTree_WithChildDirectories()
        {
            // Arrange            
            _context.Args = new string[] { _child1Directory.Name };

            // Act
            _engine.RemoveTree(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories()
                .Should().NotContain(d => d.Name == _child1Directory.Name);
        }

        [TestMethod]
        public void RemoveTree_RelativePath()
        {
            // Arrange            
            _context.Args = new string[] { _child3Directory.Name };

            // Act
            _engine.RemoveTree(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories()
                .Should().NotContain(d => d.Name == _child3Directory.Name);
        }

        [TestMethod]
        public void RemoveTree_AbsolutePath()
        {
            // Arrange            
            _context.Args = new string[] { @"c:\" + _child3Directory.Name };

            // Act
            _engine.RemoveTree(_context);

            // Assert
            _context.User.CurrentDirectory.GetDirectories()
                .Should().NotContain(d => d.Name == _child3Directory.Name);
        }

        #endregion

        #region MF Command

        [TestMethod]
        public void CreateFile_WithRelativePath()
        {
            // Arrange
            string newFileName = "newFile";
            _context.Args = new string[] { newFileName };

            // Act
            _engine.CreateFile(_context);

            // Assert
            _context.User.CurrentDirectory.GetFiles()
                .Should().Contain(f => f.Name == newFileName);
        }

        [TestMethod]
        public void CreateFile_WithAbsolutePath()
        {
            // Arrange
            string newFilePath = @"c:\child1\newfile";
            _context.Args = new string[] { newFilePath };

            // Act
            _engine.CreateFile(_context);

            // Assert
            _child1Directory.GetFiles().Should().Contain(f => f.Name == "newfile");
        }

        #endregion
        
        #region DEL Command

        [TestMethod]
        public void RemoveFile_RelativePath()
        {
            // Arrange            
            _context.Args = new string[] { _file1.Name };

            // Act
            _engine.RemoveFile(_context);

            // Assert
            _rootDirectory.GetFiles().Should().NotContain(f => f.Name == _file1.Name);
        }

        [TestMethod]
        public void RemoveFile_AbsolutePath()
        {
            // Arrange            
            _context.Args = new string[] { @"c:\" + _file1.Name };

            // Act
            _engine.RemoveFile(_context);

            // Assert
            _rootDirectory.GetFiles().Should().NotContain(f => f.Name == _file1.Name);
        }

        #endregion
        
        #region LOCK Command

        [TestMethod]
        public void LockFile()
        {
            // Arrange            
            _context.Args = new string[] { _file1.Name };

            // Act
            _engine.LockFile(_context);

            // Assert
            _file1.LockedUsers.Should().Contain(u => u == _context.User.UserName);
        }

        [TestMethod]
        public void LockFile_OnlyOnce()
        {
            // Arrange            
            _file1.LockedUsers.Add(_context.User.UserName);
            _context.Args = new string[] { _file1.Name };

            // Act
            _engine.LockFile(_context);

            // Assert
            _file1.LockedUsers.Should().HaveCount(1);
        }

        #endregion
        
        #region UNLOCK Command

        [TestMethod]
        public void UnlockFile()
        {
            // Arrange            
            _file1.LockedUsers.Add(_context.User.UserName);
            _context.Args = new string[] { _file1.Name };

            // Act
            _engine.UnlockFile(_context);

            // Assert
            _file1.LockedUsers.Should().NotContain(u => u == _context.User.UserName);
        }

        [TestMethod]
        public void UnlockFile_NotThrowException_IfNotLocked()
        {
            // Arrange            
            _context.Args = new string[] { _file1.Name };

            // Act && Assert
            _engine.UnlockFile(_context);
        }

        #endregion
        
        #region CD Command

        [TestMethod]
        public void NavigateTo_RelativePath()
        {
            // Arrange            
            _context.Args = new string[] { _child1Directory.Name };

            // Act
            _engine.Navigate(_context);

            // Assert
            Assert.ReferenceEquals(_child1Directory, _context.User.CurrentDirectory);
        }

        [TestMethod]
        public void NavigateTo_AbsolutePath()
        {
            // Arrange            
            _context.Args = new string[] { @"c:\" + _child1Directory.Name };

            // Act
            _engine.Navigate(_context);

            // Assert
            Assert.ReferenceEquals(_child1Directory, _context.User.CurrentDirectory);
        }
        
        #endregion
        
        #region MOVE Command

        [TestMethod]
        public void MoveDir()
        {
            // Arrange            
            _context.Args = new string[] { _child1Directory.Name, _child2Directory.Name };

            // Act
            _engine.Move(_context);

            // Assert
            _child2Directory.GetDirectories().Should().Contain(d => d.Name == _child1Directory.Name);
            _rootDirectory.GetDirectories().Should().NotContain(d => d.Name == _child1Directory.Name);
        }

        [TestMethod]
        public void MoveFile()
        {
            // Arrange            
            _context.Args = new string[] { _file1.Name, _child1Directory.Name };

            // Act
            _engine.Move(_context);

            // Assert
            _child1Directory.GetFiles().Should().Contain(f => f.Name == _file1.Name);
            _rootDirectory.GetFiles().Should().NotContain(f => f.Name == _file1.Name);
        }

        #endregion
        
        #region COPY Command

        [TestMethod]
        public void CopyDir()
        {
            // Arrange            
            _context.Args = new string[] { _child1Directory.Name, _child2Directory.Name };

            // Act
            _engine.Copy(_context);

            // Assert
            _rootDirectory.GetDirectories().Should().Contain(d => d.Name == _child1Directory.Name);
            _child2Directory.GetDirectories().Should().Contain(d => d.Name == _child1Directory.Name);            
        }

        [TestMethod]
        public void CopyFile()
        {
            // Arrange            
            _context.Args = new string[] { _file1.Name, _child1Directory.Name };

            // Act
            _engine.Copy(_context);

            // Assert
            _rootDirectory.GetFiles().Should().Contain(f => f.Name == _file1.Name);
            _child1Directory.GetFiles().Should().Contain(f => f.Name == _file1.Name);   
        }

        #endregion
        
        #region PRINT Command

        [TestMethod]
        public void PrintFileSystem()
        {
            // Arrange
            IFile file11 = CommandHelper.GetFSManager().CreateFile("file11");
            file11.LockedUsers.AddRange(new string[] { "user1", "user2" });
            _child1Directory.AddFile(file11);
            _file1.LockedUsers.Add("user");
            
            // Act
            _engine.Print(_context);

            // Assert
            Assert.AreEqual(PrintResults.ExpectedResult, _context.Answer);
        }

        #endregion

        #endregion
    }
}
