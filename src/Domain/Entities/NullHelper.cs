using System.Windows.Media.Imaging;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Entities
{
    public class NullHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public NullHelper()
            : base(null)
        {
            _lookUpObject = null;
            _name = PilotLookUp.Resources.Strings.NullName;
            _isLookable = false;
        }

        public NullHelper(IThemeService themeService, object value)
            : base(themeService)
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
