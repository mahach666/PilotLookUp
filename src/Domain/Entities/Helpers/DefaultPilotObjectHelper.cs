using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class DefaultPilotObjectHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DefaultPilotObjectHelper(
            IThemeService themeService,
            string name,
            string stringId,
            object lookUpObject,
            bool isLookable,
            ILogger logger)
            : base(themeService, logger)
        {
            _name = name;
            _stringId = stringId;
            _lookUpObject = lookUpObject;
            _isLookable = isLookable;
        }

        public override System.Windows.Media.Brush DefaultTextColor =>
            new System.Windows.Media.SolidColorBrush(_themeService.IsJediTheme ? System.Windows.Media.Colors.Black : System.Windows.Media.Colors.White);

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
} 