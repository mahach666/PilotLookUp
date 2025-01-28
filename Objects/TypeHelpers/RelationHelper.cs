using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class RelationHelper : PilotObjectHelper
    {
        public RelationHelper(IRelation obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Name is null ? obj.Id.ToString() : obj.Name;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
