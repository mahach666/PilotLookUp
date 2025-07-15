using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Utils
{
    public class TraceStrategyRegistry
    {
        private readonly List<ITraceStrategy> _strategies = new List<ITraceStrategy>();
        private readonly ILogger _logger;

        public TraceStrategyRegistry(ILogger logger)
        {
            _logger = logger;
        }

        public void Register(ITraceStrategy strategy)
        {
            _strategies.Add(strategy);
        }

        public async Task TraceAsync(object obj, TraceContext context)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanTrace(obj, context));
            if (strategy != null)
            {
                await strategy.TraceAsync(obj, context);
            }
            else
            {
                var wrapped = context.Tracer.PilotObjectMap.Wrap(obj);
                if (wrapped == null)
                {
                    _logger.Trace($"[TRACE] TraceStrategyRegistry.TraceAsync: Wrap вернул null для типа {obj?.GetType().FullName}");
                }
                context.ObjectSet.Add(wrapped);
            }
        }
    }
} 