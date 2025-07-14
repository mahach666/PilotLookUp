using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IRepoService _repoService;
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IWindowService _windowService;
        private readonly ITreeItemService _treeItemService;
        private readonly IDataObjectService _dataObjectService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;
        private readonly IObjectSetFactory _objectSetFactory;

        public ViewModelFactory(
            IRepoService repoService,
            ICustomSearchService searchService,
            ITabService tabService,
            IWindowService windowService,
            ITreeItemService treeItemService,
            IDataObjectService dataObjectService,
            IErrorHandlingService errorHandlingService,
            IValidationService validationService,
            IObjectSetFactory objectSetFactory)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(
                repoService, searchService, tabService, windowService, treeItemService, 
                dataObjectService, errorHandlingService, validationService, objectSetFactory);
            
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _dataObjectService = dataObjectService;
            _errorHandlingService = errorHandlingService;
            _validationService = validationService;
            _objectSetFactory = objectSetFactory;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null)
        {
            var vm = new LookUpVM(_repoService, _windowService, _errorHandlingService, _validationService);
            if (dataObjects != null && dataObjects.Count > 0)
            {
                vm.SelectionDataObjects = dataObjects.Select(x => new ListItemVM(x, _validationService)).ToList();
            }
            return vm;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService)
        {
            return new SearchVM(navigationService, _searchService, _tabService, _objectSetFactory, _errorHandlingService, _validationService);
        }

        public TaskTreeVM CreateTaskTreeVM(IPilotObjectHelper selectedObject)
        {
            return new TaskTreeVM(selectedObject, _repoService, _searchService, _windowService, _treeItemService, _errorHandlingService, _validationService);
        }

        public AttrVM CreateAttrVM(IPilotObjectHelper selectedObject)
        {
            return new AttrVM(selectedObject, _dataObjectService, _errorHandlingService, _validationService);
        }

        public MainVM CreateMainVM(INavigationService navigationService)
        {
            return new MainVM(navigationService, this, _errorHandlingService, _validationService);
        }

        public SearchResVM CreateSearchResVM(INavigationService navigationService, IPilotObjectHelper pilotObjectHelper)
        {
            return new SearchResVM(navigationService, _tabService, pilotObjectHelper, _objectSetFactory, _validationService);
        }
    }
} 