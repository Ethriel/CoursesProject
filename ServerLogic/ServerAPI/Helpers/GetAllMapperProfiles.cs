using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace ServerAPI.Helpers
{
    public static class GetAllMapperProfiles
    {
        static Type[] mapperProfiles;
        static GetAllMapperProfiles()
        {
            var parentProfile = typeof(Profile);
            var assembly = Assembly.GetExecutingAssembly();
            var allTypes = assembly.GetTypes();
            mapperProfiles = allTypes.Where(x => x.IsSubclassOf(parentProfile)).ToArray();
        }
        public static Type[] MapperProfiles { get => mapperProfiles; }
    }
}
