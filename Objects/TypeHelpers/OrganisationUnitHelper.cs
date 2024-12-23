using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class OrganisationUnitHelper : PilotObjectHelper
    {
        public OrganisationUnitHelper(IOrganisationUnit obj, IObjectsRepository objectsRepository): base(objectsRepository)
        {
            LookUpObject = obj;
            Name = obj.Title;
        }
    }
}
