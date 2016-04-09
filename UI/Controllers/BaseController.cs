using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogTutorial.UI.Controllers
{
    public class BaseController : Controller
    {
        protected JsonResult Message(bool success, bool loggedIn, bool isAdmin, object data)
        {
            return Message(new Classes.JSONMessageObject()
            {
                Success = success,
                LoggedIn = loggedIn,
                IsAdmin = isAdmin,
                Data = data
            });
        }

        private JsonResult Message(Classes.JSONMessageObject message)
        {
            return Json(message, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
