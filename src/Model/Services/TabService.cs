using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class TabService : ITabService
    {
        private  ITabServiceProvider _tabServiceProvider { get; }

        public TabService(ITabServiceProvider tabServiceProvider)
        {
            _tabServiceProvider = tabServiceProvider;
        }

        public void GoTo(IDataObject dataObject)
        => _tabServiceProvider.ShowElement(dataObject.Id);
    }
}
