using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogTutorial.UI.Controllers
{
    public class MethodController : BaseController
    {
        public class FakeData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public JsonResult GetSpoofedInfo()
        {
            var Names = new List<FakeData>
            {
                new FakeData() {FirstName = "Spencer", LastName = "Ruport"},
                new FakeData() {FirstName = "John", LastName = "Smith"}
            };

            return Message(true, false, false, new
            {
                Names = from d in Names
                        select new
                        {
                            Name = d.FirstName + " " + d.LastName
                        }
            });
        }
    }
}
