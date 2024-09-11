using Ascon.Pilot.SDK;
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


namespace PilotLookUp
{

    [Export(typeof(IMenu<MainViewContext>))]

    public class App : IMenu<MainViewContext>
    {

        private IObjectsRepository _objectsRepository;
        private IFileProvider _fileProvider;
        private ITabServiceProvider _tabServiceProvider;
        private IObjectModifier _objectModifier;

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

        public void OnMenuItemClick(string name, MainViewContext context)
        {
            //MessageBox.Show("Ura");
            var a = _selection;
            if (name == "LookSelected")
            {
                new RiseCommand( new LookSelektion());
            }
            else if (name == "LookDB")
            {
                //LookDB(context);
            }
        }
    }
}