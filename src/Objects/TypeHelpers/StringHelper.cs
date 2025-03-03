using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StringHelper : PilotObjectHelper
    {
        public StringHelper(string value, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = value;
            _name = value;
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stringIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
