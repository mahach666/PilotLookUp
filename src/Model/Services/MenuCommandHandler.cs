using Ascon.Pilot.SDK.Menu;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class MenuCommandHandler : IMenuCommandHandler
    {
        private readonly IViewFactory _viewFactory;
        private readonly ISelectedService _selectedService;
        private readonly IClientEventService _clientEventService;

        public MenuCommandHandler(
            IViewFactory viewFactory,
            ISelectedService selectedService,
            IClientEventService clientEventService)
        {
            _viewFactory = viewFactory;
            _selectedService = selectedService;
            _clientEventService = clientEventService;
        }

        public void BuildMainMenu(IMenuBuilder builder)
        {
            var item = builder.AddItem("PilotLookUp", 1).WithHeader("PilotLookUp");
            item.WithSubmenu().AddItem("LookSelected", 0).WithHeader("LookSelected");
            item.WithSubmenu().AddItem("LookDB", 1).WithHeader("LookDB");
            item.WithSubmenu().AddItem("Search", 2).WithHeader("Search");
            item.WithSubmenu().AddItem("EventMonitor", 3).WithHeader("EventMonitor");
        }

        public void BuildLookSelectedItem(IMenuBuilder builder)
        {
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }

        public void ItemClick(string name)
        {
            _clientEventService.Raise(new MenuItemClickRecord(name));

            switch (name)
            {
                case "LookDB":
                    _viewFactory.LookDB();
                    return;
                case "Search":
                    _viewFactory.SearchPage();
                    return;
                case "EventMonitor":
                    _viewFactory.EventMonitor();
                    return;
                case "LookSelected":
                    if (_selectedService.Selected?.Any() == true)
                        _viewFactory.LookSelection(_selectedService.Selected);
                    return;
            }
        }
    }
}
