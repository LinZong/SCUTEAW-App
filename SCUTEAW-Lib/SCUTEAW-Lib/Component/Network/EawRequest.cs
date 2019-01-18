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
        public bool LoginToEduAdm(List<KeyValuePair<string, string>> Params,out HttpStatusCode status)
        {
            var LoginRequest = new RestRequest(requestUrl.EAWLoginUrl, Method.POST);
            LoginRequest.AddDecompressionMethod(DecompressionMethods.GZip);
            LoginRequest.AddAllParameters(Params);
            var response = client.Execute(LoginRequest);
            status = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var location = response.Headers.FirstOrDefault(x => x.Name == "Location" && x.Value.ToString() == requestUrl.StudentHomepageUrl);
                if (location != null)
                {
                     return true;
                }
            }
            return false;
        }

        public string GetPersonalInfo(string StuId)
        {
            var PersonalInfoReq = new RestRequest(string.Format(requestUrl.GetPersonalInfoUrl, LoginHelper.DateTimeNowUnix(), StuId),Method.GET);
            var Resp = client.Execute(PersonalInfoReq).Content;
            return Resp;
        }
        public string GetRecentScoreInfo(string StuId)
        {
            var PersonalInfoReq = new RestRequest(string.Format(requestUrl.GetRecentScoreUrl, StuId), Method.GET);
            var Resp = client.Execute(PersonalInfoReq).Content;
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
