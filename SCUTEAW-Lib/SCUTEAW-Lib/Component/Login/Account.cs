using SCUTEAW_Lib.Component.Helper;
using SCUTEAW_Lib.Component.Network;
using SCUTEAW_Lib.Model.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SCUTEAW_Lib.Component.Login
{
    public enum LoginType
    {
        UseStudentIdAndPassword,UseCookie
    }
    public class Account
    {
        public bool CookieMode { get; private set; }
        public bool IsLoginIn { get; private set; }
        public UserIdentifier UserAccount { get; private set; }

        private readonly EawRequest Request;
        public Account(EawRequest _req)
        {
            Request = _req;
        }
        public bool LoginWithPassword(string studentId, string password,out string FailedResult)
        {
            UserAccount = new UserIdentifier { StudentId = studentId, Password = password };
            CookieMode = false;
            try
            {
                (string mod, string expo, string csrf) = Request.GetLoginInfo(studentId, password);
                int Exit = 999;
                var EncryptPasswd = LoginHelper.EncryptPassword(mod, expo, password, out Exit);
                var param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("csrftoken", csrf));
                param.Add(new KeyValuePair<string, string>("yhm", studentId));
                param.Add(new KeyValuePair<string, string>("mm", EncryptPasswd));
                var response = Request.LoginToEduAdm(param);
                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    var location = response.Headers.FirstOrDefault(x => x.Name == "Location" && x.Value.ToString().Contains(Requester.requestUrl.StudentHomepageUrl));
                    if (location != null)
                    {
                        FailedResult = null;
                        IsLoginIn = true;
                        return true;
                    }
                    else FailedResult = "Username or password is wrong.";
                }
                else if (response.StatusCode == HttpStatusCode.OK) FailedResult = "Username or password is wrong.";
                else FailedResult = "Request time out.";
                IsLoginIn = false;
                return false;
            }
            catch (Exception e)
            {
                FailedResult = e.Message;
                IsLoginIn = false;
                return false;
            }
        }
        public bool LoginWithCookie(string studentId,string Jsession, string JwxtToken)
        {
            UserAccount = new UserIdentifier { StudentId = studentId, Password = null };
            var cookieStatus = Request.ValidateSessionCookie(Jsession, JwxtToken);
            if (cookieStatus)
            {
                CookieMode = true;
                IsLoginIn = true;
                return true;
            }
            IsLoginIn = false;
            CookieMode = false;
            return false;
        }
        public void LogoutAccount()
        {
            if (IsLoginIn && !CookieMode)
            {
                IsLoginIn = false;
                Request.LogoutEduAdm();
            }
        }
    }
}
