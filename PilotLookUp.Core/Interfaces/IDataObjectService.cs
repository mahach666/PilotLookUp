using PilotLookUp.Contracts;
using PilotLookUp.Core.Objects.TypeHelpers;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(DataObjectHelper dataObjectHelper);
    }
}
