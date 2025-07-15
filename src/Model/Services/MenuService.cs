using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class MenuService : IMenuService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly ISelectionService _selectionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IViewDirectorService _viewDirectorService;

        public MenuService(
            IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            ISelectionService selectionService,
            IThemeProvider themeProvider,
            IViewDirectorService viewDirectorService)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _selectionService = selectionService;
            _themeProvider = themeProvider;
            _viewDirectorService = viewDirectorService;
        }

        public void HandleMenuItemClick(string name)
        {
            switch (name)
            {
                case "LookDB":
                    _viewDirectorService.LookDB(_objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    break;
                case "Search":
                    _viewDirectorService.SearchPage(_objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    break;
                case "LookSelected":
                    var selection = _selectionService.GetCurrentSelection();
                    if (_selectionService.HasSelection())
                    {
                        _viewDirectorService.LookSelection(selection, _objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    }
                    break;
            }
        }

        public void HandleToolbarItemClick(string name)
        {

        }
    }
} 