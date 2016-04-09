using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogTutorial.UI.Classes
{
    public class JSONMessageObject
    {
        protected IList<Exceptions.ClientException> _ClientExceptions = null;
        protected IList<Exceptions.ServerException> _ServerExceptions = null;

        public bool Success { get; set; }
        public bool LoggedIn { get; set; }
        public bool IsAdmin { get; set; }
        public object Data { get; set; }
        public IList<Exceptions.ClientException> ClientExceptions
        {
            get
            {
                if (_ClientExceptions == null) _ClientExceptions = new List<Exceptions.ClientException>();
                return _ClientExceptions;
            }
            set
            {
                _ClientExceptions = value;
            }
        }
        public IList<Exceptions.ServerException> ServerExceptions
        {
            get
            {
                if (_ServerExceptions == null) _ServerExceptions = new List<Exceptions.ServerException>();
                return _ServerExceptions;
            }
            set
            {
                _ServerExceptions = value;
            }
        }

        public string toJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Success = Success,
                LoggedIn = LoggedIn,
                IsAdmin = IsAdmin,
                Data = Data,
                ClientExceptions = from ex in ClientExceptions
                                   select new
                                   {
                                       Source = ex.Source,
                                       Value = ex.Value,
                                       Message = ex.Message,
                                       Number = ex.Number
                                   },
                ServerExceptions = from ex in ServerExceptions
                                   select new
                                   {
                                       Message = ex.Message
                                   }
            });
        }
    }
}