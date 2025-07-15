using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class StorageDataObjectHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public StorageDataObjectHelper(IThemeService themeService, IStorageDataObject obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
            _stringId = obj?.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\storageIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
