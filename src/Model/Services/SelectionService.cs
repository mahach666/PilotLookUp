using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class SelectionService : ISelectionService
    {
        private readonly IObjectMappingService _objectMappingService;
        private ObjectSet _currentSelection;

        public SelectionService(IObjectMappingService objectMappingService)
        {
            _objectMappingService = objectMappingService;
        }

        public ObjectSet GetCurrentSelection()
        {
            return _currentSelection;
        }

        public void UpdateSelection(IEnumerable<object> rawObjects)
        {
            if (rawObjects?.Any() == true)
            {
                var wrappedObjects = _objectMappingService.WrapMany(rawObjects);
                _currentSelection = new ObjectSet(null);
                _currentSelection.AddRange(wrappedObjects);
            }
            else
            {
                _currentSelection = null;
            }
        }

        public bool HasSelection()
        {
            return _currentSelection != null && _currentSelection.Any();
        }
    }
} 