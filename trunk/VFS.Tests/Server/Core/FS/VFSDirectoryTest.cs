using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core.FS.Impl;
using VFS.Server.Core.FS;
using VFS.Server.Core.Exceptions;

namespace VFS.Tests.Server.Core.FS
{
    /// <summary>
    /// Summary description for VFSDirectoryTest
    /// </summary>
    [TestClass]
    public class VFSDirectoryTest
    {
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

        [TestMethod]
        public void NewDirectory_ShouldBeRoot()
        {
            VFSDirectory newDirectory = new VFSDirectory();
            Assert.ReferenceEquals(newDirectory, newDirectory.Root);
        }

        [TestMethod]
        public void AddedChild_HasParentAndRootAsParent()
        {
            VFSDirectory parent = new VFSDirectory();
            VFSDirectory child = new VFSDirectory();

            parent.AddDirectory(child);

            Assert.ReferenceEquals(parent.Root, child.Root);
            Assert.ReferenceEquals(parent, child.Parent);
        }

        [TestMethod]
        [ExpectedException(typeof(FSException))]
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
