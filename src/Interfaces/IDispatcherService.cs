using System;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface IDispatcherService
    {
        void Invoke(Action action);
        Task InvokeAsync(Func<Task> func);
    }
} 