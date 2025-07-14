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
        private readonly IErrorHandlingService _errorHandlingService;

        public ViewModelProvider(
            IRepoService repoService,
            ICustomSearchService searchService,
            ITabService tabService,
            IWindowService windowService,
            ITreeItemService treeItemService,
            IDataObjectService dataObjectService,
            IErrorHandlingService errorHandlingService)
        {
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _dataObjectService = dataObjectService;
            _errorHandlingService = errorHandlingService;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null, IErrorHandlingService errorHandlingService = null)
        {
            var vm = new LookUpVM(_repoService, _windowService, errorHandlingService ?? _errorHandlingService);
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

        public TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null)
        {
            return new TaskTreeVM(selectedObject, _repoService, _searchService, _windowService, _treeItemService, errorHandlingService ?? _errorHandlingService);
        }

        public AttrVM CreateAttrVM(PilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null)
        {
            return new AttrVM(selectedObject, _dataObjectService, errorHandlingService ?? _errorHandlingService);
        }

        public MainVM CreateMainVM(INavigationService navigationService, IViewModelFactory viewModelFactory, IErrorHandlingService errorHandlingService = null)
        {
            return new MainVM(navigationService, viewModelFactory, errorHandlingService ?? _errorHandlingService);
        }
    }
} 