using TBENesys_Orderflow_Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TBENesys_Orderflow_Core.Models
{
    public class ResponseMessage
    {
        public bool? IsSuccessful { set; get; }
        public string User { set; get; }
        public DateTime MessageDateTime { set; get; }
        public string MessageType { set; get; }
        public string Message { set; get; }
        public string Details { set; get; }
        public string AddAnotherLink { set; get; }
    }

    public class SessionTest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public SessionTest(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void setSession(string UserID)
        {
            _session.SetString("UserID", UserID);
        }

        public string getSession()
        {
            return _session.GetString("UserID");
        }

        public void setSessionRoles(string UserRoles)
        {
            _session.SetString("UserRoles", UserRoles);
        }

        public string getSessionRoles()
        {
            return _session.GetString("UserRoles");
        }

        public void setSessionTerms(string TermsStatus)
        {
            _session.SetString("TermsAccepted", TermsStatus);
        }

        public string getSessionTerms()
        {
            return _session.GetString("TermsAccepted");
        }

        public void setSessionOrderTerms(string TermsStatus)
        {
            _session.SetString("TermsAccepted", TermsStatus);
        }

        public string getSessionOrderTerms()
        {
            return _session.GetString("TermsAccepted");
        }

        public UserOrderDetails GetComplexData(string key)
        {
            var data = _session.GetString(key);
            if (data == null)
            {
                return default(UserOrderDetails);
            }
            return JsonConvert.DeserializeObject<UserOrderDetails>(data);
        }

        public  void SetComplexData(string key, object value)
        {
            _session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public void SetEmailData(string key, object value)
        {
            _session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public string GetEmailData(string key)
        {
            var data = _session.GetString(key);
            if (data == null)
            {
                return default(string);
            }
            return JsonConvert.DeserializeObject<string>(data);
        }
    }
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }

}
