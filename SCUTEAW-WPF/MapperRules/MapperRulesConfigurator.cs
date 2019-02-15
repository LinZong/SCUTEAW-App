using System;
using AutoMapper;
using Newtonsoft.Json.Linq;
using SCUTEAW_App.MapperRules.Rules;

namespace SCUTEAW_App.MapperRules
{
    public static class MapperRulesConfigurator
    {
       public static void Configure()
        {
            Mapper.Initialize(cfg => 
            {
                cfg.AddProfile<MapJTokenScoreToModel>();
                cfg.AddProfile<MapJTokenCourseToModel>();
            });
        }
    }

    public static class MapJTokenToStringExtension
    {
        public static TResult Mapper<TResult>(this JObject jObj, string ValueAttribute, Func<JToken, TResult> converter)
        {
            if (jObj.TryGetValue(ValueAttribute, out JToken token))
                return converter(token);
            else return default(TResult);
        }
        public static string StringMapper(this JObject jObj, string ValueAttribute)
        {
            if (jObj.TryGetValue(ValueAttribute, out JToken token))
                return token.ToString();
            else return string.Empty;// as null.
        }
        public static void ConfigJTokenToString<TViewModel,TDestMember>(this IMemberConfigurationExpression<JObject, TViewModel, TDestMember> opt, string ValueAttribute)
        {
            opt.AllowNull();
            opt.MapFrom(s => s.StringMapper(ValueAttribute));
        }
    }
}
