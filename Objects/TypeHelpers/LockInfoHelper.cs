using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LockInfoHelper : PilotObjectHelper
    {
        public LockInfoHelper(ILockInfo value, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = true;
        }
    }
}