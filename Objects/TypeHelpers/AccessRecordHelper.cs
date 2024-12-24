using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessRecordHelper : PilotObjectHelper
    {
        public AccessRecordHelper(IAccessRecord obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId).Title;
            _isLookable = true;            
        }
    }
}
