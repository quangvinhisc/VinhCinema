using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using VinhCinema.Web.App_Start;

namespace VinhCinema.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(config);
            Bootstrapper.Run();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}