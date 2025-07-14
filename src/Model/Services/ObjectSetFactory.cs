using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Reflection;

namespace PilotLookUp.Model.Services
{
    public class ObjectSetFactory : IObjectSetFactory
    {
        private readonly IThemeService _themeService;
        private readonly IValidationService _validationService;
        public ObjectSetFactory(IThemeService themeService, IValidationService validationService)
        {
            _themeService = themeService;
            _validationService = validationService;
        }

        public ObjectSet Create(MemberInfo memberInfo)
        {
            return new ObjectSet(_themeService, memberInfo, _validationService);
        }
    }
} 