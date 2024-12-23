using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
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
        public AccessHelper( IAccess obj, IObjectsRepository objectsRepository) : base (objectsRepository)
        {
            LookUpObject = obj;
            Name = obj.AccessLevel.ToString();
        }
    }
}
