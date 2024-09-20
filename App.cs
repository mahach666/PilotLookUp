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
using Ascon.Pilot.SDK.Toolbar;
using PilotLookUp.Model;
using System.Xml.Linq;


namespace PilotLookUp
{

    [Export(typeof(IMenu<MainViewContext>))]
    [Export(typeof(IMenu<ObjectsViewContext>))]
    [Export(typeof(IMenu<StorageContext>))]
    [Export(typeof(IMenu<TasksViewContext2>))]
    [Export(typeof(IMenu<DocumentFilesContext>))]
    [Export(typeof(IMenu<LinkedObjectsContext>))]
    public class App : IMenu<MainViewContext>, IMenu<ObjectsViewContext>
        , IMenu<StorageContext>, IMenu<TasksViewContext2>
        , IMenu<DocumentFilesContext>, IMenu<LinkedObjectsContext>
    {

        private IObjectsRepository _objectsRepository;
        private IFileProvider _fileProvider;
        private ITabServiceProvider _tabServiceProvider;
        private IObjectModifier _objectModifier;
        private List<PilotTypsHelper> _convertSelection;

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

        public void Build(IMenuBuilder builder, ObjectsViewContext context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }

        public void Build(IMenuBuilder builder, StorageContext context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }

        public void Build(IMenuBuilder builder, TasksViewContext2 context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }
        public void Build(IMenuBuilder builder, DocumentFilesContext context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }
        public void Build(IMenuBuilder builder, LinkedObjectsContext context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }


        public void OnMenuItemClick(string name, MainViewContext context)
        {
            ItemClick(name);
        }

        public void OnMenuItemClick(string name, ObjectsViewContext context)
        {
            ItemClick(name);
        }

        public void OnMenuItemClick(string name, StorageContext context)
        {
            ItemClick(name);
        }

        public void OnMenuItemClick(string name, TasksViewContext2 context)
        {
            ItemClick(name);
        }

        public void OnMenuItemClick(string name, DocumentFilesContext context)
        {
            ItemClick(name);
        }

        public void OnMenuItemClick(string name, LinkedObjectsContext context)
        {
            ItemClick(name);
        }





        private void ItemClick(string name)
        {
            //List<PilotTypsHelper> converter = _selection?.Select(i => new PilotTypsHelper(i))?.ToList();

            if (_convertSelection == null || !_convertSelection.Any()) return;

            if (name == "LookSelected")
            {
                new LookSeleсtion(_convertSelection, _objectsRepository);
            }
            else if (name == "LookDB")
            {
                //LookDB(context);
            }
        }

        private void ContextButtunBuilder(IMenuBuilder builder)
        {
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }

        private void SelectUpdater(MarshalByRefObject context)
        {
            switch (context)
            {
                case MainViewContext mainViewContext:
                    return;

                case ObjectsViewContext objectsViewContext:
                    _convertSelection = objectsViewContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    return;

                case TasksViewContext2 tasksViewContext:
                    _convertSelection = tasksViewContext.SelectedTasks?.Select(i => new PilotTypsHelper(i)).ToList();
                    return;

                case DocumentFilesContext documentFilesContext:
                    _convertSelection = documentFilesContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    break;

                case LinkedObjectsContext linkedObjectsContext:
                    _convertSelection = linkedObjectsContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    break;
            }
        }

    }
}