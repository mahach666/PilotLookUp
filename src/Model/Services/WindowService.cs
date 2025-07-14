using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;

namespace PilotLookUp.Model.Services
{
    public class WindowService : IWindowService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IThemeProvider _themeProvider;

        public WindowService(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider, IThemeProvider themeProvider)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _themeProvider = themeProvider;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            ViewDirector.LookSelection(obj, _objectsRepository, _tabServiceProvider, _themeProvider.Theme);
        }
    }
}
