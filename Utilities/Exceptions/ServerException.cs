using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogTutorial.Utilities.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string message)
            : base(message)
        {
        }
    }
}