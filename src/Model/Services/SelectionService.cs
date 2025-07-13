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
        private readonly IObjectSetFactory _objectSetFactory;
        private ObjectSet _currentSelection;

        public SelectionService(IObjectMappingService objectMappingService, IObjectSetFactory objectSetFactory)
        {
            _objectMappingService = objectMappingService;
            _objectSetFactory = objectSetFactory;
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
                _currentSelection = _objectSetFactory.Create(null);
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