using PilotLookUp.Interfaces;
using System.Windows;

namespace PilotLookUp.Model.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        public void ShowError(string message, string title = "Ошибка")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        public void ShowInfo(string message, string title = "Информация")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void ShowWarning(string message, string title = "Предупреждение")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }

        public bool ShowConfirmation(string message, string title = "Подтверждение")
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                return result == MessageBoxResult.Yes;
            });
        }
    }
} 