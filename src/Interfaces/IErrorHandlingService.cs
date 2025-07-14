using System;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface IErrorHandlingService
    {
        void HandleError(Exception ex, string context = null);
        void LogError(Exception ex, string context = null);
        Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int retryCount = 3, string context = null);
        Task ExecuteWithRetryAsync(Func<Task> operation, int retryCount = 3, string context = null);
    }
} 