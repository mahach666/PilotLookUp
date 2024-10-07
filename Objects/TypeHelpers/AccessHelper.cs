using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class AccessHelper : PilotObjectHelper
    {
        public AccessHelper(IAccess obj)
        {
            LookUpObject = obj;
            Name = obj.AccessLevel.ToString();
        }
    }
}
