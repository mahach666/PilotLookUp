using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Contracts;
using PilotLookUp.Core.Objects;
using System;

namespace PilotLookUp.Infrastructure.Model.Services
{
    public class WindowService : IWindowService
    {
        private readonly INavigationService _navigationService;

        public WindowService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void CreateNewMainWindow(ObjectSet obj)
        {
            if (obj == null) return;
            _navigationService.LookSelection(obj);
        }
    }
}
