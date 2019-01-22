using AutoMapper;
using Newtonsoft.Json.Linq;
using SCUTEAW_App.Model;

namespace SCUTEAW_App.MapperRules.Rules
{
    public class MapJTokenScoreToModel:Profile
    {
        public MapJTokenScoreToModel()
        {
            CreateMap<JObject, ScoreItemModel>()
            .ForMember(x => x.CourseName, opt => opt.ConfigJTokenToString("kcmc"))
            .ForMember(x => x.CourseNature, opt => opt.ConfigJTokenToString("kcxzmc"))
            .ForMember(x => x.Score, opt => opt.ConfigJTokenToString("bfzcj"))
            .ForMember(x => x.CourseCredit, opt => opt.ConfigJTokenToString("xf"))
            .ForMember(x => x.CourseGPA, opt => opt.ConfigJTokenToString("jd"))
            .ForMember(x => x.ScoreType, opt => opt.ConfigJTokenToString("ksxz"))
            .ForMember(x => x.CourseCollege, opt => opt.ConfigJTokenToString("kkbmmc"))
            .ForMember(x => x.CourseType, opt => opt.ConfigJTokenToString("kclbmc"));
        }
    }
}
