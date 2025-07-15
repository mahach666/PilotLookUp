using PilotLookUp.Domain.Entities;
using System.Collections;
using System.Threading.Tasks;

namespace PilotLookUp.Utils.Strategies
{
    public class EnumerableTraceStrategy : ITraceStrategy
    {
        public bool CanTrace(object obj, TraceContext context) => obj is IEnumerable && !(obj is string);

        public async Task TraceAsync(object obj, TraceContext context)
        {
            foreach (var item in (IEnumerable)obj)
            {
                await context.Tracer.TraceInternalAsync(item, context.ObjectSet);
            }
        }
    }
} 