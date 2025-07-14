using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;

namespace PilotLookUp.Model.Services
{
    public class WindowService : IWindowService
    {
        private IObjectsRepository _objectsRepository;
        private ITabServiceProvider _tabServiceProvider;
        private Ascon.Pilot.Themes.ThemeNames _theme;

        public WindowService(IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , Ascon.Pilot.Themes.ThemeNames theme)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _theme = theme;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            ViewDirector.LookSelection(obj, _objectsRepository, _tabServiceProvider, _theme);
        }
    }
}
