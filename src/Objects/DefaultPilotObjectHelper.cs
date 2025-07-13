using PilotLookUp.Interfaces;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects
{
    public class DefaultPilotObjectHelper : PilotObjectHelper
    {
        private readonly IThemeService _themeService;

        public DefaultPilotObjectHelper(IThemeService themeService, string name, string stringId, object lookUpObject, bool isLookable)
        {
            _themeService = themeService;
            _name = name;
            _stringId = stringId;
            _lookUpObject = lookUpObject;
            _isLookable = isLookable;
        }

        public override System.Windows.Media.Brush DefaultTextColor =>
            new System.Windows.Media.SolidColorBrush(_themeService.IsJediTheme ? System.Windows.Media.Colors.Black : System.Windows.Media.Colors.White);

        public override BitmapImage GetImage()
        {
            // Реализуйте по необходимости
            return null;
        }
    }
} 