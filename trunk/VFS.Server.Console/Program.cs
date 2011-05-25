using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VFS.Contract;
using VFS.Server.Console.Protocol;

namespace VFS.Server.Console
{
    class Program
    {
        

        static void Main(string[] args)
        {
            //Создаем непосредственно сам Хост  
            ServiceHost host = new ServiceHost(typeof(RemoteConsole));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            Uri adress = new Uri("net.tcp://localhost:20010/ChatService");
            host.AddServiceEndpoint(typeof(IRemoteConsole), binding, adress.ToString());
            //Открываем порт и сервис ожидает клиентов  
            host.Open();
            System.Console.WriteLine("Service running...");
            System.Console.ReadKey();
            host.Close();  
        }
    }
}
