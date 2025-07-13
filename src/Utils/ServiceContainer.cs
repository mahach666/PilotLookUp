using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using SimpleInjector;

namespace PilotLookUp.Utils
{
    public static class ServiceContainer
    {
        private static IObjectsRepository _globalObjectsRepository;
        private static ITabServiceProvider _globalTabServiceProvider;

        public static Container CreateContainer(IObjectsRepository objectsRepository = null, ITabServiceProvider tabServiceProvider = null, Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
            // Создаем новый контейнер для каждого окна
            var container = new Container();
            
            // Настраиваем базовые сервисы
            ConfigureBaseServices(container, theme);
            
            // Регистрируем внешние сервисы (используем переданные или глобальные)
            var repo = objectsRepository ?? _globalObjectsRepository;
            var tabProvider = tabServiceProvider ?? _globalTabServiceProvider;
            
            if (repo != null)
                container.RegisterInstance(repo);
            if (tabProvider != null)
                container.RegisterInstance(tabProvider);
            
            // Валидируем контейнер
            container.Verify();
            
            return container;
        }

        private static void ConfigureBaseServices(Container container, Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
            // Регистрируем сервисы
            container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
            container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
            container.Register<ITabService, TabService>(Lifestyle.Singleton);
            container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            
            // Регистрируем новые сервисы
            container.Register<ISearchViewModelCreator, SearchViewModelCreator>(Lifestyle.Singleton);
            container.Register<IViewModelProvider, ViewModelProvider>(Lifestyle.Singleton);
            container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            
            // Регистрируем новые сервисы для разделения ответственности
            container.Register<IObjectMappingService, ObjectMappingService>(Lifestyle.Singleton);
            container.Register<ISelectionService, SelectionService>(Lifestyle.Singleton);
            container.Register<IMenuService, MenuService>(Lifestyle.Singleton);
            
            // Регистрируем ThemeService
            var themeToUse = theme ?? App.Theme;
            container.Register<IThemeService>(() => new ThemeService(themeToUse), Lifestyle.Singleton);
            
            // Регистрируем ViewModels (только те, которые не требуют параметров)
            container.Register<LookUpVM>(Lifestyle.Transient);
            // SearchVM, MainVM, TaskTreeVM и AttrVM создаются через фабрику
            
            // Регистрируем Views
            container.Register<MainView>(() =>
                System.Windows.Application.Current.Dispatcher.Invoke(() => new MainView()), Lifestyle.Transient);
        }

        public static void SetGlobalServices(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            _globalObjectsRepository = objectsRepository;
            _globalTabServiceProvider = tabServiceProvider;
        }
    }
} 