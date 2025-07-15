using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class OtherHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public OtherHelper(IThemeService themeService,
            object value,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = value;
            _name = value?.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
