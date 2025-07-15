using PilotLookUp.Domain.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly string _logFilePath = "error.log";

        public void HandleError(Exception ex, string context = null)
        {
            LogError(ex, context);
        }

        public void LogError(Exception ex, string context = null)
        {
            var message = $"[{DateTime.Now}] ERROR{(context != null ? $" [{context}]" : "")}: {ex.Message}\n{ex.StackTrace}\n";
            try
            {
                File.AppendAllText(_logFilePath, message);
            }
            catch
            {
                Console.WriteLine(message);
            }
        }

        public async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            int retryCount = 3,
            string context = null)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex)
                {
                    attempts++;
                    LogError(ex, context);
                    if (attempts >= retryCount)
                        throw;
                    await Task.Delay(500 * attempts);
                }
            }
        }

        public async Task ExecuteWithRetryAsync(
            Func<Task> operation,
            int retryCount = 3,
            string context = null)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    await operation();
                    return;
                }
                catch (Exception ex)
                {
                    attempts++;
                    LogError(ex, context);
                    if (attempts >= retryCount)
                        throw;
                    await Task.Delay(500 * attempts);
                }
            }
        }
    }
} 