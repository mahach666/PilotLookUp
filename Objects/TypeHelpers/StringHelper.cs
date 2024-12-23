using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class StringHelper : PilotObjectHelper
    {
        public StringHelper(string value, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = value;
            Name = value;
        }
    }
}
