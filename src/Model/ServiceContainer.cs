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
        private Container _container;

        public Container CreateContainer(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            IFileProvider fileProvider,
            IModelManager modelManager = null,
            IModelStorageProvider modelStorageProvider = null)
        {
            _container = new Container();

            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);
            _container.RegisterInstance(pilotDialogService);
            _container.RegisterInstance(fileProvider);

            RegisterServices(_container);
            RegisterFactories(_container);

            // BIM-сервисы клиент отдает только в Pilot-BIM (в ICE - null);
            // регистрация делегатом до единственного Verify, RegisterInstance(null) невозможен
            _container.Register<IBimModelService>(
                () => new BimModelService(modelManager, modelStorageProvider,
                    _container.GetInstance<IClientEventService>(),
                    _container.GetInstance<IObjectsRepository>()),
                Lifestyle.Singleton);

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
            container.Register<IClientEventService, ClientEventService>(Lifestyle.Singleton);
            container.Register<IMenuCommandHandler, MenuCommandHandler>(Lifestyle.Singleton);
        }

        private void RegisterFactories(Container container)
        {
            container.Register<IViewFactory, ViewFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            container.Register<IPageServiceFactory, PageServiceFactory>(Lifestyle.Singleton);

            // Lazy-обертки для разрыва циклов между фабриками и сервисами,
            // зависящими от IViewModelFactory
            container.Register(() => new Lazy<IViewFactory>(container.GetInstance<IViewFactory>), Lifestyle.Singleton);
            container.Register(() => new Lazy<ITreeItemService>(container.GetInstance<ITreeItemService>), Lifestyle.Singleton);
            container.Register(() => new Lazy<IPageServiceFactory>(container.GetInstance<IPageServiceFactory>), Lifestyle.Singleton);
        }
    }
}
