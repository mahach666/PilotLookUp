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
        private readonly Ascon.Pilot.Themes.ThemeNames _theme;

        public MenuService(
            IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            ISelectionService selectionService,
            Ascon.Pilot.Themes.ThemeNames theme)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _selectionService = selectionService;
            _theme = theme;
        }

        public void HandleMenuItemClick(string name)
        {
            switch (name)
            {
                case "LookDB":
                    ViewDirector.LookDB(_objectsRepository, _tabServiceProvider, _theme);
                    break;
                case "Search":
                    ViewDirector.SearchPage(_objectsRepository, _tabServiceProvider, _theme);
                    break;
                case "LookSelected":
                    var selection = _selectionService.GetCurrentSelection();
                    if (_selectionService.HasSelection())
                    {
                        ViewDirector.LookSelection(selection, _objectsRepository, _tabServiceProvider, _theme);
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