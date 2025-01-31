using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class HistoryItemHelper : PilotObjectHelper
    {
        public HistoryItemHelper(IHistoryItem obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\historyItemIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
