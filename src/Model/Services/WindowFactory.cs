using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Services
{
    public class WindowFactory : IWindowFactory
    {
        private readonly ILogger _logger;
        public WindowFactory(ILogger logger)
        {
            _logger = logger;
        }
        public MainView CreateMainWindow(MainVM mainViewModel)
        {
            return System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                var window = new MainView(_logger);
                window.DataContext = mainViewModel;
                return window;
            });
        }
    }
} 