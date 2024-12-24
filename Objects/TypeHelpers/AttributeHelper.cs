using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class AttributeHelper : PilotObjectHelper
    {
        public AttributeHelper(IAttribute obj , IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
        }
    }
}
