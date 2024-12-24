using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class DateTimeHelper : PilotObjectHelper
    {
        public DateTimeHelper(DateTime obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }
    }
}
