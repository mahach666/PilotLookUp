using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PilotLookUp.Model.Services
{
    public class TreeItemService : ITreeItemService
    {
        private IRepoService _lookUpModel { get; }
        private readonly IValidationService _validationService;
        private readonly ILogger _logger;

        public TreeItemService(IRepoService lookUpModel, IValidationService validationService, ILogger logger)
        {
            _lookUpModel = lookUpModel;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<ICustomTree> FillChild(ICustomTree lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        private async Task BuildChildNodes(ICustomTree lastParrent)
        {
            var sad = lastParrent.PilotObjectHelper.LookUpObject as Ascon.Pilot.SDK.IDataObject;
            List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
            //ObjectSet newPilotObj = await new Tracer(objectsRepository, null, null).Trace(children);
            ObjectSet newPilotObj = await _lookUpModel.GetWrapedObjs(children);

            foreach (var dataObjectHelper in newPilotObj)
            {
                var childNode = new ListItemVM(dataObjectHelper, _validationService, _logger);
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
    }

    public class TaskTreeBuilderService
    {
        private readonly ICustomSearchService _searchService;
        private readonly ITreeItemService _treeItemService;
        private readonly IValidationService _validationService;
        private readonly ILogger _logger;

        public TaskTreeBuilderService(
            ICustomSearchService searchService,
            ITreeItemService treeItemService,
            IValidationService validationService,
            ILogger logger)
        {
            _searchService = searchService;
            _treeItemService = treeItemService;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<(ObservableCollection<ICustomTree> nodes, Visibility revokedTaskVisible, IPilotObjectHelper lastParent)> BuildTaskTreeAsync(
            IPilotObjectHelper objectHelper,
            bool revokedTask)
        {
            bool isTask = false;
            var isTaskProp = objectHelper.GetType().GetProperty("IsTask");
            if (isTaskProp != null)
            {
                isTask = (bool)isTaskProp.GetValue(objectHelper);
            }
            else return (new ObservableCollection<ICustomTree>(), Visibility.Hidden, null);

            if (isTask && objectHelper.LookUpObject is Ascon.Pilot.SDK.IDataObject dataObject)
            {
                var lastParent = await _searchService.GetLastParent(dataObject);
                ICustomTree rootNode = new ListItemVM(lastParent, _validationService, _logger);
                rootNode = await _treeItemService.FillChild(rootNode);
                return (new ObservableCollection<ICustomTree> { rootNode }, Visibility.Hidden, lastParent);
            }
            else
            {
                var treeItems = new ObservableCollection<ICustomTree>();
                ObjectSet allLastParent = await _searchService.GetBaseParentsOfRelations(objectHelper, revokedTask);
                foreach (IPilotObjectHelper item in allLastParent)
                {
                    ICustomTree rootNode = new ListItemVM(item, _validationService, _logger);
                    rootNode = await _treeItemService.FillChild(rootNode);
                    treeItems.Add(rootNode);
                }
                return (treeItems, Visibility.Visible, null);
            }
        }
    }
}
