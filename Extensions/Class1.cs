using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PilotLookUp.Extensions
{
    internal static class ObjectsRepositoryEx
    {
        internal async static Task<IDataObject> GetObject(this IObjectsRepository objectsRepository, Guid id)
        {
            var loader = new ObjectLoader(objectsRepository);
            return  await loader.Load(id);
        }
    }
}
