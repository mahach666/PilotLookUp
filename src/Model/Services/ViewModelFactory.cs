using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IViewModelProvider _viewModelProvider;
        private readonly ISearchViewModelCreator _searchViewModelCreator;
        private readonly INavigationService _navigationService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;
        private readonly ITabService _tabService;
        private readonly IObjectSetFactory _objectSetFactory;

        public ViewModelFactory(
            IViewModelProvider viewModelProvider, 
            ISearchViewModelCreator searchViewModelCreator, 
            INavigationService navigationService, 
            IErrorHandlingService errorHandlingService, 
            IValidationService validationService,
            ITabService tabService,
            IObjectSetFactory objectSetFactory)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(viewModelProvider,
                searchViewModelCreator,
                navigationService,
                errorHandlingService,
                validationService,
                tabService,
                objectSetFactory);
            _viewModelProvider = viewModelProvider;
            _searchViewModelCreator = searchViewModelCreator;
            _navigationService = navigationService;
            _errorHandlingService = errorHandlingService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null, IErrorHandlingService errorHandlingService = null)
        {
            return _viewModelProvider.CreateLookUpVM(dataObjects, errorHandlingService ?? _errorHandlingService);
        }

        public SearchVM CreateSearchVM(IErrorHandlingService errorHandlingService = null)
        {
            return _searchViewModelCreator.CreateSearchVM(_navigationService, errorHandlingService ?? _errorHandlingService);
        }

        public TaskTreeVM CreateTaskTreeVM(IPilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null)
        {
            return _viewModelProvider.CreateTaskTreeVM(selectedObject, errorHandlingService ?? _errorHandlingService);
        }

        public AttrVM CreateAttrVM(IPilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null)
        {
            return _viewModelProvider.CreateAttrVM(selectedObject, errorHandlingService ?? _errorHandlingService);
        }

        public MainVM CreateMainVM(IErrorHandlingService errorHandlingService = null)
        {
            return _viewModelProvider.CreateMainVM(_navigationService, this, errorHandlingService ?? _errorHandlingService);
        }

        public SearchResVM CreateSearchResVM(IPilotObjectHelper pilotObjectHelper)
        {
            return new SearchResVM(_navigationService, _tabService, pilotObjectHelper, _objectSetFactory, _validationService);
        }
    }
} 