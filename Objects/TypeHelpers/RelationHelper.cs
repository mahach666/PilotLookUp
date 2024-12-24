using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class RelationHelper : PilotObjectHelper
    {
        public RelationHelper(IRelation obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Name;
        }
    }
}
