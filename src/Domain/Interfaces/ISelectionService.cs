using PilotLookUp.Domain.Entities;
using System.Collections.Generic;

namespace PilotLookUp.Domain.Interfaces
{
    public interface ISelectionService
    {
        ObjectSet GetCurrentSelection();
        void UpdateSelection(IEnumerable<object> rawObjects);
        bool HasSelection();
    }
} 