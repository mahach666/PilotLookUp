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

        public MenuService(
            IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            ISelectionService selectionService)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
            _selectionService = selectionService;
        }

        public void HandleMenuItemClick(string name)
        {
            switch (name)
            {
                case "LookDB":
                    ViewDirector.LookDB(_objectsRepository, _tabServiceProvider);
                    break;
                case "Search":
                    ViewDirector.SearchPage(_objectsRepository, _tabServiceProvider);
                    break;
                case "LookSelected":
                    var selection = _selectionService.GetCurrentSelection();
                    if (_selectionService.HasSelection())
                    {
                        ViewDirector.LookSelection(selection, _objectsRepository, _tabServiceProvider);
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