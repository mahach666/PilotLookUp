using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;

namespace PilotLookUp.Model.Services
{
    public class MenuService : IMenuService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly ISelectionService _selectionService;
        private readonly IThemeProvider _themeProvider;

        public MenuService(
            IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            ISelectionService selectionService,
            IThemeProvider themeProvider)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _selectionService = selectionService;
            _themeProvider = themeProvider;
        }

        public void HandleMenuItemClick(string name)
        {
            switch (name)
            {
                case "LookDB":
                    ViewDirector.LookDB(_objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    break;
                case "Search":
                    ViewDirector.SearchPage(_objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    break;
                case "LookSelected":
                    var selection = _selectionService.GetCurrentSelection();
                    if (_selectionService.HasSelection())
                    {
                        ViewDirector.LookSelection(selection, _objectsRepository, _tabServiceProvider, _themeProvider.Theme);
                    }
                    break;
            }
        }

        public void HandleToolbarItemClick(string name)
        {
            // Пока не используется, но может понадобиться в будущем
        }
    }
} 