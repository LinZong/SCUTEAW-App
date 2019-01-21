using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUTEAW_Lib.Model.Extractor
{
    public class ContentExtractorModel
    {
        public string MatchCsrfTokenRegex { get; set; }
        public string[] ClearNoNeedCsrfTag { get; set; }
        public string MatchStudentName { get; set; }
        public string MatchMajorAndClass { get; set; }
        public string[] ClearNoNeedStudentNameTag { get; set; }
        public string[] ClearNoNeedMatchMajorAndClassTag { get; set; }
        public string CourseReg { get; set; }
        public string ScoreReg { get; set; }
        public string QueryableYearReg { get; set; }
        public string QueryableTermReg { get; set; }
    }
}
