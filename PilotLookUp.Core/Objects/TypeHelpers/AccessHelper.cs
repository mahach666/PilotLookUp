using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class AccessHelper : PilotObjectHelper
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
