using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FilesSnapshotHelper : PilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
        }
    }
}
