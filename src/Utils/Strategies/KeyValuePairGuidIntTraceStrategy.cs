using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PilotLookUp.Extensions;

namespace PilotLookUp.Utils.Strategies
{
    public class KeyValuePairGuidIntTraceStrategy : ITraceStrategy
    {
        public bool CanTrace(object obj, TraceContext context) => obj is KeyValuePair<Guid, int>;

        public async Task TraceAsync(object obj, TraceContext context)
        {
            var tracer = context.Tracer;
            var objectsRepository = tracer.ObjectsRepository;
            var pair = (KeyValuePair<Guid, int>)obj;
            var loadedObj = await tracer.GetOrAddObjectAsync(pair.Key, async () => (object)await objectsRepository.GetObjectWithTimeout(pair.Key)) as IDataObject;
            var loadedPair = new KeyValuePair<IDataObject, int>(loadedObj, pair.Value);
            context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(loadedPair));
        }
    }
} 