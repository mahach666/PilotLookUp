using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class HistoryItemHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public HistoryItemHelper(object value)
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
