using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using Ascon.Pilot.SDK.Toolbar;
using PilotLookUp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;


namespace PilotLookUp
{

    [Export(typeof(IMenu<MainViewContext>))]
    [Export(typeof(IMenu<ObjectsViewContext>))]
    [Export(typeof(IMenu<StorageContext>))]
    [Export(typeof(IMenu<TasksViewContext2>))]
    [Export(typeof(IMenu<DocumentFilesContext>))]
    [Export(typeof(IMenu<LinkedObjectsContext>))]
    [Export(typeof(IMenu<LinkedTasksContext2>))]
    //[Export(typeof(IToolbar<ObjectsViewContext>))]
    //[Export(typeof(IToolbar<StorageContext>))]
    //[Export(typeof(IToolbar<TasksViewContext2>))]
    //[Export(typeof(IToolbar<DocumentFilesContext>))]
    //[Export(typeof(IToolbar<LinkedObjectsContext>))]
    public class App : IMenu<MainViewContext>, IMenu<ObjectsViewContext>
        , IMenu<StorageContext>, IMenu<TasksViewContext2>
        , IMenu<DocumentFilesContext>, IMenu<LinkedObjectsContext>, IMenu<LinkedTasksContext2>
    //, IToolbar<ObjectsViewContext>, IToolbar<StorageContext>
    //, IToolbar<TasksViewContext2>, IToolbar<DocumentFilesContext>
    //, IToolbar<LinkedObjectsContext>
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
        public void Build(IMenuBuilder builder, LinkedTasksContext2 context)
        {
            SelectUpdater(context);
            ContextButtunBuilder(builder);
        }

        //public void Build(IToolbarBuilder builder, ObjectsViewContext context)
        //{
        //    SelectUpdater(context);
        //}
        //public void Build(IToolbarBuilder builder, StorageContext context)
        //{
        //    SelectUpdater(context);
        //}
        //public void Build(IToolbarBuilder builder, TasksViewContext2 context)
        //{
        //    SelectUpdater(context);
        //}
        //public void Build(IToolbarBuilder builder, DocumentFilesContext context)
        //{
        //    SelectUpdater(context);
        //}
        //public void Build(IToolbarBuilder builder, LinkedObjectsContext context)
        //{
        //    SelectUpdater(context);
        //}


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

        public void OnMenuItemClick(string name, LinkedTasksContext2 context)
        {
            ItemClick(name);
        }

        //public void OnToolbarItemClick(string name, ObjectsViewContext context)
        //{          
        //}
        //public void OnToolbarItemClick(string name, StorageContext context)
        //{          
        //}
        //public void OnToolbarItemClick(string name, TasksViewContext2 context)
        //{          
        //}
        //public void OnToolbarItemClick(string name, DocumentFilesContext context)
        //{           
        //}
        //public void OnToolbarItemClick(string name, LinkedObjectsContext context)
        //{            
        //}




        private void ItemClick(string name)
        {
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
                    var selectedDO = objectsViewContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    _convertSelection = selectedDO.Any() ? selectedDO : _convertSelection;
                    return;

                case TasksViewContext2 tasksViewContext:
                    var selectedT = tasksViewContext.SelectedTasks?.Select(i => new PilotTypsHelper(i)).ToList();
                    _convertSelection = selectedT.Any() ? selectedT : _convertSelection;
                    return;

                case DocumentFilesContext documentFilesContext:
                    var selectedF = documentFilesContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    _convertSelection = selectedF.Any() ? selectedF : _convertSelection;
                    break;

                case LinkedObjectsContext linkedObjectsContext:
                    var selectedLO = linkedObjectsContext.SelectedObjects?.Select(i => new PilotTypsHelper(i)).ToList();
                    _convertSelection = selectedLO.Any() ? selectedLO : _convertSelection;
                    break;
                    case LinkedTasksContext2 linkedTasksContext:
                    var selectedLT = linkedTasksContext.SelectedTasks?.Select(i => new PilotTypsHelper(i)).ToList();
                    _convertSelection = selectedLT.Any() ? selectedLT : _convertSelection;
                    break;
            }
        }

    }
}