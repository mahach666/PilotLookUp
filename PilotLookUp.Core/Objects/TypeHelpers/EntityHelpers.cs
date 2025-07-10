using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
        }
    }

    public class OrganisationUnitHelper : PilotObjectHelper
    {
        public OrganisationUnitHelper(IOrganisationUnit obj)
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

    public class PositionHelper : PilotObjectHelper
    {
        public PositionHelper(IPosition obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            // IPosition не имеет Title, используем ID должности
            var orgUnit = objectsRepository.GetOrganisationUnit(obj.Position);
            _name = orgUnit?.Title ?? $"Position {obj.Position}";
            _isLookable = true;
            _stringId = obj.Position.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 
