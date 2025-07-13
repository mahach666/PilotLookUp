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

        public ViewModelFactory(IViewModelProvider viewModelProvider, ISearchViewModelCreator searchViewModelCreator, INavigationService navigationService)
        {
            _viewModelProvider = viewModelProvider;
            _searchViewModelCreator = searchViewModelCreator;
            _navigationService = navigationService;
        }

        public LookUpVM CreateLookUpVM(ObjectSet dataObjects = null)
        {
            return _viewModelProvider.CreateLookUpVM(dataObjects);
        }

        public SearchVM CreateSearchVM()
        {
            return _searchViewModelCreator.CreateSearchVM(_navigationService);
        }

        public TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject)
        {
            return _viewModelProvider.CreateTaskTreeVM(selectedObject);
        }

        public AttrVM CreateAttrVM(PilotObjectHelper selectedObject)
        {
            return _viewModelProvider.CreateAttrVM(selectedObject);
        }

        public MainVM CreateMainVM()
        {
            return _viewModelProvider.CreateMainVM(_navigationService, this);
        }

        public SearchResVM CreateSearchResVM(PilotObjectHelper pilotObjectHelper)
        {
            var container = ServiceContainer.CreateContainer();
            var tabService = container.GetInstance<ITabService>();
            var objectSetFactory = container.GetInstance<IObjectSetFactory>();
            return new SearchResVM(_navigationService, tabService, pilotObjectHelper, objectSetFactory);
        }
    }
} 