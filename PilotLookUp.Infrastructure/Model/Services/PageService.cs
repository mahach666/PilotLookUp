using Ascon.Pilot.SDK;
using PilotLookUp.Core.Interfaces; // for IViewModelFactory
using PilotLookUp.Interfaces;
using PilotLookUp.Contracts;
using PilotLookUp.Core.Objects;
using PilotLookUp.Enums;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Infrastructure.Model.Services
{
    public class PageService : IPageService
    {
        private IRepoService _repoService { get; }
        private ICustomSearchService _searchService { get; }
        private ITabService _tabService { get; }
        private IWindowService _windowService { get; }
        private ITreeItemService _treeItemService { get; }
        private IDataObjectService _dataObjectService { get; }
        private IClipboardService _clipboardService { get; }
        private IDispatcherService _dispatcherService { get; }
        private Container _container { get; }
        private IViewModelFactory _viewModelFactory { get; }

        public PageService(
             StartViewInfo startViewInfo,
             IRepoService repoService,
             ICustomSearchService searchService,
             ITabService tabService,
             IWindowService windowService,
             ITreeItemService treeItemService,
             IDataObjectService dataObjectService,
             IClipboardService clipboardService,
             IDispatcherService dispatcherService,
             Container container,
             IViewModelFactory viewModelFactory)
        {
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _dataObjectService = dataObjectService;
            _clipboardService = clipboardService;
            _dispatcherService = dispatcherService;
            _container = container;
            _viewModelFactory = viewModelFactory;
            _controlsHolder = new List<IPage>();

            if (startViewInfo.PageName != PagesName.None)
                CreatePage(startViewInfo.PageName, startViewInfo.SelectedObject);
        }

        public event Action<IPage> PageChanged;

        private IPage _activePage { get; set; }
        public IPage ActivePage { get { return _activePage; } }

        private List<IPage> _controlsHolder { get; }

        public void GoToPage(PagesName pageName)
        {
            if (_controlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
            {
                _activePage = _controlsHolder.FirstOrDefault(i => i.GetName() == pageName);
                PageChanged?.Invoke(_activePage);
            }
            else
            {
                CreatePage(pageName);
            }
        }
        public void CreatePage(PagesName pageName, ObjectSet dataObj = null)
        {
            // Создание страниц делегируется DI/UI-слою через фабрики
            // Здесь только вызов фабрик через DI
            switch (pageName)
            {
                case PagesName.LookUpPage:
                    var lookVM = (IPage)_viewModelFactory.CreateLookUpVM();
                    // Передаём список выбранных объектов, если он есть
                    if (dataObj != null && dataObj.Any() && lookVM is ISelectionReceiver receiver)
                    {
                        receiver.SetInitialSelection(dataObj);
                    }
                    AddPage(lookVM);
                    GoToPage(pageName);
                    break;
                case PagesName.DBPage:
                    var dbVM = (IPage)_viewModelFactory.CreateLookUpVM();
                    AddPage(dbVM);
                    GoToPage(PagesName.LookUpPage);
                    break;
                case PagesName.SearchPage:
                    var searchVM = (IPage)_viewModelFactory.CreateSearchVM();
                    AddPage(searchVM);
                    GoToPage(pageName);
                    break;
                case PagesName.TaskTree:
                    // TaskTree требует PilotObjectHelper, который должен быть передан через dataObj
                    if (dataObj?.Count > 0)
                    {
                        var taskTreeVM = (IPage)_viewModelFactory.CreateTaskTreeVM(dataObj.First());
                        AddPage(taskTreeVM);
                        GoToPage(pageName);
                    }
                    break;
                case PagesName.AttrPage:
                    // AttrPage требует PilotObjectHelper, который должен быть передан через dataObj
                    if (dataObj?.Count > 0)
                    {
                        var attrVM = (IPage)_viewModelFactory.CreateAttrVM(dataObj.First());
                        AddPage(attrVM);
                        GoToPage(pageName);
                    }
                    break;
            }
        }

        private void AddPage(IPage item)
        {
            _controlsHolder.RemoveAll(obj => obj.GetName() == item.GetName());
            _controlsHolder.Add(item);
        }
    }
}
