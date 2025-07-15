using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IWindowFactory
    {
        MainView CreateMainWindow(MainVM mainViewModel);
    }
} 