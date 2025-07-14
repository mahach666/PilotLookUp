using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils.Strategies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PilotLookUp.Utils
{
    public class Tracer
    {
        public Tracer(IObjectsRepository objectsRepository, IPilotObjectHelperFactory factory, IPilotObjectHelper senderObj, MemberInfo senderMember, IObjectSetFactory objectSetFactory)
        {
            PilotObjectMap = new PilotObjectMap(objectsRepository, factory, senderObj, senderMember);
            ObjectsRepository = objectsRepository;
            MemberInfo = senderMember;
            _objectSetFactory = objectSetFactory;
            _traceStrategyRegistry = new TraceStrategyRegistry();
            RegisterDefaultStrategies(_traceStrategyRegistry);
        }

        public int AdaptiveTimer { get; set; } = 200;
        public PilotObjectMap PilotObjectMap { get; }
        public IObjectsRepository ObjectsRepository { get; }
        public MemberInfo MemberInfo { get; }
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly TraceStrategyRegistry _traceStrategyRegistry;
        private readonly ConcurrentDictionary<Guid, Task<object>> _objectCache = new();
        private readonly ConcurrentDictionary<Guid, Task<object>> _historyCache = new();

        public async Task<ObjectSet> Trace(object obj)
        {
            var objectSet = _objectSetFactory.Create(MemberInfo);
            await TraceInternalAsync(obj, objectSet);
            return objectSet;
        }

        internal async Task TraceInternalAsync(object obj, ObjectSet objectSet)
        {
            var context = new TraceContext(this, objectSet);
            await _traceStrategyRegistry.TraceAsync(obj, context);
        }

        private void RegisterDefaultStrategies(TraceStrategyRegistry registry)
        {
            registry.Register(new GuidTraceStrategy());
            registry.Register(new KeyValuePairGuidIntTraceStrategy());
            registry.Register(new EnumerableTraceStrategy());
            // Можно добавить другие стратегии по мере необходимости
        }

        public Task<object> GetOrAddObjectAsync(Guid guid, Func<Task<object>> factory)
        {
            return _objectCache.GetOrAdd(guid, _ => factory());
        }
        public Task<object> GetOrAddHistoryAsync(Guid guid, Func<Task<object>> factory)
        {
            return _historyCache.GetOrAdd(guid, _ => factory());
        }
    }
}
