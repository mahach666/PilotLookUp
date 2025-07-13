using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Services
{
    public class SearchViewModelCreator : ISearchViewModelCreator
    {
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;

        public SearchViewModelCreator(
            ICustomSearchService searchService,
            ITabService tabService)
        {
            _searchService = searchService;
            _tabService = tabService;
        }

        public SearchVM CreateSearchVM(INavigationService navigationService)
        {
            return new SearchVM(navigationService, _searchService, _tabService);
        }
    }
} 