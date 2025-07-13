using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class ViewModelProvider : IViewModelProvider
    {
        private readonly IRepoService _repoService;
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IWindowService _windowService;
        private readonly ITreeItemService _treeItemService;
        private readonly IDataObjectService _dataObjectService;

        public ViewModelProvider(
            IRepoService repoService,
            ICustomSearchService searchService,
            ITabService tabService,
            IWindowService windowService,
            ITreeItemService treeItemService,
            IDataObjectService dataObjectService)
        {
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _dataObjectService = dataObjectService;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null)
        {
            var vm = new LookUpVM(_repoService, _windowService);
            if (dataObjects != null && dataObjects.Count > 0)
            {
                vm.SelectionDataObjects = dataObjects.Select(x => new ListItemVM(x)).ToList();
            }
            return vm;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService)
        {
            // Этот метод будет делегирован в SearchViewModelCreator
            throw new System.NotImplementedException("Use ISearchViewModelCreator instead");
        }

        public TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject)
        {
            return new TaskTreeVM(selectedObject, _repoService, _searchService, _windowService, _treeItemService);
        }

        public AttrVM CreateAttrVM(PilotObjectHelper selectedObject)
        {
            return new AttrVM(selectedObject, _dataObjectService);
        }

        public MainVM CreateMainVM(INavigationService navigationService, IViewModelFactory viewModelFactory)
        {
            return new MainVM(navigationService, viewModelFactory);
        }
    }
} 