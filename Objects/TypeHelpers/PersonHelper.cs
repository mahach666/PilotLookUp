using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            //System.Collections.ObjectModel.ReadOnlyCollection<int> a =obj.AllOrgUnits();
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
