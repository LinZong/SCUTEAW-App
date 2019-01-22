using AutoMapper;
using Newtonsoft.Json.Linq;
using SCUTEAW_App.Model;

namespace SCUTEAW_App.MapperRules.Rules
{
    public class MapJTokenCourseToModel : Profile
    {
        public MapJTokenCourseToModel()
        {
            CreateMap<JObject, CourseItemModel>()
                .ForMember(x => x.CourseName, opt => opt.ConfigJTokenToString("kcmc"))
                .ForMember(x => x.HaveCourseRoomAndTeacher, opt => opt.MapFrom<ResolveCourseRoomAndTeacher>())
                .ForMember(x => x.HaveCourseWeek, opt => opt.ConfigJTokenToString("zcd"));
        }

        public class ResolveCourseRoomAndTeacher : IValueResolver<JObject, CourseItemModel, string>
        {
            public string Resolve(JObject source, CourseItemModel destination, string destMember, ResolutionContext context)
            {
                var HaveCourseRoom = source.StringMapper("cdmc");
                var Teacher = source.StringMapper("xm");
                return $"{HaveCourseRoom}   {Teacher}";
            }
        }
    }
}
