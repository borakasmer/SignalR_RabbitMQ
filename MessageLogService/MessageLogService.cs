using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogService
{
    public partial class MessageLogService : ServiceBase
    {
        public MessageLogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var service = new MessageListener();
            service.DoWork();
        }

        protected override void OnStop()
        {
        }
    }
}
