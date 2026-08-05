using Ascon.Pilot.SDK;
using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System;
using System.Linq;

namespace PilotLookUp.Model.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IRepoService _repoService;
        private readonly IFileService _fileService;
        private readonly ICustomSearchService _searchService;
        private readonly IDataObjectService _dataObjectService;
        private readonly ITabService _tabService;
        private readonly IObjectsRepository _objectsRepository;
        private readonly IClientEventService _clientEventService;
        private readonly IAdvancedSearchService _advancedSearchService;

        // Lazy разрывает циклы: ViewFactory/TreeItemService/PageServiceFactory
        // сами зависят от IViewModelFactory
        private readonly Lazy<IViewFactory> _viewFactory;
        private readonly Lazy<ITreeItemService> _treeItemService;
        private readonly Lazy<IPageServiceFactory> _pageServiceFactory;

        public ViewModelFactory(
            IRepoService repoService,
            IFileService fileService,
            ICustomSearchService searchService,
            IDataObjectService dataObjectService,
            ITabService tabService,
            IObjectsRepository objectsRepository,
            IClientEventService clientEventService,
            IAdvancedSearchService advancedSearchService,
            Lazy<IViewFactory> viewFactory,
            Lazy<ITreeItemService> treeItemService,
            Lazy<IPageServiceFactory> pageServiceFactory)
        {
            _repoService = repoService;
            _fileService = fileService;
            _searchService = searchService;
            _dataObjectService = dataObjectService;
            _tabService = tabService;
            _objectsRepository = objectsRepository;
            _clientEventService = clientEventService;
            _advancedSearchService = advancedSearchService;
            _viewFactory = viewFactory;
            _treeItemService = treeItemService;
            _pageServiceFactory = pageServiceFactory;
        }

        public MainVM CreateMainVM(StartViewInfo startInfo)
        {
            var pageService = _pageServiceFactory.Value.CreatePageService(startInfo);
            return new MainVM(pageService);
        }

        public LookUpVM CreateLookUpVM()
        {
            return new LookUpVM(_repoService, _viewFactory.Value, _fileService);
        }

        public LookUpVM CreateLookUpVM(ObjectSet objectSet)
        {
            var vm = CreateLookUpVM();
            vm.SelectionDataObjects = objectSet.Select(x => CreateListItemVM(x)).ToList();
            return vm;
        }

        public TaskTreeVM CreateTaskTreeVM(PilotObjectHelper pilotObjectHelper)
        {
            return new TaskTreeVM(pilotObjectHelper, _repoService, _searchService, _viewFactory.Value, _treeItemService.Value, this);
        }

        public AttrVM CreateAttrVM(PilotObjectHelper pilotObjectHelper)
        {
            return new AttrVM(pilotObjectHelper, _dataObjectService, _repoService, _viewFactory.Value, _objectsRepository);
        }

        public ListItemVM CreateListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            return new ListItemVM(pilotObjectHelper);
        }

        public SearchResVM CreateSearchResVM(IPageService pageService, PilotObjectHelper pilotObjectHelper)
        {
            return new SearchResVM(pageService, _tabService, pilotObjectHelper);
        }

        public EventMonitorVM CreateEventMonitorVM()
        {
            return new EventMonitorVM(_objectsRepository, _repoService, _viewFactory.Value, _clientEventService);
        }

        public AdvancedSearchVM CreateAdvancedSearchVM(Action<ObjectSet> publishResults)
        {
            return new AdvancedSearchVM(_advancedSearchService, _repoService, publishResults);
        }
    }
}
