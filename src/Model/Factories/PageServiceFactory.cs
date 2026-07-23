using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;

namespace PilotLookUp.Model.Factories
{
    public class PageServiceFactory : IPageServiceFactory
    {
        private readonly IRepoService _repoService;
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IViewModelFactory _viewModelFactory;

        public PageServiceFactory(
            IRepoService repoService,
            ICustomSearchService searchService,
            ITabService tabService,
            IViewModelFactory viewModelFactory)
        {
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _viewModelFactory = viewModelFactory;
        }

        public IPageService CreatePageService(StartViewInfo startViewInfo)
        {
            return new PageService(startViewInfo, _repoService, _searchService, _tabService, _viewModelFactory);
        }
    }
}
