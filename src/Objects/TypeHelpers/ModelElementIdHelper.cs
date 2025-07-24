using Ascon.Pilot.Bim.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class ModelElementIdHelper : PilotObjectHelper
    {
        public ModelElementIdHelper(IModelElementId obj)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
            _stringId = obj.ElementId.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\modelidicon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
