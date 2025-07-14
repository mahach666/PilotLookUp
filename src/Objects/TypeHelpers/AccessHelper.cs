using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public AccessHelper(IAccess obj)
        {
            _lookUpObject = obj;
            _name = obj.AccessLevel.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\accessIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
