using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogTutorial.UI.Classes.Exceptions
{
    public class ClientException : Exception
    {
        public string Number { get; protected set; }
        public string Value { get; set; }
        public bool LoggedIn { get; set; }
        public bool IsAdmin { get; set; }

        public ClientException(string source, string value, string message, Guid number)
            : base(message)
        {
            this.Source = source;
            this.Number = number.ToString();
            this.Value = value;
        }
        public ClientException(string source, int value, string message, Guid number)
            : this(source, value.ToString(), message, number)
        {
        }
    }
}