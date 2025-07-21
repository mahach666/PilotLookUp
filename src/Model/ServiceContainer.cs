using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.Model.Factories;
using PilotLookUp.View;
using PilotLookUp.View.UserControls;
using PilotLookUp.ViewModel;
using SimpleInjector;
using Container = SimpleInjector.Container;
using System;

namespace PilotLookUp.Model
{
    public class ServiceContainer
    {
        private static Container _container;

        public Container CreateContainer(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            SelectedService selectedService)
        {
            if (_container != null)
                throw new InvalidOperationException("Container is already created. Use GetContainer() to access existing container.");

            _container = new Container();

            // Регистрируем внешние зависимости
            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);
            _container.RegisterInstance(pilotDialogService);
            _container.RegisterInstance(selectedService);

            RegisterServices(_container);
            RegisterFactories(_container);
            RegisterViewModels(_container);
            RegisterViews(_container);

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
            container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            
            // PageService не регистрируется, так как требует StartViewInfo для каждого окна
            // container.Register<IPageService, PageService>(Lifestyle.Singleton);
        }

        private void RegisterFactories(Container container)
        {
            container.Register<IViewFactory, ViewFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
        }

        private void RegisterViewModels(Container container)
        {
            // MainVM не регистрируется - создается в ViewModelFactory.CreateMainVM с конкретным PageService
            // container.Register<MainVM>(Lifestyle.Transient);
            
            container.Register<LookUpVM>(Lifestyle.Transient);
            
            // SearchVM не регистрируется - создается в PageService.CreatePage
            // container.Register<SearchVM>(Lifestyle.Transient);
            
            // ViewModels ниже не регистрируются, так как требуют конкретные параметры:
            // TaskTreeVM - требует конкретный PilotObjectHelper
            // AttrVM - требует конкретный PilotObjectHelper  
            // SearchResVM - требует конкретный PilotObjectHelper
            // ListItemVM - требует конкретный PilotObjectHelper

            // VMFactory удален, поэтому делегаты больше не нужны
            // container.Register<Func<LookUpVM>>(() => () => container.GetInstance<LookUpVM>(), Lifestyle.Singleton);
            // container.Register<Func<SearchVM>>(() => () => container.GetInstance<SearchVM>(), Lifestyle.Singleton);
        }

        private void RegisterViews(Container container)
        {
            // Views обычно не регистрируются в DI контейнере
            // MainView создается в ViewFactory
            // UserControls (LookUpPage, SearchPage, TaskTreeViewPage, AttrPage) создаются через XAML
            
            // container.Register<MainView>(Lifestyle.Transient); // Создается в ViewFactory
            // UserControls не нужно регистрировать:
            // container.Register<LookUpPage>(Lifestyle.Transient);
            // container.Register<SearchPage>(Lifestyle.Transient);
            // container.Register<TaskTreeViewPage>(Lifestyle.Transient);
            // container.Register<AttrPage>(Lifestyle.Transient);
        }
    }
}
