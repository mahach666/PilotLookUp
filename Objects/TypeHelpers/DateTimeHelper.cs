using Ascon.Pilot.SDK;
using System;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DateTimeHelper : PilotObjectHelper
    {
        public DateTimeHelper(DateTime obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }
    }
}
