using SCUTEAW_Lib.Model.Extractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace SCUTEAW_Lib.Component.Extractor
{
    public class ContentExtractor
    {
        public static readonly Lazy<ContentExtractorModel> ExtractorModel = new Lazy<ContentExtractorModel>(() =>
        {
            var RegexModelPath = ConfigurationManager.AppSettings["ExtractorRegexDefinationPath"];
            using (var FileReader = new StreamReader(RegexModelPath, Encoding.UTF8))
            {
                var ExtractorJsonStr = FileReader.ReadToEnd();
                return JsonConvert.DeserializeObject<ContentExtractorModel>(ExtractorJsonStr);
            }
        });
        public static ContentExtractorModel Extractor { get { return ExtractorModel.Value; } }
        public static string ExtractCsrfToken(string rawString)
        {
            var CsrfToken = (new Regex(Extractor.MatchCsrfTokenRegex)).Match(rawString).Value;
            foreach (var patt in Extractor.ClearNoNeedCsrfTag)
            {
                CsrfToken = CsrfToken.Replace(patt, "");
            }
            return CsrfToken;
        }
        public static (string Major,string Name) ExtractPersonalInfo(string rawString)
        {
            var major = (new Regex(Extractor.MatchMajorAndClass)).Match(rawString).Value;
            foreach (var patt in Extractor.ClearNoNeedMatchMajorAndClassTag)
            {
                major = major.Replace(patt, "");
            }
            var name = (new Regex(Extractor.MatchStudentName)).Match(rawString).Value;
            foreach (var patt in Extractor.ClearNoNeedStudentNameTag)
            {
                name = name.Replace(patt, "");
            }
            return (major,name);
        }
        public static List<KeyValuePair<string,string>> ExtractRecentScore(string rawString)
        {
            var list = new List<KeyValuePair<string, string>>();
            var DOMParser = new HtmlDocument();
            DOMParser.LoadHtml(rawString);
            var CourseNode = DOMParser.DocumentNode.SelectNodes(Extractor.CourseReg);
            var ScoreNode = DOMParser.DocumentNode.SelectNodes(Extractor.ScoreReg);
            CourseNode.Remove(0);
            for (int i = 0; i < CourseNode.Count; i++)
            {
                list.Add(new KeyValuePair<string, string>(CourseNode[i].InnerText.Trim(), ScoreNode[i].InnerText));
            }
            return list;
        }
        public static List<string> ExtractRecentCourse(string rawString)
        {
            var parser = new HtmlDocument();
            parser.LoadHtml(rawString);
            var CourseNodes = parser.DocumentNode.SelectNodes(Extractor.CourseReg);
            CourseNodes.RemoveAt(0);
            return CourseNodes.Select(x => x.InnerText.Trim()).ToList();
        }
        public static (List<string> SelectableYear,List<string> SelectableTerm,string SelectedYear,string SelectedTerm) 
                        ExtractQueryCourseScheduleParam(string rawString)
        {
            var parser = new HtmlDocument();
            parser.LoadHtml(rawString);
            var YearNode = parser.DocumentNode.SelectNodes(Extractor.QueryableYearReg);
            var Selectedyear = YearNode.FirstOrDefault(x => x.GetAttributeValue("selected", "") == "selected")?.GetAttributeValue("value", null);
            YearNode.RemoveAt(0);
            List<string> YearList = YearNode.Select(x => x.InnerText.Trim()).ToList();
            parser.LoadHtml(rawString);
            var TernNode = parser.DocumentNode.SelectNodes(Extractor.QueryableTermReg);
            var SelectedTerm = TernNode.FirstOrDefault(x => x.GetAttributeValue("selected", "") == "selected")?.GetAttributeValue("value", null);
            TernNode.RemoveAt(0);
            List<string> TermList = TernNode.Select(x => x.InnerText.Trim()).ToList();
            return (YearList, TermList, Selectedyear, SelectedTerm);
        }
    }
}
