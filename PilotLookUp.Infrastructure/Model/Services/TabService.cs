using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Contracts;
using System;

namespace PilotLookUp.Infrastructure.Model.Services
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
