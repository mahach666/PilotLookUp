using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using Ascon.Pilot.SDK.Toolbar;
using System.ComponentModel.Composition;

namespace PilotLookUp
{
    [Export(typeof(IMenu<MainViewContext>))]
    [Export(typeof(IMenu<ObjectsViewContext>))]
    [Export(typeof(IMenu<StorageContext>))]
    [Export(typeof(IMenu<TasksViewContext2>))]
    [Export(typeof(IMenu<DocumentFilesContext>))]
    [Export(typeof(IMenu<LinkedObjectsContext>))]
    [Export(typeof(IMenu<LinkedTasksContext2>))]
    [Export(typeof(IMenu<DocumentAnnotationsListContext>))]
    [Export(typeof(IMenu<SignatureRequestsContext>))]

    [Export(typeof(IToolbar<MainViewContext>))]
    [Export(typeof(IToolbar<ObjectsViewContext>))]
    [Export(typeof(IToolbar<TasksViewContext2>))]
    [Export(typeof(IToolbar<DocumentFilesContext>))]
    [Export(typeof(IToolbar<LinkedObjectsContext>))]
    [Export(typeof(IToolbar<LinkedTasksContext2>))]
    [Export(typeof(IToolbar<DocumentAnnotationsListContext>))]
    [Export(typeof(IToolbar<SignatureRequestsContext>))]
    public partial class App : IMenu<MainViewContext>,
        IMenu<ObjectsViewContext>,
        IMenu<StorageContext>,
        IMenu<TasksViewContext2>,
        IMenu<DocumentFilesContext>,
        IMenu<LinkedObjectsContext>,
        IMenu<LinkedTasksContext2>,
        IMenu<DocumentAnnotationsListContext>,
        IMenu<SignatureRequestsContext>,

        IToolbar<MainViewContext>,
        IToolbar<ObjectsViewContext>,
        IToolbar<TasksViewContext2>,
        IToolbar<DocumentFilesContext>,
        IToolbar<LinkedObjectsContext>,
        IToolbar<LinkedTasksContext2>,
        IToolbar<DocumentAnnotationsListContext>,
        IToolbar<SignatureRequestsContext>
    {
        public void Build(IMenuBuilder builder, DocumentAnnotationsListContext context) =>
            ContextButtonBuilder(builder, context);
        public void Build(IMenuBuilder builder, SignatureRequestsContext context) =>
            ContextButtonBuilder(builder, context);

        public void Build(IToolbarBuilder builder, DocumentAnnotationsListContext context) =>
            SelectUpdater(context);
        public void Build(IToolbarBuilder builder, SignatureRequestsContext context) =>
            SelectUpdater(context);


        public void OnMenuItemClick(string name, DocumentAnnotationsListContext context) =>
            ItemClick(name);
        public void OnMenuItemClick(string name, SignatureRequestsContext context) =>
            ItemClick(name);


        public void OnToolbarItemClick(string name, DocumentAnnotationsListContext context) { }
        public void OnToolbarItemClick(string name, SignatureRequestsContext context) { }
    }
}
