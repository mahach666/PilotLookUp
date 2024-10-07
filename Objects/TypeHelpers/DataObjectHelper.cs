using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class DataObjectHelper : PilotObjectHelper
    {
        public DataObjectHelper(IDataObject obj)
        {
            LookUpObject = obj;
            Name = obj.DisplayName;
        }
    }
}
