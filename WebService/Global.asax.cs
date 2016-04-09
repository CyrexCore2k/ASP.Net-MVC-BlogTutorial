using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Utilities = BlogTutorial.Utilities;

namespace BlogTutorial.WebService
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

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();

            if (ex is Utilities.Exceptions.ClientException)
            {
                OutputMessage(new Utilities.JSONMessageObject() 
                {
                    ClientExceptions = { (Utilities.Exceptions.ClientException)ex }
                });
            }
            else 
            {
                WriteToFile(ex, (Exception ex1) =>
                {
                    OutputMessage(new Utilities.JSONMessageObject()
                    {
                        ServerExceptions = { new Utilities.Exceptions.ServerException("Failed to log error.") }
                    });
                });
            }

            Server.ClearError();
        }

        public void WriteToFile(Exception ex) { WriteToFile(ex, null); }
        public void WriteToFile(Exception ex, Action<Exception> failed)
        {
            string Path = Server.MapPath("/Content/logs/");
            Guid LogId = Guid.NewGuid();
            while(System.IO.File.Exists(Path + LogId.ToString() + ".log")) LogId = Guid.NewGuid();

            try
            {
                System.IO.File.AppendAllText(Path + LogId.ToString() + ".log", FormatException(ex, Request));
                OutputMessage(new Utilities.JSONMessageObject()
                {
                    ServerExceptions = { new Utilities.Exceptions.ServerException("Error saved to file log. Id: " + LogId.ToString()) }
                });
            }
            catch(Exception writeException)
            {
                if (failed != null) failed(ex);
            }
        }

        public static string FormatException(Exception ex, System.Web.HttpRequest request)
        {
            string InnerException = "null";
            if (ex.InnerException != null) InnerException = FormatException(ex.InnerException, request);

            return String.Format("\r\n\r\nApplication Error\r\n\r\n" +
                                                 "MESSAGE: {0}\r\n" +
                                                 "SOURCE: {1}\r\n" +
                                                 "FORM: {2}\r\n" +
                                                 "QUERYSTRING: {3}\r\n" +
                                                 "TARGETSITE: {4}\r\n" +
                                                 "STACKTRACE: {5}\r\n" +
                                                 "TIME: {6}\r\n" +
                                                 "INNER EXCEPTION BEGIN ***\r\n{7}\r\nINNER EXCEPTION END ***\r\n",
                                                 ex.Message,
                                                 ex.Source,
                                                 request.Form.ToString(),
                                                 request.QueryString.ToString(),
                                                 ex.TargetSite,
                                                 ex.StackTrace,
                                                 DateTime.Now.ToString(),
                                                 InnerException);
        }

        private void OutputMessage(Utilities.JSONMessageObject data)
        {
            const string JSONP_CALLBACK_PARAMETER = "callback";

            data.LoggedIn = false;
            data.IsAdmin = false;
            data.Success = false;

            Response.StatusCode = 200;

            if (Request.ContentType.ToLower().Contains("application/json"))
            {
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(data.toJSON());
            }
            else if (Request.ContentType.ToLower().Contains("application/jsonp"))
            {
                Response.ContentType = "application/javascript; charset=utf-8";
                Response.Write(Request.QueryString[JSONP_CALLBACK_PARAMETER].ToString() + "(" + data.toJSON() + ")");
            }
            else
            {
                Response.ContentType = "text/html; charset=utf-8";
                System.Text.StringBuilder MessageBuilder = new System.Text.StringBuilder();
                if (data.ServerExceptions.Count > 0)
                {
                    MessageBuilder.Append("<h2>Server Exceptions</h2><ul>");
                    foreach (var Exception in data.ServerExceptions)
                        MessageBuilder.Append("<li>" + Exception.Message + "</li>");
                }
                if (data.ClientExceptions.Count > 0)
                {
                    MessageBuilder.Append("<h2>Client Exceptions</h2><ul>");
                    foreach (var Exception in data.ClientExceptions)
                        MessageBuilder.Append("<li>" + Exception.Number + ": <b>" + Exception.Source + "</b> caused error: " + Exception.Message + ", value: " + Exception.Value + "</li>");
                }

                Response.Write(String.Format(System.IO.File.ReadAllText(Server.MapPath("/Content/error.htm")), MessageBuilder.ToString()));
            }
        }
    }
}