﻿using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows;
using PilotLookUp.Commands;
using System.Windows.Input;
using System.Linq;
using IDataObject = Ascon.Pilot.SDK.IDataObject;
using System.Collections.Generic;
using Ascon.Pilot.SDK.Toolbar;


namespace PilotLookUp
{

    [Export(typeof(IMenu<MainViewContext>))]
    [Export(typeof(IToolbar<ObjectsViewContext>))]
    public class App : IMenu<MainViewContext>, IToolbar<ObjectsViewContext>
    {

        private IObjectsRepository _objectsRepository;
        private IFileProvider _fileProvider;
        private ITabServiceProvider _tabServiceProvider;
        private IObjectModifier _objectModifier;
        private ObjectsViewContext _viewContext;

        private IDataObject _selection;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository, IFileProvider fileProvider
            , ITabServiceProvider tabServiceProvider, IObjectModifier objectModifier)
        {
            _objectsRepository = objectsRepository;
            _fileProvider = fileProvider;
            _tabServiceProvider = tabServiceProvider;
            _objectModifier = objectModifier;
        }

        public void Build(IMenuBuilder builder, MainViewContext context)
        {
            var item = builder.AddItem("PilotLookUp", 1).WithHeader("PilotLookUp");
            item.WithSubmenu().AddItem("LookSelected", 0).WithHeader("LookSelected");
            item.WithSubmenu().AddItem("LookDB", 0).WithHeader("LookDB");
        }

        public void Build(IToolbarBuilder builder, ObjectsViewContext context)
        {
            _viewContext = context;
            _selection = context.SelectedObjects.First();
        }

        public void OnMenuItemClick(string name, MainViewContext context)
        {
            if (name == "LookSelected")
            {
                new RiseCommand( new LookSelektion());
            }
            else if (name == "LookDB")
            {
                //LookDB(context);
            }
        }

        public void OnToolbarItemClick(string name, ObjectsViewContext context)
        {
        }
    }
}