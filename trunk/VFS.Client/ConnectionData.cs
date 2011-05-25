using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Client
{
    /// <summary>
    /// Represent data for connecting to server
    /// </summary>
    sealed class ConnectionData
    {
        /// <summary>
        /// Initialize instance of the class <see cref="ConnectionData"/>
        /// </summary>
        /// <param name="userName">User name for authentication on a server</param>
        /// <param name="endpointAddress">Endpoint adress of a server</param>
        private ConnectionData(string userName, EndpointAddress endpointAddress)
        {
            UserName = userName;
            Endpoint = endpointAddress;
        }

        /// <summary>
        /// User name for authentication on a server
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Endpoint adress of a server
        /// </summary>
        public EndpointAddress Endpoint { get; private set; }

        /// <summary>
        /// Parse string value
        /// </summary>
        /// <param name="value">value to parse</param>
        ///
        /// <exception cref="FormatException">
        /// if <paramref name="value"/> is hasn't format 'connect server_name[:port] username'
        /// </exception>
        public static ConnectionData Parse(string value)
        {            
            string[] values = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < 3
                || values[0].ToUpper() != "CONNECT")
            {
                throw new FormatException();
            }
            string endPointAddress = String.Format("net.tcp://{0}/RemoteConsole", values[1]);
            EndpointAddress endpoint = new EndpointAddress(endPointAddress);
            string userName = values[2];

            return new ConnectionData(userName, endpoint);
        }
    }
}
