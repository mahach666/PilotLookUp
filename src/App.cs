using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using System;
using System.ComponentModel.Composition;

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
            IPilotServiceProvider serviceProvider,
            IFileProvider fileProvider)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;

            var serviceContainer = new ServiceContainer();
            var container = serviceContainer
                .CreateContainer(objectsRepository,
                tabServiceProvider,
                pilotDialogService,
                fileProvider);

            _viewFactory = container.GetInstance<IViewFactory>();
            _selectedService = container.GetInstance<ISelectedService>();

            serviceProvider
                .Register<IMenu<ModelContext>>(
                    new ModelContextButton(
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