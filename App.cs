using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using Ascon.Pilot.Themes;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System;
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

    public class App : IMenu<MainViewContext>
        , IMenu<ObjectsViewContext>
        , IMenu<StorageContext>
        , IMenu<TasksViewContext2>
        , IMenu<DocumentFilesContext>
        , IMenu<LinkedObjectsContext>
        , IMenu<LinkedTasksContext2>
    {
        private IObjectsRepository _objectsRepository;
        private ObjectSet _convertSelection;
        private static ThemeNames _theme { get; set; }
        public static ThemeNames Theme { get=> _theme; }

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository, IPilotDialogService pilotDialogService)
        {
            _objectsRepository = objectsRepository;
            _theme = pilotDialogService.Theme;
        }

        // Build
        public void Build(IMenuBuilder builder, MainViewContext context)
        {
            var item = builder.AddItem("PilotLookUp", 1).WithHeader("PilotLookUp");
            item.WithSubmenu().AddItem("LookSelected", 0).WithHeader("LookSelected");
            item.WithSubmenu().AddItem("LookDB", 1).WithHeader("LookDB");
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

        // Event
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




        private void ItemClick(string name)
        {
            if (name == "LookDB")
            {
                var pilotObjectMap = new PilotObjectMap(_objectsRepository);
                var repo = new ObjectSet(null) { pilotObjectMap.Wrap(_objectsRepository) };
                new LookSeleсtion(repo, _objectsRepository);
            }

            if (_convertSelection == null || !_convertSelection.Any()) return;

            if (name == "LookSelected")
            {
                new LookSeleсtion(_convertSelection, _objectsRepository);
            }
        }

        private void ContextButtunBuilder(IMenuBuilder builder)
        {
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }

        private void SelectUpdater(MarshalByRefObject context)
        {
            var pilotObjectMap = new PilotObjectMap(_objectsRepository);
            _convertSelection = new ObjectSet(null);

            switch (context)
            {
                case MainViewContext mainViewContext:
                    return;

                case ObjectsViewContext objectsViewContext:
                    var selectedDO = objectsViewContext.SelectedObjects?.Select(i => pilotObjectMap.Wrap(i)).ToList();
                    _convertSelection.AddRange(selectedDO.Any() ? selectedDO : _convertSelection);
                    return;

                case TasksViewContext2 tasksViewContext:
                    var selectedT = tasksViewContext.SelectedTasks?.Select(i => pilotObjectMap.Wrap(i)).ToList();
                    _convertSelection.AddRange(selectedT.Any() ? selectedT : _convertSelection);
                    return;

                case DocumentFilesContext documentFilesContext:
                    var selectedF = documentFilesContext.SelectedObjects?.Select(i => pilotObjectMap.Wrap(i)).ToList();
                    _convertSelection.AddRange(selectedF.Any() ? selectedF : _convertSelection);
                    break;

                case LinkedObjectsContext linkedObjectsContext:
                    var selectedLO = linkedObjectsContext.SelectedObjects?.Select(i => pilotObjectMap.Wrap(i)).ToList();
                    _convertSelection.AddRange(selectedLO.Any() ? selectedLO : _convertSelection);
                    break;

                case LinkedTasksContext2 linkedTasksContext:
                    var selectedLT = linkedTasksContext.SelectedTasks?.Select(i => pilotObjectMap.Wrap(i)).ToList();
                    _convertSelection.AddRange(selectedLT.Any() ? selectedLT : _convertSelection);
                    break;
            }
        }

    }
}