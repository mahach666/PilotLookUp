using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Factories;
using PilotLookUp.Model.Services;
using PilotLookUp.ViewModel;
using SimpleInjector;
using System;
using Container = SimpleInjector.Container;

namespace PilotLookUp.Model
{
    public class ServiceContainer
    {
        private static Container _container;

        public Container CreateContainer(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService)
        {
            if (_container != null)
                throw new InvalidOperationException("!!!");

            _container = new Container();

            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);
            _container.RegisterInstance(pilotDialogService);

            RegisterServices(_container);
            RegisterFactories(_container);
            RegisterViewModels(_container);

            _container.Verify();
            return _container;
        }

        public static Container GetContainer()
        {
            if (_container == null)
                throw new InvalidOperationException("Container is not created yet. Call CreateContainer() first.");
            
            return _container;
        }

        private void RegisterServices(Container container)
        {
            container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
            container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
            container.Register<ITabService, TabService>(Lifestyle.Singleton);
            container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            container.Register<ISelectedService, SelectedService>(Lifestyle.Singleton);
            
            // TreeItemService регистрируется с делегатом для избежания циклической зависимости с IViewModelFactory
            container.Register<ITreeItemService>(() => 
            {
                var repoService = container.GetInstance<IRepoService>();
                var viewModelFactory = container.GetInstance<IViewModelFactory>();
                return new TreeItemService(repoService, viewModelFactory);
            }, Lifestyle.Singleton);
        }

        private void RegisterFactories(Container container)
        {
            container.Register<IViewFactory, ViewFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            container.Register<IPageServiceFactory, PageServiceFactory>(Lifestyle.Singleton);
        }

        private void RegisterViewModels(Container container)
        {            
            container.Register<LookUpVM>(Lifestyle.Transient);
        }
    }
}
