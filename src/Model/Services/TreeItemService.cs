using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
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
        private IViewModelFactory _viewModelFactory { get; }

        public TreeItemService(IRepoService lookUpModel, IViewModelFactory viewModelFactory)
        {
            _lookUpModel = lookUpModel;
            _viewModelFactory = viewModelFactory;
        }

        public async Task<ICustomTree> FillChild(ICustomTree lastParent)
        {
            await BuildChildNodes(lastParent);
            return lastParent;
        }

        private async Task BuildChildNodes(ICustomTree lastParent)
        {
            // Узел без объекта БД (например, ярлык или спецтип) - потомков нет
            if (!(lastParent.PilotObjectHelper?.LookUpObject is IDataObject dataObject))
                return;

            List<Guid> children = dataObject.Children.ToList();
            ObjectSet newPilotObj = await _lookUpModel.GetWrapedObjs(children);

            foreach (var dataObjectHelper in newPilotObj)
            {
                var childNode = _viewModelFactory.CreateListItemVM(dataObjectHelper);
                if (lastParent.Children != null)
                {
                    lastParent.Children.Add(childNode);
                }
                else
                {
                    lastParent.Children = new ObservableCollection<ICustomTree>()
                    {
                        childNode
                    };
                }
                await BuildChildNodes(childNode);
            }
        }
    }
}
