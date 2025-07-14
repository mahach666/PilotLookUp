using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    public class TraceStrategyRegistry
    {
        private readonly List<ITraceStrategy> _strategies = new List<ITraceStrategy>();

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
                // fallback: просто обернуть через PilotObjectMap
                context.ObjectSet.Add(context.Tracer.PilotObjectMap.Wrap(obj));
            }
        }
    }
} 