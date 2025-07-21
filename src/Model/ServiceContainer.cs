﻿using Ascon.Pilot.SDK;
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
            IPilotDialogService pilotDialogService)
        {
            if (_container != null)
                throw new InvalidOperationException("!!!");

            _container = new Container();

            _container.RegisterInstance(objectsRepository);
            _container.RegisterInstance(tabServiceProvider);
            _container.RegisterInstance(pilotDialogService);

            RegisterServices(_container);
            RegisterFactories(_container);

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
        }

        private void RegisterFactories(Container container)
        {
            container.Register<IViewFactory, ViewFactory>(Lifestyle.Singleton);
            container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);
            container.Register<IPageServiceFactory, PageServiceFactory>(Lifestyle.Singleton);
        }
    }
}
