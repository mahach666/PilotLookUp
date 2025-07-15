using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class SelectionService : BaseValidatedService, ISelectionService
    {
        private readonly IObjectMappingService _objectMappingService;
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly ILogger _logger;
        private ObjectSet _currentSelection;

        public SelectionService(
            IObjectMappingService objectMappingService,
            IObjectSetFactory objectSetFactory,
            IValidationService validationService,
            ILogger logger) : base(validationService, objectMappingService, objectSetFactory, logger)
        {
            _objectMappingService = objectMappingService;
            _objectSetFactory = objectSetFactory;
            _logger = logger;
        }

        public ObjectSet GetCurrentSelection()
        {
            _logger.Trace($"SelectionService.GetCurrentSelection: _currentSelection is null? {_currentSelection == null}, count: {_currentSelection?.Count ?? 0}");
            if (_currentSelection != null)
            {
                foreach (var obj in _currentSelection)
                {
                    _logger.Trace($"SelectionService.GetCurrentSelection: item type: {obj?.GetType().Name}, name: {obj?.Name}");
                }
            }
            return _currentSelection;
        }

        public void UpdateSelection(IEnumerable<object> rawObjects)
        {
            _logger.Trace($"SelectionService.UpdateSelection: rawObjects null? {rawObjects == null}, count: {rawObjects?.Count() ?? 0}");
            _validationService.ValidateNotNull(rawObjects, nameof(rawObjects));
            var wrappedObjects = _objectMappingService.WrapMany(rawObjects);
            _logger.Trace($"SelectionService.UpdateSelection: wrappedObjects null? {wrappedObjects == null}, count: {wrappedObjects?.Count() ?? 0}");
            if (wrappedObjects?.Any() == true)
            {
                _currentSelection = _objectSetFactory.Create(null);
                _currentSelection.AddRange(wrappedObjects);
                _logger.Trace($"SelectionService.UpdateSelection: _currentSelection.Count = {_currentSelection.Count}");
            }
            else
            {
                _currentSelection = null;
                _logger.Trace($"SelectionService.UpdateSelection: _currentSelection сброшен в null");
            }
        }

        public bool HasSelection()
        {
            return _currentSelection != null && _currentSelection.Any();
        }
    }
} 