using System;
using System.Threading.Tasks;
using System.Windows;
using PilotLookUp.Interfaces;

namespace PilotLookUp.UI
{
    public class DispatcherService : IDispatcherService
    {
        public void Invoke(Action action) => Application.Current.Dispatcher.Invoke(action);
        public Task InvokeAsync(Func<Task> func) => Application.Current.Dispatcher.InvokeAsync(func).Task;
    }
} 