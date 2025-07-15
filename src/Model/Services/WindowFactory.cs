using PilotLookUp.Domain.Interfaces;
using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Services
{
    public class WindowFactory : IWindowFactory
    {
        public MainView CreateMainWindow(MainVM mainViewModel)
        {
            return System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                var window = new MainView();
                window.DataContext = mainViewModel;
                return window;
            });
        }
    }
} 