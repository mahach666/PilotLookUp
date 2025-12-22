using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Factories;
using PilotLookUp.Model.Services;
using SimpleInjector;
using System;
using Container = SimpleInjector.Container;

namespace PilotLookUp.Model
{
    public class ServiceContainer
    {
        private static Container _container;

        public Container CreateContainer(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            IFileProvider fileProvider)
        {
            //if (_container != null)
            //    throw new InvalidOperationException("Container has already been created. Repeated initialization is not allowed.");

            _container = new Container();

            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);
            _container.RegisterInstance(pilotDialogService);
            _container.RegisterInstance(fileProvider);

            RegisterServices(_container);
            RegisterFactories(_container);

            _container.Verify();
            return _container;
        }

        public Container RegisterBim(IModelManager modelManager,
         IModelStorageProvider modelStorageProvider)
        {
            if (_container == null)
                throw new InvalidOperationException("Container is not initialized.");

            _container.RegisterInstance(modelManager);
            _container.RegisterInstance(modelStorageProvider);

            _container.Register<BimModelService>(Lifestyle.Singleton);

            _container.Verify();
            return _container;
        }

        private void RegisterServices(Container container)
        {
            container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
            container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
            container.Register<ITabService, TabService>(Lifestyle.Singleton);
            container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);
            container.Register<ISelectedService, SelectedService>(Lifestyle.Singleton);
            container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
            container.Register<IFileService, FileService>(Lifestyle.Singleton);
        }

        private void RegisterFactories(Container container)
        {
            container.Register<IViewFactory, ViewFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            container.Register<IPageServiceFactory, PageServiceFactory>(Lifestyle.Singleton);
        }
    }
}
