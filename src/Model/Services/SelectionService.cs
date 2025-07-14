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
        private readonly IValidationService _validationService;
        private ObjectSet _currentSelection;

        public SelectionService(IObjectMappingService objectMappingService, IObjectSetFactory objectSetFactory, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(objectMappingService, objectSetFactory, validationService);
            _objectMappingService = objectMappingService;
            _objectSetFactory = objectSetFactory;
        }

        public ObjectSet GetCurrentSelection()
        {
            return _currentSelection;
        }

        public void UpdateSelection(IEnumerable<object> rawObjects)
        {
            _validationService.ValidateNotNull(rawObjects, nameof(rawObjects));
            var wrappedObjects = _objectMappingService.WrapMany(rawObjects);
            if (wrappedObjects?.Any() == true)
            {
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