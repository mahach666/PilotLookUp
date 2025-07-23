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

        private readonly IModelManager _modelManager;
        private readonly IModelStorageProvider _modelStorageProvider;
        private  IModelStorage _modelStorage;
        private DateTime _curentVersion;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            IPilotServiceProvider serviceProvider)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;


            var container = new ServiceContainer().CreateContainer(objectsRepository,
                tabServiceProvider,
                pilotDialogService);

            _viewFactory = container.GetInstance<IViewFactory>();
            _selectedService = container.GetInstance<ISelectedService>();

            serviceProvider
                .Register<IMenu<IModelElementId>>(
                new BimContextButton(
                    ContextButtonBuilder,
                    ItemClick
                ));

            _modelManager = serviceProvider.GetServices<IModelManager>().FirstOrDefault();
            _modelStorageProvider = serviceProvider.GetServices<IModelStorageProvider>().FirstOrDefault();


            Subscribe();
        }

        private void Subscribe()
        {
            _modelManager.ModelLoaded += OnModelLoaded;
        }

        private void OnModelLoaded(object sender, ModelEventArgs e)
        {
            _modelStorage = _modelStorageProvider.GetStorage(e.Viewer.ModelId);
            _curentVersion = e.Viewer.ModelVersion;
        }
    }
}