using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class TypeHelper : PilotObjectHelper
    {
        public TypeHelper(IType obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = obj;
            Name = obj.Title;
        }
    }
}
