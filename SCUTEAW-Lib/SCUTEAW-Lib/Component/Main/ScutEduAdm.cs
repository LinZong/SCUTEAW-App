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

        private LoginEaw loginEaw { get; set; }

        public ScutEduAdm() : this(new EawRequest())
        {

        }
        public ScutEduAdm(EawRequest request)
        {
            Request = request;
            loginEaw = new LoginEaw(Request);
        }
        /// <summary>
        /// 执行登陆教务请求
        /// </summary>
        /// <param name="type">登陆模式, UseStudentIdAndPassword 和 UseCookie</param>
        /// <param name="args">登陆需要的参数 如果使用StudentIdAndPassword, 依次传入StudentId, Password。如果使用UseCookie, 依次传入StudentId, JSESSION, JwxtToken</param>
        /// <returns></returns>
        public bool LoginScutEduAdm(LoginType type, params string[] args)
        {
            switch (type)
            {
                case LoginType.UseStudentIdAndPassword:
                    return loginEaw.LoginWithAccount(args[0], args[1]);
                case LoginType.UseCookie:
                    return loginEaw.LoginWithCookie(args[0], args[1], args[2]);
                default:
                    return false;
            }
        }
        public void ShowPersonalInfo()
        {
            if (CheckLoginStatus())
            {
                var rawInfo = Request.GetPersonalInfo(loginEaw.UserAccount.StudentId);
                (string major, string name) = ContentExtractor.ExtractPersonalInfo(rawInfo);
                Console.WriteLine($"{major} -- {name}");
            }
        }
        public void ShowRecentScore()
        {
            if (CheckLoginStatus())
            {
                Console.WriteLine("Let's query your RECENT Score! ->_->");
                var rawInfo = Request.GetRecentScoreInfo(loginEaw.UserAccount.StudentId);
                var ScoreList = ContentExtractor.ExtractScore(rawInfo);
                foreach (var i in ScoreList)
                {
                    Console.WriteLine($"  {i.Key}   {i.Value}  ");
                }
            }
        }
        public void LogoutScutEduAdm() => loginEaw.LogoutAccount();
        private bool CheckLoginStatus()
        {
            return loginEaw.IsLoginIn;
        }
    }
}
