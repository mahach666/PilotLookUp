using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Domain.Entities;
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
        private readonly IDataInitializationService _dataInitializationService;
        private readonly IDataFilterService _dataFilterService;
        private readonly ICopyDataService _copyDataService;
        private readonly IClipboardService _clipboardService;
        private readonly IUserNotificationService _notificationService;

        public ViewModelFactory(
            IRepoService repoService,
            ICustomSearchService searchService,
            ITabService tabService,
            IWindowService windowService,
            ITreeItemService treeItemService,
            IDataObjectService dataObjectService,
            IErrorHandlingService errorHandlingService,
            IValidationService validationService,
            IObjectSetFactory objectSetFactory,
            IDataInitializationService dataInitializationService,
            IDataFilterService dataFilterService,
            ICopyDataService copyDataService,
            IClipboardService clipboardService,
            IUserNotificationService notificationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(
                repoService, searchService, tabService, windowService, treeItemService, 
                dataObjectService, errorHandlingService, validationService, objectSetFactory,
                dataInitializationService, dataFilterService, copyDataService, clipboardService, notificationService);
            
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _dataObjectService = dataObjectService;
            _errorHandlingService = errorHandlingService;
            _validationService = validationService;
            _objectSetFactory = objectSetFactory;
            _dataInitializationService = dataInitializationService;
            _dataFilterService = dataFilterService;
            _copyDataService = copyDataService;
            _clipboardService = clipboardService;
            _notificationService = notificationService;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null)
        {
            var vm = new LookUpVM(_repoService, _windowService, _errorHandlingService, _validationService, _dataInitializationService, _dataFilterService, _copyDataService);
            if (dataObjects != null && dataObjects.Count > 0)
            {
                vm.SelectionDataObjects = dataObjects.Select(x => new ListItemVM(x, _validationService)).ToList();
            }
            return vm;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService)
        {
            return new SearchVM(navigationService, _searchService, _tabService, _objectSetFactory, _errorHandlingService, _validationService, _clipboardService);
        }

        public TaskTreeVM CreateTaskTreeVM(IPilotObjectHelper selectedObject)
        {
            return new TaskTreeVM(selectedObject, _repoService, _searchService, _windowService, _treeItemService, _errorHandlingService, _validationService, _copyDataService);
        }

        public AttrVM CreateAttrVM(IPilotObjectHelper selectedObject)
        {
            return new AttrVM(selectedObject, _dataObjectService, _errorHandlingService, _validationService, _copyDataService);
        }

        public MainVM CreateMainVM(INavigationService navigationService)
        {
            return new MainVM(navigationService, this, _errorHandlingService, _validationService, _notificationService);
        }

        public SearchResVM CreateSearchResVM(INavigationService navigationService, IPilotObjectHelper pilotObjectHelper)
        {
            return new SearchResVM(navigationService, _tabService, pilotObjectHelper, _objectSetFactory, _validationService);
        }
    }
} 