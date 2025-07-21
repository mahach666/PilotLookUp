using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using SimpleInjector;

namespace PilotLookUp.Model.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Container _container;

        public ViewModelFactory(Container container)
        {
            _container = container;
        }

        public MainVM CreateMainVM(StartViewInfo startInfo)
        {
            // Создаем PageService с StartViewInfo
            var pageService = CreatePageService(startInfo);
            return new MainVM(pageService);
        }

        public LookUpVM CreateLookUpVM()
        {
            var repoService = _container.GetInstance<IRepoService>();
            var windowService = _container.GetInstance<IWindowService>();
            return new LookUpVM(repoService, windowService);
        }

        // SearchVM не создается через фабрику - создается в PageService.CreatePage
        // public SearchVM CreateSearchVM() - метод удален

        private IPageService CreatePageService(StartViewInfo startInfo)
        {
            var repoService = _container.GetInstance<IRepoService>();
            var searchService = _container.GetInstance<ICustomSearchService>();
            var tabService = _container.GetInstance<ITabService>();
            var windowService = _container.GetInstance<IWindowService>();
            var treeItemService = _container.GetInstance<ITreeItemService>();
            var dataObjectService = _container.GetInstance<IDataObjectService>();

            return new PageService(startInfo, repoService, searchService, tabService, 
                windowService, treeItemService, dataObjectService);
        }
    }
} 