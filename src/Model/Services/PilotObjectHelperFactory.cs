using PilotLookUp.Interfaces;
using PilotLookUp.Objects;

namespace PilotLookUp.Model.Services
{
    public class PilotObjectHelperFactory : IPilotObjectHelperFactory
    {
        private readonly IThemeService _themeService;
        public PilotObjectHelperFactory(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public IPilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable)
        {
            return new DefaultPilotObjectHelper(_themeService, name, stringId, lookUpObject, isLookable);
        }
    }
} 