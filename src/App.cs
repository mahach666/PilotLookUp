using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace PilotLookUp
{
    public partial class App
    {
        private readonly IViewFactory _viewFactory;
        private readonly ISelectedService _selectedService;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            IPilotServiceProvider serviceProvider)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;

            var serviceContainer = new ServiceContainer();
            var container = serviceContainer
                .CreateContainer(objectsRepository,
                tabServiceProvider,
                pilotDialogService);

            _viewFactory = container.GetInstance<IViewFactory>();
            _selectedService = container.GetInstance<ISelectedService>();

            serviceProvider
                .Register<IMenu<IModelElementId>>(
                new BimContextButton(
                    ContextButtonBuilder,
                    ItemClick));

            //var modelManager = serviceProvider.GetServices<IModelManager>().FirstOrDefault();
            //var modelStorageProvider = serviceProvider.GetServices<IModelStorageProvider>().FirstOrDefault();

            //if (modelManager != null && modelStorageProvider != null)
            //{
            //    serviceContainer.RegisterBim(modelManager, modelStorageProvider);
            //}
        }
    }
}