using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Extensions
{
    internal static class ObjectsRepositoryEx
    {
        internal async static Task<IDataObject> GetObject(this IObjectsRepository objectsRepository, Guid id)
        {
            var loader = new ObjectLoader(objectsRepository);
            return  await loader.Load(id);
        }
        internal async static Task<IDataObject> GetObjectWithTimeout(this IObjectsRepository objectsRepository
            , Guid id
            , int timeoutMilliseconds = 300)
        {
            var loader = new ObjectLoader(objectsRepository);
            return await loader.LoadWithTimeout(id, timeoutMilliseconds);
        }

        internal async static Task<IHistoryItem> GetHistoryItemWithTimeout(this IObjectsRepository objectsRepository
            , Guid id
            , int timeoutMilliseconds = 300)
        {
            var loader = new HistoryItemLoader(objectsRepository);
            return await loader.LoadWithTimeout(id, timeoutMilliseconds);
        }

        internal async static Task<object> GetObjByGuid(this IObjectsRepository objectsRepository
            ,Guid guid
            , int timeoutMilliseconds = 300)
        {
            var lodedObj = await objectsRepository.GetObjectWithTimeout(guid, timeoutMilliseconds);
            if (lodedObj != null)
            {
                return lodedObj;
            }

            var userStates = objectsRepository.GetUserStates();
            var userState = userStates.FirstOrDefault(i => i.Id == guid);
            if (userState != null)
            {
                return userState;
            }

            var userStateMachines = objectsRepository.GetUserStateMachines();
            var userStateMachine = userStateMachines.FirstOrDefault(i => i.Id == guid);
            if (userStateMachine != null)
            {
                return userStateMachines;
            }

            return null;
        }
    }
}
