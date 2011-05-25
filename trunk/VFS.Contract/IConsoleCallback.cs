using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Contract
{
    public interface IConsoleCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(string msg);
    }
}
