using Ascon.Pilot.SDK;
using System.Threading.Tasks;

namespace PilotLookUp.Extensions
{
    static public class DataObjectEx
    {
        static public async Task<IDataObject> FindLastParrent(this IDataObject dataObject, IObjectsRepository objectsRepository)
        {
            while (dataObject.ParentId != null && dataObject.ParentId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                dataObject = await objectsRepository.GetObjectWithTimeout(dataObject.ParentId);
            }
            return dataObject;
        }
    }
}
