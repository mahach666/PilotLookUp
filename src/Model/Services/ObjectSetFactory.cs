using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Reflection;

namespace PilotLookUp.Model.Services
{
    public class ObjectSetFactory : BaseValidatedService, IObjectSetFactory
    {
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;

        public ObjectSetFactory(IThemeService themeService,
            IValidationService validationService,
            ILogger logger) : base(validationService, themeService, logger)
        {
            _themeService = themeService;
            _logger = logger;
        }

        public ObjectSet Create(MemberInfo memberInfo)
        {
            return new ObjectSet(_themeService, memberInfo, _validationService, _logger);
        }
    }
}