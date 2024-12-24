using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StateInfoHelper : PilotObjectHelper
    {
        public StateInfoHelper(IStateInfo obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.State.ToString();
            _isLookable = true;
        }
    }
}
