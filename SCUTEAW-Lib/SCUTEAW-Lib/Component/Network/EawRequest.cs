using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using SCUTEAW_Lib.Component.Helper;
using SCUTEAW_Lib.Model.Login;
using RestSharp;
using SCUTEAW_Lib.Component.Network;
using SCUTEAW_Lib.Component.Extractor;
using System.Linq;
using Newtonsoft.Json;
using System;
using SCUTEAW_Lib.Component.Login;

namespace SCUTEAW_Lib.Component.Network
{

    public class EawRequest : Requester
    {
        public EawRequest() : base()
        {
        }
        public EawRequest(WebProxy proxy) : base(proxy)
        {

        }
        public (string mod, string expo, string csrf) GetLoginInfo(string studentId, string password)
        {
            var FullPageRequest = new RestRequest(requestUrl.EAWLoginUrl, Method.GET);
            FullPageRequest.AddDecompressionMethod(DecompressionMethods.GZip);
            var FullPageResponse = client.Execute(FullPageRequest);
            try
            {
                var FullPage = FullPageResponse.Content;
                if (string.IsNullOrEmpty(FullPage)) throw new FormatException("Login page is empty.");
                var CsrfToken = ContentExtractor.ExtractCsrfToken(FullPage);
                var CombineUrl = requestUrl.GetPublicKeyUrl + LoginHelper.DateTimeNowUnix().ToString();
                var LoginRsaPubKeyRequest = new RestRequest(CombineUrl, Method.GET);
                LoginRsaPubKeyRequest.AddDecompressionMethod(DecompressionMethods.GZip);
                var PublicKeyContent = client.Execute(LoginRsaPubKeyRequest).Content;
                var PublicKeyObject = JObject.Parse(PublicKeyContent);
                var Modulus = PublicKeyObject.GetValue("modulus").ToString();
                var Exponent = PublicKeyObject.GetValue("exponent").ToString();
                return (Modulus, Exponent, CsrfToken);
            }
            catch (JsonReaderException)
            {
                throw new FormatException("Cannot extract encrypt RSA public key.");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        public IRestResponse LoginToEduAdm(List<KeyValuePair<string, string>> Params)
        {
            var LoginRequest = new RestRequest(requestUrl.EAWLoginUrl, Method.POST);
            LoginRequest.AddDecompressionMethod(DecompressionMethods.GZip);
            LoginRequest.AddAllParameters(Params);
            var response = client.Execute(LoginRequest);

            return response;
        }
        public string GetQueryableCourseTermYear(string StuId)
        {
            var resp = ExecuteGetRequest(requestUrl.GetCourseSchedulePageUrl, StuId);
            return resp.Content;
        }
        public string GetCourseScheduleJson(string Year,string Term)
        {
            string ReqTerm = ScutEduAdm.TransformTermIndices(Term);

            var req = new RestRequest(requestUrl.GetCourseScheduleJsonUrl, Method.POST);
            req.AddParameter(new Parameter("xnm", Year,ParameterType.QueryString));
            req.AddParameter(new Parameter("xqm", ReqTerm, ParameterType.QueryString));
            var res = client.Execute(req);
            return res.Content;
        }

        public string GetScoreListJson(string Year, string Term)
        {
            string ReqTerm = ScutEduAdm.TransformTermIndices(Term);

            var req = new RestRequest(requestUrl.GetScoreJsonUrl, Method.POST);
            req.AddParameter(new Parameter("xnm", Year, ParameterType.QueryString));
            req.AddParameter(new Parameter("xqm", ReqTerm, ParameterType.QueryString));
            //add some required request parameter.
            req.AddParameter(new Parameter("_search",false, ParameterType.QueryString));
            req.AddParameter(new Parameter("nd", LoginHelper.DateTimeNowUnix().ToString(), ParameterType.QueryString));
            req.AddParameter(new Parameter("queryModel.showCount", 999, ParameterType.QueryString));
            req.AddParameter(new Parameter("queryModel.currentPage", "1", ParameterType.QueryString));
            req.AddParameter(new Parameter("queryModel.sortOrder", "asc", ParameterType.QueryString));
            req.AddParameter(new Parameter("time", "1", ParameterType.QueryString));

            var res = client.Execute(req);
            return res.Content;
        }
        public string GetPersonalInfo(string StuId)
        {
            var resp = ExecuteGetRequest(requestUrl.GetPersonalInfoUrl, LoginHelper.DateTimeNowUnix().ToString(),StuId);
            return resp.Content;
        }
        public string GetRecentScoreInfo(string StuId)
        {
            var resp = ExecuteGetRequest(requestUrl.GetRecentScoreUrl, StuId);
            return resp.Content;
        }
        public string GetRecentCourseInfo(string StuId)
        {
            var resp = ExecuteGetRequest(requestUrl.GetRecentCourseUrl, StuId);
            return resp.Content;
        }
        public IRestResponse ExecuteGetRequest(string Url,params string[] paramArgs)
        {
            var PersonalInfoReq = new RestRequest(string.Format(Url, paramArgs), Method.GET);
            var Resp = client.Execute(PersonalInfoReq);
            return Resp;
        }
        public void LogoutEduAdm()
        {
            var LogoutRequest = new RestRequest(string.Format(requestUrl.LogoutUrl, LoginHelper.DateTimeNowUnix()), Method.GET);
            client.Execute(LogoutRequest);
        }
        public void SetLoginCookies(SessionIdentifier session)
        {
            client.AddAllCookies(session.ToCookies());
        }
        public bool ValidateSessionCookie(string Jsession, string JwxtToken)
        {
            var session = new SessionIdentifier() { JSESSIONID = Jsession, BIGServerJwxtToken = JwxtToken };
            var CookieVerifyReq = new RestRequest(requestUrl.StudentHomepageUrl, Method.GET);
            CookieVerifyReq.AddDecompressionMethod(DecompressionMethods.GZip);
            var testClient = ClientFactory();
            testClient.Proxy = UsedProxy;
            testClient.AddAllCookies(session.ToCookies());
            var cont = testClient.Execute(CookieVerifyReq);

            if (cont.StatusCode == HttpStatusCode.OK)
            {
                //把这个校验通过的Cookie给到主client。
                client.AddAllCookies(session.ToCookies());
                return true;
            }
            return false;
        }
    }
}
