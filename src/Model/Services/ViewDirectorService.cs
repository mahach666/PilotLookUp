using Ascon.Pilot.SDK;
using Ascon.Pilot.Themes;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Windows;

namespace PilotLookUp.Model.Services
{
    public class ViewDirectorService : IViewDirectorService
    {
        private readonly IUserNotificationService _notificationService;
        private readonly ILogger _logger;

        public ViewDirectorService(IUserNotificationService notificationService, ILogger logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public void LookSelection(
            ObjectSet selectedObjects
            , IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , ThemeNames theme)
        {
            _logger.Trace($"[TRACE] ViewDirectorService.LookSelection: selectedObjects is null? {selectedObjects == null}, count: {selectedObjects?.Count ?? 0}");
            if (selectedObjects != null)
            {
                foreach (var obj in selectedObjects)
                {
                    _logger.Trace($"[TRACE] ViewDirectorService.LookSelection: item type: {obj?.GetType().Name}, name: {obj?.Name}");
                }
            }
            if (selectedObjects == null || !selectedObjects.IsLookable || selectedObjects.Count == 0)
            {
                _notificationService.ShowWarning("Нет выбранных объектов для просмотра.");
                return;
            }

            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.LookUpPage, selectedObjects);
        }

        public void LookDB(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , ThemeNames theme)
        {
            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.LookUpPage);
        }

        public void SearchPage(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , ThemeNames theme)
        {
            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.SearchPage);
        }

        private void ShowView(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , ThemeNames theme
            , PagesName pageName
            , ObjectSet selectedObjects = null)
        {
            _logger.Trace($"[TRACE] ViewDirectorService.ShowView: pageName={pageName}, selectedObjects.Count={selectedObjects?.Count}");
            try
            {
                // Создаем новый контейнер для этого окна
                var container = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider, theme);
                _logger.Trace("[TRACE] ViewDirectorService.ShowView: DI контейнер создан");
                
                var navigationService = container.GetInstance<INavigationService>();
                _logger.Trace("[TRACE] ViewDirectorService.ShowView: navigationService получен");
                
                // Настраиваем начальную страницу
                switch (pageName)
                {
                    case PagesName.LookUpPage:
                        navigationService.NavigateToLookUp(selectedObjects);
                        _logger.Trace("[TRACE] ViewDirectorService.ShowView: NavigateToLookUp вызван");
                        break;
                    case PagesName.SearchPage:
                        navigationService.NavigateToSearch();
                        _logger.Trace("[TRACE] ViewDirectorService.ShowView: NavigateToSearch вызван");
                        break;
                }
                
                var viewModelFactory = container.GetInstance<IViewModelFactory>();
                _logger.Trace("[TRACE] ViewDirectorService.ShowView: viewModelFactory получен");
                var mainVM = viewModelFactory.CreateMainVM(navigationService);
                _logger.Trace("[TRACE] ViewDirectorService.ShowView: mainVM создан");
                
                var windowFactory = container.GetInstance<IWindowFactory>();
                _logger.Trace("[TRACE] ViewDirectorService.ShowView: windowFactory получен");
                
                // Создаем и показываем окно
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        _logger.Trace("[TRACE] ViewDirectorService.ShowView: Dispatcher.Invoke стартует");
                        var window = windowFactory.CreateMainWindow(mainVM);
                        _logger.Trace("[TRACE] ViewDirectorService.ShowView: MainView создан через фабрику");
                        window.Show();
                        _logger.Trace("[TRACE] ViewDirectorService.ShowView: window.Show() вызван");
                    }
                    catch (System.Exception ex)
                    {
                        _logger.Error($"[TRACE] ViewDirectorService.ShowView: Ошибка при создании окна: {ex.Message}");
                        _notificationService.ShowError($"Ошибка при создании окна: {ex.Message}");
                    }
                });
            }
            catch (System.Exception ex)
            {
                _logger.Error($"[TRACE] ViewDirectorService.ShowView: Ошибка верхнего уровня: {ex.Message}");
                _notificationService.ShowError($"Ошибка при создании окна: {ex.Message}");
                throw;
            }
        }
    }
} 