using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class FilesSnapshotHelper : PilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
        }
    }
}
