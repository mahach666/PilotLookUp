using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Extensions
{
    static public class DataObjectEx
    {
        static public async Task<IDataObject> FindLastParent(this IDataObject dataObject, IObjectsRepository objectsRepository)
        {
            // Корень ссылается сам на себя (а данные могут содержать цикл) -
            // без учета пройденных id подъем зацикливается и вешает клиент
            var visited = new HashSet<Guid> { dataObject.Id };

            while (dataObject.ParentId != null && dataObject.ParentId != default && !visited.Contains(dataObject.ParentId))
            {
                var parent = await objectsRepository.GetObjectWithTimeout(dataObject.ParentId);

                // Родитель не загрузился за таймаут - возвращаем последнего загруженного
                if (parent == null)
                    break;

                dataObject = parent;
                visited.Add(dataObject.Id);
            }
            return dataObject;
        }
    }
}
