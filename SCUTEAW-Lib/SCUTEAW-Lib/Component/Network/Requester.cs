using SCUTEAW_Lib.Model.Network;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace SCUTEAW_Lib.Component.Network
{
    // 对WebClient进行封装
    public class Requester
    {
        public static RequestUrl requestUrl;
        public RestClient client;
        public WebProxy UsedProxy { get; set; }

        public Requester()
        {
            // Load defined simulate query header.
            var task = LoadRequestUrlConfig();
            client = ClientFactory();
            requestUrl = task.Result;
            client.BaseUrl = new System.Uri(requestUrl.BaseUrl);
        }
        public Requester(WebProxy proxy)
        {
            // specify the proxy applied to HttpClient
            //UsedHttpClientHandler = new HttpClientHandler() {AllowAutoRedirect = false, Proxy = proxy, UseCookies = true, AutomaticDecompression = DecompressionMethods.GZip };
            var task = LoadRequestUrlConfig();
            client = ClientFactory(proxy);
            UsedProxy = proxy;
            requestUrl = task.Result;
            client.BaseUrl = new System.Uri(requestUrl.BaseUrl);
        }
        protected static List<KeyValuePair<string, string>> LoadPresetRequestHeader()
        {
            var header = new List<KeyValuePair<string, string>>();
            var HeaderPath = ConfigurationManager.AppSettings["HeaderDefinationPath"];
            StreamReader HeaderConfigReader = new StreamReader(HeaderPath, Encoding.UTF8);
            var HeaderConfigJson = HeaderConfigReader.ReadToEnd();
            HeaderConfigReader.Close();
            var HeaderJObj = JObject.Parse(HeaderConfigJson);
            foreach (var h in HeaderJObj)
            {
                header.Add(new KeyValuePair<string, string>(h.Key, h.Value.ToString()));
            }
            return header;
        }
        public static RestClient ClientFactory(WebProxy proxy = null)
        {
            var _client = new RestClient();
            _client.FollowRedirects = false;
            _client.CookieContainer = new CookieContainer();
            _client.AddAllDefaultHeader(LoadPresetRequestHeader());
            _client.Proxy = proxy ?? new WebProxy();
            return _client;
        }

        private Task<RequestUrl> LoadRequestUrlConfig()
        {
            return Task.Run(() =>
            {
                using (var UrlProfileReader = new StreamReader(ConfigurationManager.AppSettings["RequestUrl"], Encoding.UTF8))
                {
                    var json = UrlProfileReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<RequestUrl>(json);
                }
            });
        }
    }
    public static class HttpClientExtension
    {
        public static void AddAllDefaultHeader(this RestClient client, List<KeyValuePair<string, string>> collection)
        {
            foreach (var h in collection)
            {
                client.AddDefaultHeader(h.Key, h.Value);
            }
        }
        public static void AddAllParameters(this RestRequest request, List<KeyValuePair<string, string>> Params)
        {
            foreach (var h in Params)
            {
                request.AddParameter(h.Key, h.Value);
            }
        }
        public static void AddCookies(this RestClient client, Cookie cookie)
        {
            client.CookieContainer.Add(cookie);
        }
        public static void AddAllCookies(this RestClient client, List<Cookie> cookie)
        {
            foreach (var ck in cookie)
            {
                client.AddCookies(ck);
            }
        }
    }
}
