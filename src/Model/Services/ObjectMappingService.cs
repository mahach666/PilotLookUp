using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class ObjectMappingService : IObjectMappingService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly PilotObjectMap _pilotObjectMap;
        private readonly IValidationService _validationService;

        public ObjectMappingService(IObjectsRepository objectsRepository, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(objectsRepository, validationService);
            _objectsRepository = objectsRepository;
            _pilotObjectMap = new PilotObjectMap(_objectsRepository);
        }

        public IPilotObjectHelper Wrap(object rawObject)
        {
            _validationService.ValidateNotNull(rawObject, nameof(rawObject));
            return _pilotObjectMap.Wrap(rawObject);
        }

        public IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects)
        {
            _validationService.ValidateNotNull(rawObjects, nameof(rawObjects));
            return rawObjects?.Select(Wrap) ?? Enumerable.Empty<IPilotObjectHelper>();
        }
    }
} 