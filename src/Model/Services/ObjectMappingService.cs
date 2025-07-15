using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class ObjectMappingService : IObjectMappingService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly PilotObjectMap _pilotObjectMap;
        private readonly IValidationService _validationService;
        private readonly IPilotObjectHelperFactory _factory;
        private readonly ILogger _logger;

        public ObjectMappingService(IObjectsRepository objectsRepository, IPilotObjectHelperFactory factory, IValidationService validationService, ILogger logger)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(objectsRepository, factory, validationService);
            _objectsRepository = objectsRepository;
            _factory = factory;
            _pilotObjectMap = new PilotObjectMap(_objectsRepository, _factory);
            _logger = logger;
        }

        public IPilotObjectHelper Wrap(object rawObject)
        {
            _logger.Trace($"[TRACE] ObjectMappingService.Wrap: rawObject type: {rawObject?.GetType().Name}");
            _validationService.ValidateNotNull(rawObject, nameof(rawObject));
            var result = _pilotObjectMap.Wrap(rawObject);
            _logger.Trace($"[TRACE] ObjectMappingService.Wrap: result is null? {result == null}, type: {result?.GetType().Name}, name: {result?.Name}");
            return result;
        }

        public IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects)
        {
            _validationService.ValidateNotNull(rawObjects, nameof(rawObjects));
            var result = rawObjects?.Select(Wrap).ToList() ?? new List<IPilotObjectHelper>();
            _logger.Trace($"[TRACE] ObjectMappingService.WrapMany: rawObjects count: {rawObjects?.Count() ?? 0}, result count: {result.Count}");
            foreach (var r in result)
            {
                _logger.Trace($"[TRACE] ObjectMappingService.WrapMany: item is null? {r == null}, type: {r?.GetType().Name}, name: {r?.Name}");
            }
            return result;
        }
    }
} 