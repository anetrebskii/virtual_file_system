using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VFS.Contract;

namespace VFS.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new ConsoleCallback());  
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            DuplexChannelFactory<IRemoteConsole> factory = new DuplexChannelFactory<IRemoteConsole>(context, binding);  
            Uri adress = new Uri("net.tcp://localhost:20010/ChatService");
            EndpointAddress endpoint = new EndpointAddress(adress.ToString());
            //Связь с сервером не устанавливается до тех пор, пока не будет вызван метод Authentificate  
            IRemoteConsole chat = factory.CreateChannel(endpoint);  
            Console.Write("Enter you name: ");
            string name = Console.ReadLine();
            Console.WriteLine(chat.Authentificate(name));  

            chat.Quite();
            Console.ReadLine();
        }
    }
}
