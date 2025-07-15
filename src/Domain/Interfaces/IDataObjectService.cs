using PilotLookUp.Domain.Entities;
using System.Collections.Generic;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(IPilotObjectHelper dataObjectHelper);
    }
}
