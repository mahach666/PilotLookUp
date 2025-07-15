using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class OrganizationUnitHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public OrganizationUnitHelper(IThemeService themeService,
            IOrganisationUnit obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _logger.Trace($"OrganizationUnitHelper: Конструктор вызван для типа {obj?.GetType().FullName}");
            _lookUpObject = obj;
            _name = obj?.Title;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
            _logger.Trace($"OrganizationUnitHelper: _name = {_name}, _stringId = {_stringId}");
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
