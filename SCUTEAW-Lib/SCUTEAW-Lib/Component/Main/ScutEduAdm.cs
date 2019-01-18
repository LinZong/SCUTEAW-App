using System;
using System.Collections.Generic;
using System.Threading;
using SCUTEAW_Lib.Component.Extractor;
using SCUTEAW_Lib.Component.Helper;
using SCUTEAW_Lib.Component.Network;
using SCUTEAW_Lib.Model.Login;
namespace SCUTEAW_Lib.Component.Login
{
    public class ScutEduAdm
    {

        private EawRequest Request { get; set; }

        public Account account { get; private set; }

        public ScutEduAdm() : this(new EawRequest())
        {

        }
        public ScutEduAdm(EawRequest request)
        {
            Request = request;
            account = new Account(Request);
        }
        /// <summary>
        /// 执行登陆教务请求
        /// </summary>
        /// <param name="type">登陆模式, UseStudentIdAndPassword 和 UseCookie</param>
        /// <param name="args">登陆需要的参数 如果使用StudentIdAndPassword, 依次传入StudentId, Password。如果使用UseCookie, 依次传入StudentId, JSESSION, JwxtToken</param>
        /// <returns></returns>
        public bool LoginScutEduAdm(LoginType type,out string FailedResult, params string[] args)
        {
            switch (type)
            {
                case LoginType.UseStudentIdAndPassword:
                    return account.LoginWithPassword(args[0], args[1], out FailedResult);
                case LoginType.UseCookie:
                    FailedResult = "Cookie is invalid.";
                    return account.LoginWithCookie(args[0], args[1], args[2]);
                default:
                    FailedResult = "Unknown failure.";
                    return false;
            }
        }
        public (string major, string name) ShowPersonalInfo()
        {
            if (CheckLoginStatus())
            {
                var rawInfo = Request.GetPersonalInfo(account.UserAccount.StudentId);
                return ContentExtractor.ExtractPersonalInfo(rawInfo);
            }
            return (null,null);
        }
        public List<KeyValuePair<string,string>> ShowRecentScore()
        {
            if (CheckLoginStatus())
            {
                var rawInfo = Request.GetRecentScoreInfo(account.UserAccount.StudentId);
                var ScoreList = ContentExtractor.ExtractScore(rawInfo);
                return ScoreList;
            }
            return null;
        }
        public void LogoutScutEduAdm() => account.LogoutAccount();
        private bool CheckLoginStatus()
        {
            return account.IsLoginIn;
        }
    }
}
