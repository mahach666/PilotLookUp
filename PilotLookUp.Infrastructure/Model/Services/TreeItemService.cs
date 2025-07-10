using Ascon.Pilot.SDK;
using System;
using PilotLookUp.Core.Interfaces; // for IViewModelFactory
using PilotLookUp.Interfaces;
using PilotLookUp.Contracts;
using PilotLookUp.Core.Objects;
using PilotLookUp.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Infrastructure.Model.Services
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

        public async Task<ICustomTree> FillChild(ICustomTree lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        private async Task BuildChildNodes(ICustomTree lastParrent)
        {
            var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
            List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
            ObjectSet newPilotObj = await _lookUpModel.GetWrapedObjs(children);

            foreach (var dataObjectHelper in newPilotObj)
            {
                // Создание узлов делегируется DI/UI-слою через фабрики
                var childNode = CreateTreeNode(dataObjectHelper);
                if (lastParrent.Children != null)
                {
                    lastParrent.Children.Add(childNode);
                }
                else
                {
                    lastParrent.Children = new ObservableCollection<ICustomTree>()
                    {
                        childNode
                    };
                }
                await BuildChildNodes(childNode); // Рекурсия для вложенных детей
            }
        }

        // Фабрика для создания ICustomTree
        private ICustomTree CreateTreeNode(PilotObjectHelper helper)
        {
            return (ICustomTree)_viewModelFactory.CreateListItemVM(helper);
        }
    }
}
