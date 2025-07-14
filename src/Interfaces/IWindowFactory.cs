using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IWindowFactory
    {
        MainView CreateMainWindow(MainVM mainViewModel);
    }
} 