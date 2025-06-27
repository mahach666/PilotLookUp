using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using Ascon.Pilot.SDK.Toolbar;
using Ascon.Pilot.Themes;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
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
    [Export(typeof(IToolbar<MainViewContext>))]
    [Export(typeof(IToolbar<ObjectsViewContext>))]
    [Export(typeof(IToolbar<TasksViewContext2>))]
    [Export(typeof(IToolbar<DocumentFilesContext>))]
    [Export(typeof(IToolbar<LinkedObjectsContext>))]
    [Export(typeof(IToolbar<LinkedTasksContext2>))]
    public class App : IMenu<MainViewContext>,
        IMenu<ObjectsViewContext>,
        IMenu<StorageContext>,
        IMenu<TasksViewContext2>,
        IMenu<DocumentFilesContext>,
        IMenu<LinkedObjectsContext>,
        IMenu<LinkedTasksContext2>,
        IToolbar<MainViewContext>,
        IToolbar<ObjectsViewContext>, 
        IToolbar<TasksViewContext2>,
        IToolbar<DocumentFilesContext>,
        IToolbar<LinkedObjectsContext>,
        IToolbar<LinkedTasksContext2>
    {
        private IObjectsRepository _objectsRepository;
        private ITabServiceProvider _tabServiceProvider;
        private ObjectSet _convertSelection;
        private static ThemeNames _theme { get; set; }
        public static ThemeNames Theme { get => _theme; }

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;


            _theme = pilotDialogService.Theme;
        }

        // Build
        public void Build(IMenuBuilder builder, MainViewContext context)
        {
            var item = builder.AddItem("PilotLookUp", 1).WithHeader("PilotLookUp");
            item.WithSubmenu().AddItem("LookSelected", 0).WithHeader("LookSelected");
            item.WithSubmenu().AddItem("LookDB", 1).WithHeader("LookDB");
            item.WithSubmenu().AddItem("Search", 2).WithHeader("Search");
        }
        public void Build(IMenuBuilder builder, ObjectsViewContext context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IMenuBuilder builder, StorageContext context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IMenuBuilder builder, TasksViewContext2 context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IMenuBuilder builder, DocumentFilesContext context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IMenuBuilder builder, LinkedObjectsContext context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IMenuBuilder builder, LinkedTasksContext2 context) =>
            ContextButtonBuilder(builder, context);

        // Event
        public void OnMenuItemClick(string name, MainViewContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, ObjectsViewContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, StorageContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, TasksViewContext2 context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, DocumentFilesContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, LinkedObjectsContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, LinkedTasksContext2 context) =>
            ItemClick(name);

        private void ItemClick(string name)
        {
            if (name == "LookDB")
            {
                ViewDirector.LookDB(_objectsRepository, _tabServiceProvider);
                return;
            }
            else if (name == "Search")
            {
                ViewDirector.SearchPage(_objectsRepository, _tabServiceProvider);
                return;
            }

            if (_convertSelection == null || !_convertSelection.Any()) return;

            if (name == "LookSelected")
            {
                ViewDirector.LookSelection(_convertSelection, _objectsRepository, _tabServiceProvider);
                return;
            }

        }

        private void ContextButtonBuilder(IMenuBuilder builder, MarshalByRefObject context)
        {
            SelectUpdater(context);
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }

        private void SelectUpdater(MarshalByRefObject context)
        {
            var map = new PilotObjectMap(_objectsRepository);

            IEnumerable<object>? raw = context switch
            {
                ObjectsViewContext c => c.SelectedObjects?.Cast<object>(),
                DocumentFilesContext c => c.SelectedObjects?.Cast<object>(),
                LinkedObjectsContext c => c.SelectedObjects?.Cast<object>(),

                TasksViewContext2 c => c.SelectedTasks?.Cast<object>(),
                LinkedTasksContext2 c => c.SelectedTasks?.Cast<object>(),

                _ => null
            };

            if (raw?.Any() == true)
            {
                _convertSelection = new ObjectSet(null);
                _convertSelection.AddRange(raw.Select(map.Wrap));
            }
        }

        // Juts for stable update
        public void Build(IToolbarBuilder builder, ObjectsViewContext context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, ObjectsViewContext context) { }


        public void Build(IToolbarBuilder builder, TasksViewContext2 context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, TasksViewContext2 context) { }

        public void Build(IToolbarBuilder builder, DocumentFilesContext context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, DocumentFilesContext context) { }

        public void Build(IToolbarBuilder builder, LinkedObjectsContext context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, LinkedObjectsContext context) { }

        public void Build(IToolbarBuilder builder, LinkedTasksContext2 context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, LinkedTasksContext2 context) { }

        public void Build(IToolbarBuilder builder, MainViewContext context) =>
            SelectUpdater(context);

        public void OnToolbarItemClick(string name, MainViewContext context) { }
    }
}