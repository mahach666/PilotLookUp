using Ascon.Pilot.SDK;
using PilotLookUp.Infrastructure;
using PilotLookUp.Interfaces;
using PilotLookUp.Core.Interfaces; // for IViewModelFactory
using PilotLookUp.Contracts;
using PilotLookUp.Core.Objects;
using PilotLookUp.Infrastructure.Model.Services;
using PilotLookUp.UI;
using PilotLookUp.UI.View;
using PilotLookUp.UI.ViewModel;
using PilotLookUp.UI.Factories;
using PilotLookUp.Commands;
using SimpleInjector;
using System;

namespace PilotLookUp.Plugin
{
    internal static class DependencyInjection
    {
        private static Container _container;
        private static StartViewInfo _currentStartViewInfo = new StartViewInfo { PageName = PilotLookUp.Enums.PagesName.None };

        public static Container Initialize(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider, ThemeService.ThemeNames theme)
        {
            if (_container != null)
                return _container;

            _container = new Container();
            _container.Options.AllowOverridingRegistrations = true;
            _container.Options.EnableAutoVerification = false;

            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);

            // default view info (will be overridden runtime)
            _container.Register<StartViewInfo>(() => _currentStartViewInfo, Lifestyle.Singleton);
            // Store theme in static ThemeService for infrastructure
            ThemeService.CurrentTheme = theme;

            // Core/Infrastructure registrations already via assembly scanning? Register manually
            _container.Register<IRepoService, RepoService>();
            _container.Register<ICustomSearchService, SearchService>();
            _container.Register<ITabService, TabService>();
            _container.Register<IWindowService, WindowService>();
            _container.Register<ITreeItemService>(() => new TreeItemService(
                _container.GetInstance<IRepoService>(),
                _container.GetInstance<IViewModelFactory>()), Lifestyle.Transient);
            _container.Register<IDataObjectService, DataObjectService>();
            _container.Register<IPageService>(() => new PageService(
                _container.GetInstance<StartViewInfo>(),
                _container.GetInstance<IRepoService>(),
                _container.GetInstance<ICustomSearchService>(),
                _container.GetInstance<ITabService>(),
                _container.GetInstance<IWindowService>(),
                _container.GetInstance<ITreeItemService>(),
                _container.GetInstance<IDataObjectService>(),
                _container.GetInstance<IClipboardService>(),
                _container.GetInstance<IDispatcherService>(),
                _container,
                _container.GetInstance<IViewModelFactory>()), Lifestyle.Transient);
            _container.Register<IClipboardService, ClipboardService>(Lifestyle.Singleton);
            _container.Register<IDispatcherService, DispatcherService>(Lifestyle.Singleton);

            // UI factories
            _container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);

            // navigation
            _container.Register<INavigationService>(() => new NavigationService(_container), Lifestyle.Singleton);

            // UI root
            _container.Register<MainView>();
            _container.Register<MainVM>();

            // ViewModels
            _container.Register<LookUpVM>(Lifestyle.Transient);
            _container.Register<SearchVM>(() => new SearchVM(
                _container.GetInstance<IPageService>(),
                _container.GetInstance<ICustomSearchService>(),
                _container.GetInstance<ITabService>(),
                _container.GetInstance<IClipboardService>(),
                _container.GetInstance<IDispatcherService>(),
                _container), Lifestyle.Transient);
            _container.Register<TaskTreeVM>(Lifestyle.Transient);
            _container.Register<AttrVM>(Lifestyle.Transient);
            _container.Register<ListItemVM>(Lifestyle.Transient);
            _container.Register<SearchResVM>(Lifestyle.Transient);

            // ViewModel Factories
            _container.Register<Func<PilotObjectHelper, TaskTreeVM>>(() => 
                (pilotObjectHelper) => new TaskTreeVM(
                    pilotObjectHelper,
                    _container.GetInstance<IRepoService>(),
                    _container.GetInstance<ICustomSearchService>(),
                    _container.GetInstance<IWindowService>(),
                    _container.GetInstance<ITreeItemService>(),
                    _container.GetInstance<IClipboardService>(),
                    _container.GetInstance<IDispatcherService>()), Lifestyle.Singleton);

            _container.Register<Func<PilotObjectHelper, AttrVM>>(() => 
                (pilotObjectHelper) => new AttrVM(
                    pilotObjectHelper,
                    _container.GetInstance<IDataObjectService>(),
                    _container.GetInstance<IClipboardService>()), Lifestyle.Singleton);

            _container.Register<Func<PilotObjectHelper, ListItemVM>>(() => 
                (pilotObjectHelper) => new ListItemVM(pilotObjectHelper), Lifestyle.Singleton);

            _container.Register<Func<PilotObjectHelper, SearchResVM>>(() => 
                (pilotObjectHelper) => new SearchResVM(
                    _container.GetInstance<IPageService>(),
                    _container.GetInstance<ITabService>(),
                    pilotObjectHelper), Lifestyle.Singleton);

            return _container;
        }

        public static Container Container => _container ?? throw new System.InvalidOperationException("DI container is not initialized");

        // метод для обновления текущего StartViewInfo перед показом окна
        public static void SetStartViewInfo(StartViewInfo info)
        {
            _currentStartViewInfo = info;
        }
    }
} 