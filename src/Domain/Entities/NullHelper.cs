using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class NullHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public NullHelper(ILogger logger)
            : base(null, logger)
        {
            _lookUpObject = null;
            _name = PilotLookUp.Resources.Strings.NullName;
            _isLookable = false;
        }

        public NullHelper(IThemeService themeService,
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
