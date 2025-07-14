using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OrganizationUnitHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public OrganizationUnitHelper(IThemeService themeService, IOrganisationUnit obj)
            : base(themeService)
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] OrganizationUnitHelper: Конструктор вызван для типа {obj?.GetType().FullName}");
            _lookUpObject = obj;
            _name = obj?.Title;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
            System.Diagnostics.Debug.WriteLine($"[TRACE] OrganizationUnitHelper: _name = {_name}, _stringId = {_stringId}");
        }

        public override BitmapImage GetImage()
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] OrganizationUnitHelper.GetImage: Вызван для типа {_lookUpObject?.GetType().FullName}");
            try
            {
                var image = new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
                System.Diagnostics.Debug.WriteLine($"[TRACE] OrganizationUnitHelper.GetImage: Иконка создана успешно");
                return image;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TRACE] OrganizationUnitHelper.GetImage: Ошибка при создании иконки: {ex.Message}");
                return null;
            }
        }
    }
}
