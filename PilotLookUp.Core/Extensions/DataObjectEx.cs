using Ascon.Pilot.SDK;
using PilotLookUp.Core.Extensions;
using System;
using System.Threading.Tasks;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Core.Extensions
{
    static public class DataObjectEx
    {
        static public async Task<IDataObject> FindLastParrent(this IDataObject dataObject, IObjectsRepository objectsRepository)
        {
            if (dataObject.ParentId == Guid.Empty)
                return dataObject;
            else
                return await objectsRepository.GetObjectWithTimeout(dataObject.ParentId);
        }
    }
}
