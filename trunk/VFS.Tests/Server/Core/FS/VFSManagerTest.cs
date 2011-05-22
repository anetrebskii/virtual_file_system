﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;

namespace VFS.Tests.Server.Core.FS
{
    /// <summary>
    /// Summary description for VFSDirectoryTest
    /// </summary>
    [TestClass]
    public class VFSManagerTest
    {
        private VFSManager _vfsManager;
        private IDirectory _rootDirectory;

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

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _vfsManager = new VFSManager();

            _rootDirectory = _vfsManager.CreateDirectory("c:");
            IDirectory child1 = _vfsManager.CreateDirectory("child1");
            IDirectory child2 = _vfsManager.CreateDirectory("child2");

            _rootDirectory.AddDirectory(child1);
            _rootDirectory.AddDirectory(child2);

            child1.AddDirectory(_vfsManager.CreateDirectory("child11"));
            child1.AddDirectory(_vfsManager.CreateDirectory("child12"));
            child2.AddDirectory(_vfsManager.CreateDirectory("child21"));
            child2.AddDirectory(_vfsManager.CreateDirectory("child22"));
        }

        [TestMethod]
        public void FindDirectory_AbsolutePath()
        {
            IDirectory actualResult =
                _vfsManager.FindDirectory(@"c:\child2\child21", _rootDirectory.GetDirectories().First());

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void FindDirectory_RelativePath()
        {
            IDirectory actualResult =
                _vfsManager.FindDirectory(@"child2\child21", _rootDirectory);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void Define_AbsolutePath()
        {
            Assert.IsTrue(_vfsManager.IsAbsolutePath(@"c:\child1\child11"));
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryAlreadyExistsException))]
        public void AddDirectory_WhichAlreadyExists()
        {
            VFSDirectory parent = new VFSDirectory();
            VFSDirectory child1 = new VFSDirectory() { Name = "child" };
            VFSDirectory child2 = new VFSDirectory() { Name = "child" };

            parent.AddDirectory(child1);
            parent.AddDirectory(child2);
        }
    }
}
