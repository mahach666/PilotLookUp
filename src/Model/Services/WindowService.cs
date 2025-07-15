using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class WindowService : IWindowService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IThemeProvider _themeProvider;
        private readonly IViewDirectorService _viewDirectorService;

        public WindowService(
            IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IThemeProvider themeProvider,
            IViewDirectorService viewDirectorService)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _themeProvider = themeProvider;
            _viewDirectorService = viewDirectorService;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            _viewDirectorService.LookSelection(obj, _objectsRepository, _tabServiceProvider, _themeProvider.Theme);
        }
    }
}
