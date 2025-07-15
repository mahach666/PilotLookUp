using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Infrastructure.Strategies
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
                var lodedHistory = await tracer.GetOrAddHistoryAsync(
                    guid,
                    async () => await objectsRepository.GetHistoryItemWithTimeout(guid, tracer.AdaptiveTimer)) as IHistoryItem;

                if (lodedHistory != null)
                {
                    context.ObjectSet.Add(tracer.PilotObjectMap.Wrap(lodedHistory));
                    return;
                }
            }

            var lodedObj = await tracer.GetOrAddObjectAsync(
                guid,
                async () => await objectsRepository.GetObjectWithTimeout(guid, tracer.AdaptiveTimer));

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
    }
} 