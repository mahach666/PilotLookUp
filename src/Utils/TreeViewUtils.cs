using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    //static class TreeViewUtils
    //{
    //    public static async Task<ICastomTree> FillChild(IObjectsRepository objectsRepository, ICastomTree lastParrent)
    //    {
    //        await BuildChildNodes(objectsRepository, lastParrent);
    //        return lastParrent;
    //    }

    //    private static async Task BuildChildNodes(IObjectsRepository objectsRepository, ICastomTree lastParrent)
    //    {
    //        var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
    //        List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
    //        ObjectSet newPilotObj = await new Tracer(objectsRepository, null, null).Trace(children);
    //        foreach (var dataObjectHelper in newPilotObj)
    //        {
    //            var childNode = new ListItemVM(dataObjectHelper);
    //            if (lastParrent.Children != null)
    //            {
    //                lastParrent.Children.Add(childNode);
    //            }
    //            else
    //            {
    //                lastParrent.Children = new ObservableCollection<ICastomTree>()
    //                {
    //                    childNode
    //                };
    //            }
    //            await BuildChildNodes(objectsRepository, childNode); // Рекурсия для вложенных детей
    //        }
    //    }
    //}
}
