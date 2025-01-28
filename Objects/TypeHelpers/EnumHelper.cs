using Ascon.Pilot.SDK;
using System;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class EnumHelper : PilotObjectHelper
    {
        public EnumHelper(Enum obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }
    }
}
