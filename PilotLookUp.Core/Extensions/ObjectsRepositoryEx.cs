using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Core.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Core.Extensions
{
    public static class ObjectsRepositoryEx
    {
        public static async Task<IDataObject> GetObject(this IObjectsRepository objectsRepository,
            Guid id)
        {
            return await objectsRepository.GetObjectWithTimeout(id);
        }

        public static async Task<IDataObject> GetObjectWithTimeout(this IObjectsRepository objectsRepository,
            Guid id,
            int timeoutMilliseconds = 300)
        {
            var loader = new ObjectLoader(objectsRepository);
            return await loader.LoadWithTimeout(id, timeoutMilliseconds);
        }

        // TODO: IHistoryItem not found in SDK
        /*
        public static async Task<IHistoryItem> GetHistoryItemWithTimeout(this IObjectsRepository objectsRepository,
            Guid id,
            int timeoutMilliseconds = 300)
        {
            var loader = new HistoryItemLoader(objectsRepository);
            return await loader.LoadWithTimeout(id, timeoutMilliseconds);
        }
        */

        public static async Task<object> GetObjByGuid(this IObjectsRepository objectsRepository,
            Guid guid,
            int timeoutMilliseconds = 300)
        {
            try
            {
                var obj = await objectsRepository.GetObjectWithTimeout(guid, timeoutMilliseconds);
                if (obj != null) return obj;
            }
            catch { }

            return guid;
        }
    }
}
