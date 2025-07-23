using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using SimpleInjector;
using System;
using System.ComponentModel.Composition;

namespace PilotLookUp
{
    public partial class App 
    {
        private readonly Container _container;
        private readonly IViewFactory _viewFactory;
        private readonly ISelectedService _selectedService;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;

            _container = new ServiceContainer().CreateContainer(objectsRepository,
                tabServiceProvider,
                pilotDialogService);

            _viewFactory = _container.GetInstance<IViewFactory>();
            _selectedService = _container.GetInstance<ISelectedService>();
        }
    }
}