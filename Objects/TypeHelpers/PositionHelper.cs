using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PositionHelper : PilotObjectHelper
    {
        public PositionHelper(IPosition obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.Position).Title;
            _isLookable = true;
        }
    }
}
