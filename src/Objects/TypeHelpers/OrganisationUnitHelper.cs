using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OrganizationUnitHelper : PilotObjectHelper
    {
        public OrganizationUnitHelper(IOrganisationUnit obj)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
