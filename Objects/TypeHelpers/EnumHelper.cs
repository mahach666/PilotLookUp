using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class EnumHelper : PilotObjectHelper
    {
        public EnumHelper(Enum obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\enumIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
