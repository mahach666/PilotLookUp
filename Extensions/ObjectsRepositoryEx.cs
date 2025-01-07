using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System;
using System.Collections.Generic;
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
        internal async static Task<List<IDataObject>> GetChildrensWithTimeout(this IObjectsRepository objectsRepository, IDataObject currentObject, int timeoutMilliseconds = 300)
        {
            var loader = new ObjectLoader(objectsRepository);
            var childrensId = currentObject.Children;
            List<IDataObject> childrens = new List<IDataObject>();
            foreach (var child in childrensId)
            {
                var childObj = await loader.LoadWithTimeout(child);
                childrens.Add(childObj);
            }
            return childrens;
        }
    }
}
