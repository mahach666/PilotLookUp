using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    public class TreeItemService : ITreeItemService
    {
        private IRepoService _lookUpModel { get; }

        public TreeItemService(IRepoService lookUpModel)
        {
            _lookUpModel = lookUpModel;
        }

        public async Task<ICastomTree> FillChild(ICastomTree lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        private async Task BuildChildNodes(ICastomTree lastParrent)
        {
            var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
            List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
            //ObjectSet newPilotObj = await new Tracer(objectsRepository, null, null).Trace(children);
            ObjectSet newPilotObj = await _lookUpModel.GetWrapedObjs(children);

            foreach (var dataObjectHelper in newPilotObj)
            {
                var childNode = new ListItemVM(dataObjectHelper);
                if (lastParrent.Children != null)
                {
                    lastParrent.Children.Add(childNode);
                }
                else
                {
                    lastParrent.Children = new ObservableCollection<ICastomTree>()
                    {
                        childNode
                    };
                }
                await BuildChildNodes(childNode); // Рекурсия для вложенных детей
            }
        }
    }
}
