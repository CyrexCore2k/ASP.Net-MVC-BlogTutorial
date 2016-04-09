using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities = BlogTutorial.Utilities;

namespace BlogTutorial.WebService.Controllers
{
    public class BaseController : Controller
    {
        public const string DEFAULT_JSONP_CALLBACK_PARAMETER = "callback";

        public virtual string JsonpCallback { get { return Request.QueryString[DEFAULT_JSONP_CALLBACK_PARAMETER] ?? string.Empty; } }

        private JsonResult Message(Utilities.JSONMessageObject message)
        {
            if (JsonpCallback != string.Empty)
                return new Utilities.Services.JsonpResult(JsonpCallback) { Data = message, ContentType = "application/javascript", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            else
                return new JsonResult() { Data = message, ContentType = "application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        protected JsonResult Message(bool success, bool loggedIn, bool isAdmin, object data)
        {
            return Message(new Utilities.JSONMessageObject()
            {
                Success = success,
                LoggedIn = loggedIn,
                IsAdmin = isAdmin,
                Data = data
            });
        }
    }
}
