using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class AccessRecordHelper : PilotObjectHelper
    {
        public AccessRecordHelper(IAccessRecord obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = obj;
            Name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId).Title;
        }
    }
}
