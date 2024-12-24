using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class FileHelper : PilotObjectHelper
    {
        public FileHelper(IFile obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Name;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
