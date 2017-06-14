using Microsoft.AspNet.SignalR;
using SignalR_RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SignalR_RabbitMQ.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        static private Dictionary<string, string> ChatList = new Dictionary<string, string>();
        [HttpPost]
        public ActionResult LogIn(string name)
        {
            //Bu Nick daha önceden alındı mı?
            if (!ChatList.Any(cl => cl.Value.ToUpper() == name.ToUpper()) && name.Trim() != "")
            {
                ChatList.Add(DateTime.Now.ToString(), name);
                Session.Add("UserName", name);
                return View();
            }
            else
            {
                return Content("Error");
            }
        }
    }
    public class Chat : Hub
    {
        public async override Task OnConnected()
        {
            await Clients.Caller.connected(Context.ConnectionId);
        }

        public async Task SendMessageToAll(string Message, string UserName)
        {
            //İlgili Mesaj Loglanır.
            RabbitMQPublisher.SendMessage(new MessageLog() { Nick = UserName, Text = Message, CreatedDate = DateTime.Now });
            await Clients.All.SendMessageAll(Message, UserName);
        }
    }
}