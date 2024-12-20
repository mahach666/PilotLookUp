﻿using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
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

    public class App : IMenu<MainViewContext>
        , IMenu<ObjectsViewContext>
        , IMenu<StorageContext>
        , IMenu<TasksViewContext2>
        , IMenu<DocumentFilesContext>
        , IMenu<LinkedObjectsContext>
        , IMenu<LinkedTasksContext2>
    {
        private IObjectsRepository _objectsRepository;
        private List<PilotObjectHelper> _convertSelection;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }

        // Build
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
            PilotObjectMap.Updaate(_objectsRepository);

            switch (context)
            {
                case MainViewContext mainViewContext:
                    return;

                case ObjectsViewContext objectsViewContext:
                    var selectedDO = objectsViewContext.SelectedObjects?.Select(i => PilotObjectMap.Wrap(i)).ToList();
                    _convertSelection = selectedDO.Any() ? selectedDO : _convertSelection;
                    return;

                case TasksViewContext2 tasksViewContext:
                    var selectedT = tasksViewContext.SelectedTasks?.Select(i => PilotObjectMap.Wrap(i)).ToList();
                    _convertSelection = selectedT.Any() ? selectedT : _convertSelection;
                    return;

                case DocumentFilesContext documentFilesContext:
                    var selectedF = documentFilesContext.SelectedObjects?.Select(i => PilotObjectMap.Wrap(i)).ToList();
                    _convertSelection = selectedF.Any() ? selectedF : _convertSelection;
                    break;

                case LinkedObjectsContext linkedObjectsContext:
                    var selectedLO = linkedObjectsContext.SelectedObjects?.Select(i => PilotObjectMap.Wrap(i)).ToList();
                    _convertSelection = selectedLO.Any() ? selectedLO : _convertSelection;
                    break;

                case LinkedTasksContext2 linkedTasksContext:
                    var selectedLT = linkedTasksContext.SelectedTasks?.Select(i => PilotObjectMap.Wrap(i)).ToList();
                    _convertSelection = selectedLT.Any() ? selectedLT : _convertSelection;
                    break;
            }
        }

    }
}