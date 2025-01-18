using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class HistoryItemHelper : PilotObjectHelper
    {
        public HistoryItemHelper(IHistoryItem obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
