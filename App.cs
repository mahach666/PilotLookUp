using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows;

namespace PilotLookUp
{

    [Export(typeof(IMenu<MainViewContext>))]
    public class App : IMenu<MainViewContext>
    {

        private IObjectsRepository _objectsRepository;
        private IFileProvider _fileProvider;
        private ITabServiceProvider _tabServiceProvider;
        private IObjectModifier _objectModifier;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository, IFileProvider fileProvider, ITabServiceProvider tabServiceProvider, IObjectModifier objectModifier)
        {
            _objectsRepository = objectsRepository;
            _fileProvider = fileProvider;
            _tabServiceProvider = tabServiceProvider;
            _objectModifier = objectModifier;
        }

        public void Build(IMenuBuilder builder, MainViewContext context)
        {
            builder.AddItem("Gogogo", 1).WithHeader("Anal boss");
        }

        public void OnMenuItemClick(string name, MainViewContext context)
        {
            MessageBox.Show("Ura");
        }
    }
}