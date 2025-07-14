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

        public ObjectMappingService(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
            _pilotObjectMap = new PilotObjectMap(_objectsRepository);
        }

        public IPilotObjectHelper Wrap(object rawObject)
        {
            return _pilotObjectMap.Wrap(rawObject);
        }

        public IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects)
        {
            return rawObjects?.Select(Wrap) ?? Enumerable.Empty<IPilotObjectHelper>();
        }
    }
} 