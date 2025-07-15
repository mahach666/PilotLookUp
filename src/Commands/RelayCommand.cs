using System;
using System.Windows.Input;

namespace PilotLookUp.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;

        public void Execute(object parameter) => _execute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class SafeRelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly Domain.Interfaces.IErrorHandlingService _errorHandlingService;
        private readonly Domain.Interfaces.IUserNotificationService _notificationService;
        private readonly string _errorContext;
        private readonly string _userMessage;

        public SafeRelayCommand(Action<T> execute,
            Domain.Interfaces.IErrorHandlingService errorHandlingService,
            Domain.Interfaces.IUserNotificationService notificationService,
            string errorContext = null,
            string userMessage = null,
            Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandlingService = errorHandlingService;
            _notificationService = notificationService;
            _errorContext = errorContext;
            _userMessage = userMessage;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;

        public void Execute(object parameter)
        {
            try
            {
                _execute((T)parameter);
            }
            catch (Exception ex)
            {
                _errorHandlingService?.HandleError(ex, _errorContext);
                if (!string.IsNullOrEmpty(_userMessage))
                    _notificationService?.ShowError(string.Format(_userMessage, ex.Message));
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
