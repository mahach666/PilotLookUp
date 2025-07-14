using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PilotLookUp.Interfaces;

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
                var wrapped = context.Tracer.PilotObjectMap.Wrap(obj);
                if (wrapped == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TraceStrategyRegistry.TraceAsync: Wrap вернул null для типа {obj?.GetType().FullName}");
                }
                context.ObjectSet.Add(wrapped);
            }
        }
    }
} 