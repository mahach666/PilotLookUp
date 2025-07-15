namespace PilotLookUp.Domain.Interfaces
{
    public interface IUserNotificationService
    {
        void ShowError(string message, string title = "Ошибка");
        void ShowInfo(string message, string title = "Информация");
        void ShowWarning(string message, string title = "Предупреждение");
        bool ShowConfirmation(string message, string title = "Подтверждение");
    }
} 