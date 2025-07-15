using System.Resources;
using PilotLookUp.Domain.Interfaces;
using System.Windows;
using PilotLookUp.Resources;

namespace PilotLookUp.Model.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        public void ShowError(string message, string title = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title ?? Strings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        public void ShowInfo(string message, string title = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title ?? Strings.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void ShowWarning(string message, string title = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title ?? Strings.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }

        public bool ShowConfirmation(string message, string title = null)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(message, title ?? Strings.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                return result == MessageBoxResult.Yes;
            });
        }
    }
} 