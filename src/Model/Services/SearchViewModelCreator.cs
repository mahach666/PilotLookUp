using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Services
{
    public class SearchViewModelCreator : ISearchViewModelCreator
    {
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IObjectSetFactory _objectSetFactory;

        public SearchViewModelCreator(
            ICustomSearchService searchService,
            ITabService tabService,
            IObjectSetFactory objectSetFactory)
        {
            _searchService = searchService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService)
        {
            return new SearchVM(navigationService, _searchService, _tabService, _objectSetFactory);
        }
    }
} 