using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PositionHelper : PilotObjectHelper
    {
        public PositionHelper(IPosition obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.Position)?.Title ?? obj.Position.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("organisationUnitIcon");
        }
    }
}
