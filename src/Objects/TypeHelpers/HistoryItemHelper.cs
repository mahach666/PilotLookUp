using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class HistoryItemHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public HistoryItemHelper(IThemeService themeService, object obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
