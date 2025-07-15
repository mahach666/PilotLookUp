using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Domain.UseCases;
using PilotLookUp.Model.Services;
using PilotLookUp.View;
using SimpleInjector;
using System;

namespace PilotLookUp.Infrastructure
{
    public static class ServiceContainer
    {
        public static Container CreateContainer(IObjectsRepository objectsRepository = null,
            ITabServiceProvider tabServiceProvider = null,
            Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
            var container = new Container();

            if (objectsRepository == null)
                throw new ArgumentNullException(nameof(objectsRepository), "IObjectsRepository обязателен!");
            if (tabServiceProvider == null)
                throw new ArgumentNullException(nameof(tabServiceProvider), "ITabServiceProvider обязателен!");
            if (theme == null)
                throw new ArgumentNullException(nameof(theme), "Theme обязателен!");

            container.RegisterInstance(objectsRepository);
            container.RegisterInstance(tabServiceProvider);

            container.RegisterInstance<IThemeProvider>(new ThemeProvider(theme.Value));

            container.Register<ILogger, SimpleLogger>(Lifestyle.Singleton);

            ConfigureBaseServices(container, theme.Value);

            container.Verify();

            return container;
        }

        private static void ConfigureBaseServices(Container container,
            Ascon.Pilot.Themes.ThemeNames? theme = null)
        {
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
            container.Register<TaskTreeBuilderService, TaskTreeBuilderService>(Lifestyle.Singleton);

            container.Register<IClipboardService, ClipboardService>(Lifestyle.Singleton);
            container.Register<IUserNotificationService, UserNotificationService>(Lifestyle.Singleton);
            container.Register<IDataFilterService, DataFilterService>(Lifestyle.Singleton);
            container.Register<IDataInitializationService, DataInitializationService>(Lifestyle.Singleton);
            container.Register<ICopyDataService, CopyDataService>(Lifestyle.Singleton);
            container.Register<IViewDirectorService, ViewDirectorService>(Lifestyle.Singleton);

            container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);

            container.Register(() =>
                System.Windows.Application.Current.Dispatcher.Invoke(() => new MainView(container.GetInstance<ILogger>())), Lifestyle.Transient);
        }
    }
}