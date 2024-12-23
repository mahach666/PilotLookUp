using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class PositionHelper : PilotObjectHelper
    {
        public PositionHelper(IPosition obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = obj;
            Name = objectsRepository.GetOrganisationUnit(obj.Position).Title;
        }
    }
}
