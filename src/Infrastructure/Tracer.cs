using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure.Strategies;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace PilotLookUp.Infrastructure
{
    public class Tracer
    {
        private readonly ILogger _logger;
        public Tracer(
            IObjectsRepository objectsRepository,
            IPilotObjectHelperFactory factory,
            IPilotObjectHelper senderObj,
            MemberInfo senderMember,
            IObjectSetFactory objectSetFactory,
            ILogger logger)
        {
            _logger = logger;
            PilotObjectMap = new PilotObjectMap(objectsRepository,
                factory,
                senderObj,
                senderMember);

            ObjectsRepository = objectsRepository;
            MemberInfo = senderMember;
            _objectSetFactory = objectSetFactory;
            _traceStrategyRegistry = new TraceStrategyRegistry(_logger);
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
