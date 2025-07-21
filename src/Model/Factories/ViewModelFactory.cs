using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using SimpleInjector;
using System.Linq;

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
            var pageServiceFactory = _container.GetInstance<IPageServiceFactory>();
            var pageService = pageServiceFactory.CreatePageService(startInfo);
            return new MainVM(pageService);
        }

        public LookUpVM CreateLookUpVM()
        {
            var repoService = _container.GetInstance<IRepoService>();
            var windowService = _container.GetInstance<IWindowService>();
            return new LookUpVM(repoService, windowService);
        }

        public LookUpVM CreateLookUpVM(ObjectSet objectSet)
        {
            var vm = CreateLookUpVM();
            vm.SelectionDataObjects = objectSet.Select(x => CreateListItemVM(x)).ToList();
            return vm;
        }

        public TaskTreeVM CreateTaskTreeVM(PilotObjectHelper pilotObjectHelper)
        {
            var repoService = _container.GetInstance<IRepoService>();
            var searchService = _container.GetInstance<ICustomSearchService>();
            var windowService = _container.GetInstance<IWindowService>();
            var treeItemService = _container.GetInstance<ITreeItemService>();
            
            return new TaskTreeVM(pilotObjectHelper, repoService, searchService, windowService, treeItemService, this);
        }

        public AttrVM CreateAttrVM(PilotObjectHelper pilotObjectHelper)
        {
            var dataObjectService = _container.GetInstance<IDataObjectService>();
            return new AttrVM(pilotObjectHelper, dataObjectService);
        }

        public ListItemVM CreateListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            return new ListItemVM(pilotObjectHelper);
        }

        public SearchResVM CreateSearchResVM(IPageService pageService, PilotObjectHelper pilotObjectHelper)
        {
            var tabService = _container.GetInstance<ITabService>();
            return new SearchResVM(pageService, tabService, pilotObjectHelper);
        }
    }
}