using System;
using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using SimpleInjector;
using PilotLookUp.Domain.UseCases;

namespace PilotLookUp.Utils
{
    public static class ServiceContainer
    {
        public static Container CreateContainer(IObjectsRepository objectsRepository = null,
            ITabServiceProvider tabServiceProvider = null,
            Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
            // Создаем новый контейнер для каждого окна
            var container = new Container();
            
            // Регистрируем внешние сервисы (обязательные)
            if (objectsRepository == null)
                throw new ArgumentNullException(nameof(objectsRepository), "IObjectsRepository обязателен!");
            if (tabServiceProvider == null)
                throw new ArgumentNullException(nameof(tabServiceProvider), "ITabServiceProvider обязателен!");
            if (theme == null)
                throw new ArgumentNullException(nameof(theme), "Theme обязателен!");
                
            container.RegisterInstance(objectsRepository);
            container.RegisterInstance(tabServiceProvider);
            
            // Регистрируем ThemeProvider
            container.RegisterInstance<IThemeProvider>(new ThemeProvider(theme.Value));
            
            // Настраиваем базовые сервисы
            ConfigureBaseServices(container, theme.Value);
            
            // Валидируем контейнер
            container.Verify();
            
            return container;
        }

        private static void ConfigureBaseServices(Container container,
            Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
            // Регистрируем основные сервисы
            container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
            container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
            container.Register<ITabService, TabService>(Lifestyle.Singleton);
            container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            container.Register<IValidationService, ValidationService>(Lifestyle.Singleton);
            container.Register<IObjectMappingService, ObjectMappingService>(Lifestyle.Singleton);
            container.Register<ISelectionService, SelectionService>(Lifestyle.Singleton);
            container.Register<IErrorHandlingService, ErrorHandlingService>(Lifestyle.Singleton);
            container.Register<IPilotObjectHelperFactory, PilotObjectHelperFactory>(Lifestyle.Singleton);
            container.Register<IWindowFactory, WindowFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            container.Register<IObjectSetFactory, ObjectSetFactory>(Lifestyle.Singleton);
            container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            container.Register<IMenuService, MenuService>(Lifestyle.Singleton);
            container.Register<IThemeService, ThemeService>(Lifestyle.Singleton);
            container.Register<TaskTreeBuilderService>(Lifestyle.Singleton);
            
            // Регистрируем новые сервисы для разделения ответственности
            container.Register<IClipboardService, ClipboardService>(Lifestyle.Singleton);
            container.Register<IUserNotificationService, UserNotificationService>(Lifestyle.Singleton);
            container.Register<IDataFilterService, DataFilterService>(Lifestyle.Singleton);
            container.Register<IDataInitializationService, DataInitializationService>(Lifestyle.Singleton);
            container.Register<ICopyDataService, CopyDataService>(Lifestyle.Singleton);
            container.Register<IViewDirectorService, ViewDirectorService>(Lifestyle.Singleton);
            
            // Регистрируем NavigationService
            container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);
            
            // Регистрируем Views
            container.Register<MainView>(() =>
                System.Windows.Application.Current.Dispatcher.Invoke(() => new MainView()), Lifestyle.Transient);
        }

        // Удаляю SetGlobalServices и все статические поля
    }
} 