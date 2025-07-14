using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PilotLookUp.Extensions;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Utils.Strategies
{
    public class GuidTraceStrategy : ITraceStrategy
    {
        public bool CanTrace(object obj, TraceContext context) => obj is Guid;

        public async Task TraceAsync(object obj, TraceContext context)
        {
            var tracer = context.Tracer;
            var guid = (Guid)obj;
            var memberInfo = tracer.MemberInfo;
            var objectsRepository = tracer.ObjectsRepository;
            var userStates = objectsRepository.GetUserStates();
            var userStateMachines = objectsRepository.GetUserStateMachines();

            if (memberInfo?.Name == "HistoryItems")
            {
                var lodedHistory = await tracer.GetOrAddHistoryAsync(guid, async () => (object)await objectsRepository.GetHistoryItemWithTimeout(guid, tracer.AdaptiveTimer)) as IHistoryItem;
                if (lodedHistory != null)
                {
                    context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(lodedHistory));
                    return;
                }
            }

            var lodedObj = await tracer.GetOrAddObjectAsync(guid, async () => (object)await objectsRepository.GetObjectWithTimeout(guid, tracer.AdaptiveTimer));
            if (lodedObj != null)
            {
                context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(lodedObj));
                return;
            }

            tracer.AdaptiveTimer = 10;

            var userState = userStates.FirstOrDefault(i => i.Id == guid);
            if (userState != null)
            {
                context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(userState));
                return;
            }

            var userStateMachine = userStateMachines.FirstOrDefault(i => i.Id == guid);
            if (userStateMachine != null)
            {
                context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(userStateMachine));
                return;
            }

            context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(guid));
        }

        // Метод Trace(IPilotObjectHelper helper) удалён как устаревший и неиспользуемый
    }
} 