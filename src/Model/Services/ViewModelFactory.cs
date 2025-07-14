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

        public ViewModelFactory(IViewModelProvider viewModelProvider, ISearchViewModelCreator searchViewModelCreator, INavigationService navigationService, IErrorHandlingService errorHandlingService, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(viewModelProvider, searchViewModelCreator, navigationService, errorHandlingService, validationService);
            _viewModelProvider = viewModelProvider;
            _searchViewModelCreator = searchViewModelCreator;
            _navigationService = navigationService;
            _errorHandlingService = errorHandlingService;
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
            var container = ServiceContainer.CreateContainer();
            var tabService = container.GetInstance<ITabService>();
            var objectSetFactory = container.GetInstance<IObjectSetFactory>();
            return new SearchResVM(_navigationService, tabService, pilotObjectHelper, objectSetFactory, _validationService);
        }
    }
} 