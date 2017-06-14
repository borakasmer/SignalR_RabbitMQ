
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SignalR_RabbitMQ.Models
{
    [AttributeUsageAttribute(AttributeTargets.All, AllowMultiple = true)]
    public class LoginFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult("LogIn", new RouteValueDictionary("LogIn"));
            }
        }
    }
}