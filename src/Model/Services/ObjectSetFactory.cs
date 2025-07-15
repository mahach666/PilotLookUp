using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Reflection;

namespace PilotLookUp.Model.Services
{
    public class ObjectSetFactory : IObjectSetFactory
    {
        private readonly IThemeService _themeService;
        private readonly IValidationService _validationService;
        private readonly ILogger _logger;

        public ObjectSetFactory(IThemeService themeService,
            IValidationService validationService,
            ILogger logger)
        {
            _themeService = themeService;
            _validationService = validationService;
            _logger = logger;
        }

        public ObjectSet Create(MemberInfo memberInfo)
        {
            return new ObjectSet(_themeService, memberInfo, _validationService, _logger);
        }
    }
} 