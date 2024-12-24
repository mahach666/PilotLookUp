using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class OrganisationUnitHelper : PilotObjectHelper
    {
        public OrganisationUnitHelper(IOrganisationUnit obj, IObjectsRepository objectsRepository): base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
