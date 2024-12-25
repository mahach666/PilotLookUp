using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System;
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
        internal async static Task<IDataObject> GetObjectWithTimeout(this IObjectsRepository objectsRepository, Guid id, int timeoutMilliseconds = 300)
        {
            var loader = new ObjectLoader(objectsRepository);
            return await loader.LoadWithTimeout(id);
        }
    }
}
