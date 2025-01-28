using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class ObjectsRepositoryHelper : PilotObjectHelper
    {
        public ObjectsRepositoryHelper( IObjectsRepository objectsRepository) 
        {
            _lookUpObject = objectsRepository;
            _name = "ObjectsRepository";
            _isLookable = true;
        }
    }
}
