using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class AccessRecordHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public AccessRecordHelper(IThemeService themeService, IAccessRecord obj, IObjectsRepository objectsRepository)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId)?.Title;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\accessIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}