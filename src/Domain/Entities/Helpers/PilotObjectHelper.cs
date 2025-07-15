using PilotLookUp.Core;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public abstract class PilotObjectHelper : IPilotObjectHelper
    {
        protected readonly IThemeService _themeService;
        protected readonly ILogger _logger;
        protected string _name { get; set; }
        public string Name { get => _name; }

        protected string _stringId { get; set; }
        public string StringId { get => _stringId; }

        protected object _lookUpObject { get; set; }
        public object LookUpObject { get => _lookUpObject; }

        protected bool _isLookable { get; set; }
        public bool IsLookable { get => _lookUpObject != null ? _isLookable : false; }

        public ObjReflection Reflection { get 
            { return LookUpObject == null 
                    ? ObjReflection.Empty(_logger) 
                    : new ObjReflection(this, _logger); 
            } 
        }

        public PilotObjectHelper(IThemeService themeService, ILogger logger)
        {
            _themeService = themeService;
            _logger = logger;
        }

        public virtual Brush DefaultTextColor =>
            new SolidColorBrush(
                _themeService.CurrentTheme == Ascon.Pilot.Themes.ThemeNames.Jedi 
                ? Colors.Black 
                : Colors.White);
        public abstract BitmapImage GetImage();
    }
}
