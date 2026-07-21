using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using System;

namespace PilotLookUp
{
    public partial class App
    {
        private void ContextButtonBuilder(IMenuBuilder builder, MarshalByRefObject context)
        {
            SelectUpdater(context, "Menu");
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }
        private void ContextButtonBuilder(IMenuBuilder builder, SignatureRequestsContext context)
        {
            SelectUpdater(context, "Menu");
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }
        private void ContextButtonBuilder(IMenuBuilder builder, ModelContext context)
        {
            SelectUpdater(context, "Menu");
            builder.AddItem("LookSelected", 0).WithHeader("LookSelected");
        }

        private void SelectUpdater(MarshalByRefObject context, string source = null) =>
            _selectedService.UpdateSelected(context, source);
        private void SelectUpdater(SignatureRequestsContext context, string source = null) =>
            _selectedService.UpdateSelected(context, source);
        private void SelectUpdater(ModelContext context, string source = null) =>
            _selectedService.UpdateSelected(context, source);

        // Build
        public void Build(IMenuBuilder builder, MainViewContext context)
        {
            var item = builder.AddItem("PilotLookUp", 1).WithHeader("PilotLookUp");
            item.WithSubmenu().AddItem("LookSelected", 0).WithHeader("LookSelected");
            item.WithSubmenu().AddItem("LookDB", 1).WithHeader("LookDB");
            item.WithSubmenu().AddItem("Search", 2).WithHeader("Search");
            item.WithSubmenu().AddItem("EventMonitor", 3).WithHeader("EventMonitor");
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
        public void Build(IMenuBuilder builder, DocumentAnnotationsListContext context) =>
            ContextButtonBuilder(builder, context);
        public void Build(IMenuBuilder builder, SignatureRequestsContext context) =>
            ContextButtonBuilder(builder, context);
    }
}
