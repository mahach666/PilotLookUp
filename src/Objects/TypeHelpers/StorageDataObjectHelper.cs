using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StorageDataObjectHelper : PilotObjectHelper
    {
        public StorageDataObjectHelper(IStorageDataObject obj)
        {
            _lookUpObject = obj;
            _name = obj.DataObject.DisplayName;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\storageIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
