using Ascon.Pilot.SDK;
using System.Threading.Tasks;

namespace PilotLookUp.Extensions
{
    static public class DataObjectEx
    {
        static public async Task<IDataObject> FindLastParent(this IDataObject dataObject, IObjectsRepository objectsRepository)
        {
            while (dataObject.ParentId != null && dataObject.ParentId != default)
            {
                var parent = await objectsRepository.GetObjectWithTimeout(dataObject.ParentId);

                // Родитель не загрузился за таймаут - возвращаем последнего загруженного
                if (parent == null)
                    break;

                dataObject = parent;
            }
            return dataObject;
        }
    }
}
