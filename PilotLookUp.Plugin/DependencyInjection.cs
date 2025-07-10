using Ascon.Pilot.SDK;
using PilotLookUp.Infrastructure;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using SimpleInjector;

namespace PilotLookUp.Plugin
{
    internal static class DependencyInjection
    {
        private static Container _container;
        private static PilotLookUp.Contracts.StartViewInfo _currentStartViewInfo = new PilotLookUp.Contracts.StartViewInfo { PageName = PilotLookUp.Enums.PagesName.None };

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
            _container.Register<PilotLookUp.Contracts.StartViewInfo>(() => _currentStartViewInfo, Lifestyle.Singleton);
            // Store theme in static ThemeService for infrastructure
            ThemeService.CurrentTheme = theme;

            // Core/Infrastructure registrations already via assembly scanning? Register manually
            _container.Register<IRepoService, RepoService>();
            _container.Register<ICustomSearchService, SearchService>();
            _container.Register<ITabService, TabService>();
            _container.Register<IWindowService, WindowService>();
            _container.Register<ITreeItemService, TreeItemService>();
            _container.Register<IDataObjectService, DataObjectService>();
            _container.Register<IPageService, PageService>(Lifestyle.Transient);
            _container.Register<IClipboardService, PilotLookUp.UI.ClipboardService>(Lifestyle.Singleton);
            _container.Register<IDispatcherService, PilotLookUp.UI.DispatcherService>(Lifestyle.Singleton);

            // navigation
            _container.Register<INavigationService>(() => new NavigationService(_container), Lifestyle.Singleton);

            // UI root
            _container.Register<PilotLookUp.View.MainView>();
            _container.Register<PilotLookUp.ViewModel.MainVM>();

            return _container;
        }

        public static Container Container => _container ?? throw new System.InvalidOperationException("DI container is not initialized");

        // метод для обновления текущего StartViewInfo перед показом окна
        public static void SetStartViewInfo(PilotLookUp.Contracts.StartViewInfo info)
        {
            _currentStartViewInfo = info;
        }
    }
} 