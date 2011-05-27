using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using VFS.Server.WinService.Protocol;

namespace VFS.Server.WinService
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost _host; 
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host = new ServiceHost(typeof(RemoteConsole));
            _host.Open();
        }

        protected override void OnStop()
        {
             _host.Close();
        }
    }
}
