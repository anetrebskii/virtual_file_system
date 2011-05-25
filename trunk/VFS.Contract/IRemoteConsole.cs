using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Contract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IConsoleCallback))]  
    public interface IRemoteConsole
    {
        [OperationContract(IsInitiating = true, IsOneWay = false, IsTerminating = false)]  
        bool Authentificate(string userName);

        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = true)] 
        void Quite();

        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = false)]  
        string SendCommand(string command);
    }
}
