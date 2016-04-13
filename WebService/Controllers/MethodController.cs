using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogTutorial.WebService.Controllers
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

        public JsonResult DemonstrateClientError()
        {
            throw new Utilities.Exceptions.ClientException("None", "Nothing", "This is just a demonstration of a client exception.", Guid.Parse("0EEDFAD3-C8C3-4CFD-BF4C-E64B9F3136F3"));
        }

        public JsonResult DemonstrateServerError()
        {
            throw new Exception("This is just a demonstration of a server exception.");
        }

    }
}
