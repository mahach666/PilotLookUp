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
            , ITabServiceProvider tabServiceProvider)
        {
            if (selectedObjects == null || !selectedObjects.IsLookable || selectedObjects.Count == 0)
            {
                System.Windows.MessageBox.Show("Нет выбранных объектов для просмотра.", "Предупреждение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            ShowView(objectsRepository, tabServiceProvider, PagesName.LookUpPage, selectedObjects);
        }

        public static void LookDB(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            ShowView(objectsRepository, tabServiceProvider, PagesName.LookUpPage);
        }

        public static void SearchPage(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            ShowView(objectsRepository, tabServiceProvider, PagesName.SearchPage);
        }

        private static void ShowView(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , PagesName pageName
            , ObjectSet selectedObjects = null)
        {
            try
            {
                // Устанавливаем глобальные сервисы (если еще не установлены)
                ServiceContainer.SetGlobalServices(objectsRepository, tabServiceProvider);
                
                // Создаем новый контейнер для этого окна
                var container = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider, App.Theme);
                
                var navigationService = container.GetInstance<INavigationService>();

                // Настраиваем начальную страницу
                switch (pageName)
                {
                    case PagesName.LookUpPage:
                        navigationService.NavigateToLookUp(selectedObjects);
                        break;
                    case PagesName.SearchPage:
                        navigationService.NavigateToSearch();
                        break;
                }

                var viewModelFactory = container.GetInstance<IViewModelFactory>();
                var mainVM = viewModelFactory.CreateMainVM();

                // Создаем и показываем окно в UI потоке
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        var window = container.GetInstance<MainView>();
                        window.DataContext = mainVM;
                        window.Show();
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Ошибка при создании окна: {ex.Message}", 
                            "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при создании окна: {ex.Message}\n\n{ex.StackTrace}", 
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw;
            }
        }
    }
}
