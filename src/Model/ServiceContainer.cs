using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using SimpleInjector;
using Container = SimpleInjector.Container;

namespace PilotLookUp.Model
{
    public class ServiceContainer
    {
        public Container CreateContainer(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            SelectedService selectedService)
        {
            var container = new Container();

            container.RegisterInstance(objectsRepository);
            container.RegisterInstance(tabServiceProvider);
            container.RegisterInstance(pilotDialogService);
            container.RegisterInstance(selectedService);

            RegisterBase(container);

            return container;
        }

        private void RegisterBase(Container container)
        {
            container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
            container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
            container.Register<ITabService, TabService>(Lifestyle.Singleton);
            container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            container.Register<IPageService, PageService>(Lifestyle.Singleton);

            container.Register<MainVM>(Lifestyle.Transient);
            container.Register<MainView>(Lifestyle.Transient);
        }

    }
}
