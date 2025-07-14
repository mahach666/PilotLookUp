using PilotLookUp.Objects;
using System.Collections;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    public interface ITraceStrategy
    {
        bool CanTrace(object obj, TraceContext context);
        Task TraceAsync(object obj, TraceContext context);
    }

    public class TraceContext
    {
        public TraceContext(Tracer tracer, ObjectSet objectSet)
        {
            Tracer = tracer;
            ObjectSet = objectSet;
        }
        public Tracer Tracer { get; }
        public ObjectSet ObjectSet { get; }
    }
} 