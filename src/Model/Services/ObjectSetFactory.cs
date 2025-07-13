using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Reflection;

namespace PilotLookUp.Model.Services
{
    public class ObjectSetFactory : IObjectSetFactory
    {
        private readonly IThemeService _themeService;
        public ObjectSetFactory(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public ObjectSet Create(MemberInfo memberInfo)
        {
            return new ObjectSet(_themeService, memberInfo);
        }
    }
} 