using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Contract;
using System.ServiceModel;
using VFS.Server.Core.Commands;

namespace VFS.Server.Console.Protocol
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class RemoteConsole : IRemoteConsole
    {
        private static List<ConnectedUser> _chatUsers = new List<ConnectedUser>();
        private ConnectedUser _user;  

        #region IRemoteConsole Members

        public bool Authentificate(string userName)
        {
            IConsoleCallback callback = OperationContext.Current.GetCallbackChannel<IConsoleCallback>();
            //Массив всех участников чата, который мы вернем клиенту  
            //string[] tmpUsers = new string[_chatUsers.Count];
            //for (int i = 0; i < _chatUsers.Count; i++)
            //{
            //    tmpUsers[i] = _chatUsers[i].Name;
            //}
            //Оповещаем всех клиентов что в чат вощел новый пользователь  
            //foreach (UserContext user in _chatUsers)
            //{
            //    user.Callback.UserEnter(name);
            //}
            //Создаем новый экземплар пользователя и заполняем все его поля  
            ConnectedUser chatUser = new ConnectedUser() { Callback = callback };
            _chatUsers.Add(chatUser);
            _user = chatUser;
            System.Console.WriteLine(">>User Enter: {0}", userName);
            return true;
        }

        public void Quite()
        {
            _chatUsers.Remove(_user);
            //Оповещаем всех клиентов о том что пользователь нас покинул  
            //foreach (ChatUser item in _chatUsers)
            //{
            //    item.Callback.UserLeave(_user.Name);
            //}
            _user = null;
            
            //Закрываем канал связи с текущим пользователем  
            //OperationContext.Current.Channel.Abort();
        }

        public string SendCommand(string command)
        {
            //var usersSending = from u in _chatUsers
            //                   where u.Name != _user.Name
            //                   select u;
            //foreach (ConnectedUser item in usersSending)
            //{
            //    item.Callback.Receive(_user.Name, msg);
            //}
            return "OK";
        }

        #endregion
    }
}
