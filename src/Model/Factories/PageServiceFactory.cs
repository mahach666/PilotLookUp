using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using SimpleInjector;

namespace PilotLookUp.Model.Factories
{
    public class PageServiceFactory : IPageServiceFactory
    {
        private readonly Container _container;

        public PageServiceFactory(Container container)
        {
            _container = container;
        }

        public IPageService CreatePageService(StartViewInfo startViewInfo)
        {
            var repoService = _container.GetInstance<IRepoService>();
            var searchService = _container.GetInstance<ICustomSearchService>();
            var tabService = _container.GetInstance<ITabService>();
            var viewModelFactory = _container.GetInstance<IViewModelFactory>();

            return new PageService(startViewInfo, repoService, searchService, tabService, viewModelFactory);
        }
    }
} 