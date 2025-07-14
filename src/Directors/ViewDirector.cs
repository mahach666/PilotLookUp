using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp
{
    public static class ViewDirector
    {
        public static void LookSelection(
            ObjectSet selectedObjects
            , IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , Ascon.Pilot.Themes.ThemeNames theme)
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] ViewDirector.LookSelection: selectedObjects is null? {selectedObjects == null}, count: {selectedObjects?.Count ?? 0}");
            if (selectedObjects != null)
            {
                foreach (var obj in selectedObjects)
                {
                    System.Diagnostics.Debug.WriteLine($"[TRACE] ViewDirector.LookSelection: item type: {obj?.GetType().Name}, name: {obj?.Name}");
                }
            }
            if (selectedObjects == null || !selectedObjects.IsLookable || selectedObjects.Count == 0)
            {
                System.Windows.MessageBox.Show("Нет выбранных объектов для просмотра.", "Предупреждение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.LookUpPage, selectedObjects);
        }

        public static void LookDB(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , Ascon.Pilot.Themes.ThemeNames theme)
        {
            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.LookUpPage);
        }

        public static void SearchPage(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , Ascon.Pilot.Themes.ThemeNames theme)
        {
            ShowView(objectsRepository, tabServiceProvider, theme, PagesName.SearchPage);
        }

        private static void ShowView(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , Ascon.Pilot.Themes.ThemeNames theme
            , PagesName pageName
            , ObjectSet selectedObjects = null)
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] ViewDirector.ShowView: pageName={pageName}, selectedObjects.Count={selectedObjects?.Count}");
            try
            {
                // Создаем новый контейнер для этого окна
                var container = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider, theme);
                System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: DI контейнер создан");
                
                var navigationService = container.GetInstance<INavigationService>();
                System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: navigationService получен");
                
                // Настраиваем начальную страницу
                switch (pageName)
                {
                    case PagesName.LookUpPage:
                        navigationService.NavigateToLookUp(selectedObjects);
                        System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: NavigateToLookUp вызван");
                        break;
                    case PagesName.SearchPage:
                        navigationService.NavigateToSearch();
                        System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: NavigateToSearch вызван");
                        break;
                }
                
                var viewModelFactory = container.GetInstance<IViewModelFactory>();
                System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: viewModelFactory получен");
                var mainVM = viewModelFactory.CreateMainVM();
                System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: mainVM создан");
                
                var windowFactory = container.GetInstance<IWindowFactory>();
                System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: windowFactory получен");
                
                // Создаем и показываем окно
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: Dispatcher.Invoke стартует");
                        var window = windowFactory.CreateMainWindow(mainVM);
                        System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: MainView создан через фабрику");
                        window.Show();
                        System.Diagnostics.Debug.WriteLine("[TRACE] ViewDirector.ShowView: window.Show() вызван");
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TRACE] ViewDirector.ShowView: Ошибка при создании окна: {ex.Message}");
                        System.Windows.MessageBox.Show($"Ошибка при создании окна: {ex.Message}", 
                            "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                });
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TRACE] ViewDirector.ShowView: Ошибка верхнего уровня: {ex.Message}");
                System.Windows.MessageBox.Show($"Ошибка при создании окна: {ex.Message}\n\n{ex.StackTrace}", 
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw;
            }
        }
    }
}
