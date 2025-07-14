using PilotLookUp.Contracts;
using PilotLookUp.Objects.TypeHelpers;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(IPilotObjectHelper dataObjectHelper);
    }
}
