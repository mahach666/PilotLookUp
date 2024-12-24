using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class TypeHelper : PilotObjectHelper
    {
        public TypeHelper(IType obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
        }
    }
}
