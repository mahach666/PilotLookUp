using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessRecordHelper : PilotObjectHelper
    {
        public AccessRecordHelper(IAccessRecord obj, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId)?.Title ?? obj.OrgUnitId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("accessIcon");
        }
    }
}