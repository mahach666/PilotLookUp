using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class LongHelper : PilotObjectHelper
    {
        public LongHelper(long value, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }
    }
}
