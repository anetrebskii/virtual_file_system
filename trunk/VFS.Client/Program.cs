using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VFS.Common;
using System.Security.Authentication;

namespace VFS.Client
{
    class Program
    {
        /// <summary>
        /// End point name to connect to a server
        /// </summary>
        static readonly string _endpointName = "RemoteConsole";

        /// <summary>
        /// Channel factory for work with server
        /// </summary>
        static DuplexChannelFactory<IRemoteConsole> _factory;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            InstanceContext context = new InstanceContext(new ConsoleCallback());
            _factory = new DuplexChannelFactory<IRemoteConsole>(context, _endpointName);
            while (true)
            {
                IRemoteConsole remoteConsole = connectToServer();
                if (remoteConsole == null)
                {
                    return;
                }
                DoWork(remoteConsole);
            }
        }

        /// <summary>
        /// Handle unhandled exceptions
        /// </summary>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO: Log
        }

        /// <summary>
        /// Provide work with remote console <paramref name="remoteConsole"/>
        /// </summary>
        /// <param name="remoteConsole">Remote console</param>
        private static void DoWork(IRemoteConsole remoteConsole)
        {
            while (true)
            {
                Console.Write("{0}> ", remoteConsole.GetUserPosition());
                string command = Console.ReadLine();
                if (command.ToUpper().Equals("QUITE"))
                {
                    remoteConsole.Quite();
                    break;
                }
                string serverResponse = String.Empty;
                try
                {
                    serverResponse = remoteConsole.SendCommand(command);
                }
                catch (CommunicationObjectFaultedException ex)
                {
                    Console.WriteLine("Соединение с сервером потеряно");
                    break;
                }
                catch (ProtocolException ex)
                {
                    Console.WriteLine("Соединение с сервером потеряно");
                    break;
                }
                if (serverResponse == null || serverResponse.Trim() == String.Empty)
                {
                    continue;
                }
                Console.WriteLine(serverResponse);
            }
        }

        /// <summary>
        /// Provide create connection to server
        /// </summary>
        /// <returns>Remote console to work with server</returns>
        private static IRemoteConsole connectToServer()
        {
            IRemoteConsole returnValue = null;
            while (returnValue == null)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                ConnectionData connectionData = null;
                try
                {
                    connectionData = ConnectionData.Parse(command);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Пожалуйста введите 'connect <server address/server name> <user name>'");
                    continue;
                }
                returnValue = tryConnectToServer(connectionData);
            }
            return returnValue;
        }

        /// <summary>
        /// try to connect to the server by connection information <paramref name="connectionData"/>
        /// </summary>
        /// <param name="connectionData">data to connect to a server</param>
        /// <returns>Remote console from server. <c>null</c> - if connection was fail</returns>
        private static IRemoteConsole tryConnectToServer(ConnectionData connectionData)
        {
            IRemoteConsole remoteConsole = _factory.CreateChannel(connectionData.Endpoint);
            try
            {
                AuthenticationResult authenticateSuccess = remoteConsole.Authenticate(connectionData.UserName);
                if (authenticateSuccess.IsSuccess)
                {
                    Console.WriteLine("Количесто уже подключенных пользователей: {0}", authenticateSuccess.CountAuthenticatedUsers);
                    return remoteConsole;
                }
                else
                {
                    Console.WriteLine("Ошибка при аутентификации");
                }
            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine("Сервер с данным адресом не был найден");
            }
            return null;
        }
    }
}
