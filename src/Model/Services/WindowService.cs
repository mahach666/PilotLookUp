using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;

namespace PilotLookUp.Model.Services
{
    public class WindowService : IWindowService
    {
        private readonly IViewFactory _viewFactory;

        public WindowService(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            _viewFactory.LookSelection(obj);
        }
    }
}
