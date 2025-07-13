using PilotLookUp.Objects;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface ISelectionService
    {
        ObjectSet GetCurrentSelection();
        void UpdateSelection(IEnumerable<object> rawObjects);
        bool HasSelection();
    }
} 