using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR_RabbitMQ.Models
{
    public partial class MessageLog
    {
        public int ID { get; set; }
        public string Nick { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}