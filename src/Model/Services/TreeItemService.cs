﻿using Ascon.Pilot.SDK;
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

        public async Task<ICustomTree> FillChild(ICustomTree lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        private async Task BuildChildNodes(ICustomTree lastParrent)
        {
            var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
            List<Guid> children = sad.Children.ToList();  
            ObjectSet newPilotObj = await _lookUpModel.GetWrapedObjs(children);

            foreach (var dataObjectHelper in newPilotObj)
            {
                var childNode = _viewModelFactory.CreateListItemVM(dataObjectHelper);
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
                await BuildChildNodes(childNode);
            }
        }
    }
}
