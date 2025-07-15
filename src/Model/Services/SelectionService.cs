using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Domain.Entities;
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
            System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.GetCurrentSelection: _currentSelection is null? {_currentSelection == null}, count: {_currentSelection?.Count ?? 0}");
            if (_currentSelection != null)
            {
                foreach (var obj in _currentSelection)
                {
                    System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.GetCurrentSelection: item type: {obj?.GetType().Name}, name: {obj?.Name}");
                }
            }
            return _currentSelection;
        }

        public void UpdateSelection(IEnumerable<object> rawObjects)
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.UpdateSelection: rawObjects null? {rawObjects == null}, count: {rawObjects?.Count() ?? 0}");
            _validationService.ValidateNotNull(rawObjects, nameof(rawObjects));
            var wrappedObjects = _objectMappingService.WrapMany(rawObjects);
            System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.UpdateSelection: wrappedObjects null? {wrappedObjects == null}, count: {wrappedObjects?.Count() ?? 0}");
            if (wrappedObjects?.Any() == true)
            {
                _currentSelection = _objectSetFactory.Create(null);
                _currentSelection.AddRange(wrappedObjects);
                System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.UpdateSelection: _currentSelection.Count = {_currentSelection.Count}");
            }
            else
            {
                _currentSelection = null;
                System.Diagnostics.Debug.WriteLine($"[TRACE] SelectionService.UpdateSelection: _currentSelection сброшен в null");
            }
        }

        public bool HasSelection()
        {
            return _currentSelection != null && _currentSelection.Any();
        }
    }
} 