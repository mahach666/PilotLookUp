using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;

namespace PilotLookUp.Model.Services
{
    public class WindowService : IWindowService
    {
        private IObjectsRepository _objectsRepository;
        private ITabServiceProvider _tabServiceProvider;

        public WindowService(IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            ViewDirector.LookSelection(obj, _objectsRepository, _tabServiceProvider);
        }
    }
}
