using Ascon.Pilot.SDK;
using System.Collections.ObjectModel;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            //ReadOnlyCollection<int> a = obj.AllOrgUnits();
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
