using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
