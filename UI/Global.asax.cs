using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogTutorial.UI
{
    /// <summary>
    /// Override to support Mono
    /// </summary>
    public class MonoWebFormViewEngine : WebFormViewEngine
    {
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            //throw new NotImplementedException("Got here.");
            return base.FileExists(controllerContext, virtualPath.Replace("~", ""));
        }
    }

    /// <summary>
    /// Override to support Mono
    /// </summary>
    public class MonoRazorViewEngine : RazorViewEngine
    {
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            //throw new NotImplementedException("Got here.");
            return base.FileExists(controllerContext, virtualPath.Replace("~", ""));
        }
    }

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Put Mono-supporting view engines in the context
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MonoWebFormViewEngine());
            ViewEngines.Engines.Add(new MonoRazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}