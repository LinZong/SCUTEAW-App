using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCUTEAW_Lib.Model.Login
{
    public class UserIdentifier
    {
        public string StudentId { get; set; }
        public string Password { get; set; }
    }
    public class SessionIdentifier
    {
        public string JSESSIONID { get; set; }
        public string BIGServerJwxtToken { get; set; }
        public override string ToString()
        {
            return $"{ConfigurationManager.AppSettings["JsessionId"]}={JSESSIONID};{ConfigurationManager.AppSettings["BIGServerJwxtToken"]}={BIGServerJwxtToken}";
        }
        public List<Cookie> ToCookies()
        {
            var list = new List<Cookie>();
            list.Add(new Cookie(ConfigurationManager.AppSettings["JsessionId"], JSESSIONID,"/", ".xsjw2018.scuteo.com"));
            list.Add(new Cookie(ConfigurationManager.AppSettings["BIGServerJwxtToken"], BIGServerJwxtToken, "/", ".xsjw2018.scuteo.com"));
            return list;
        }
    }
    public static class SessionIdentifierExtension
    {
        public static List<KeyValuePair<string, string>> ConvertToHeader(this SessionIdentifier session)
        {
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>(ConfigurationManager.AppSettings["JsessionId"],session.JSESSIONID));
            dict.Add(new KeyValuePair<string, string>(ConfigurationManager.AppSettings["BIGServerJwxtToken"],session.BIGServerJwxtToken));
            return dict;
        }
    }
}
