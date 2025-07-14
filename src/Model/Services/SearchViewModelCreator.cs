using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Services
{
    public class SearchViewModelCreator : ISearchViewModelCreator
    {
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly IErrorHandlingService _errorHandlingService;

        public SearchViewModelCreator(
            ICustomSearchService searchService,
            ITabService tabService,
            IObjectSetFactory objectSetFactory,
            IErrorHandlingService errorHandlingService)
        {
            _searchService = searchService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
            _errorHandlingService = errorHandlingService;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService, IErrorHandlingService errorHandlingService = null)
        {
            return new SearchVM(navigationService, _searchService, _tabService, _objectSetFactory, errorHandlingService ?? _errorHandlingService);
        }
    }
} 