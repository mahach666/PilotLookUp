using System.Collections.Generic;
using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;

namespace PilotLookUp.Domain.Interfaces
{
    /// <summary>
    /// Service that builds DTOs (key/value + meta) for a Pilot IDataObject.
    /// Pure domain – не содержит ссылок на WPF.
    /// </summary>
    public interface IDataObjectService
    {
        IEnumerable<AttrDTO> GetAttrDTOs(IDataObject dataObject);
    }
} 