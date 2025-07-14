using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessRecordHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public AccessRecordHelper(IAccessRecord obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId).Title;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\accessIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}