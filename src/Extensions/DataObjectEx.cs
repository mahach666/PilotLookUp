using Ascon.Pilot.SDK;
using System.Threading.Tasks;

namespace PilotLookUp.Extensions
{
    static public class DataObjectEx
    {
        static public async Task<IDataObject> FindLastParrent(this IDataObject dataObject, IObjectsRepository objectsRepository)
        {
            while (dataObject.ParentId != null && dataObject.ParentId != default)
            {
                dataObject = await objectsRepository.GetObjectWithTimeout(dataObject.ParentId);
            }
            return dataObject;
        }
    }
}
