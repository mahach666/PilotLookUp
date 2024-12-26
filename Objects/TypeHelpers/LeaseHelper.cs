using Ascon.Pilot.SDK;
using System.Runtime.Remoting.Lifetime;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LeaseHelper : PilotObjectHelper
    {
        public LeaseHelper(ILease obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }
    }
}
