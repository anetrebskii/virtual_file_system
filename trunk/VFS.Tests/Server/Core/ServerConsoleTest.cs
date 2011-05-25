using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VFS.Server.Core;
using VFS.Server.Core.FS;
using Moq;
using VFS.Server.Core.Commands;

namespace VFS.Tests.Server.Core
{
    /// <summary>
    /// Summary description for ServerConsoleTest
    /// </summary>
    [TestClass]
    public class ServerConsoleTest
    {
        private ServerConsole _serverConsole;
        private Mock<IFSEngine> _engineMock;
        private UserContext _userContext;

        public ServerConsoleTest()
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

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            _engineMock = new Mock<IFSEngine>(MockBehavior.Loose);
            _userContext = CommandHelper.CreateCommandContext().User;
            _serverConsole = new ServerConsole(_engineMock.Object);
        }

        [TestMethod]
        public void Console_ParseCommand()
        {
            _serverConsole.HandleCommand("CD C:\\word", _userContext, CommandHelper._users);
            _engineMock.Verify(s => s.Navigate(It.Is<CommandContext>(c => c.Args.Length == 1 && c.Args[0] == "C:\\word")));
        }
    }
}
