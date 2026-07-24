using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Menu;
using Ascon.Pilot.SDK.Toolbar;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace PilotLookUp
{
    // MEF-точка входа плагина. Логики здесь нет: построение меню и роутинг команд -
    // в IMenuCommandHandler, перехват выделения - в ISelectedService.
    // Build меню вызывается перед показом меню, Build тулбара - при смене выделения
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
    public class App : IMenu<MainViewContext>,
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
        private const string MenuSource = "Menu";
        private const string ToolbarSource = "Toolbar";

        private readonly IMenuCommandHandler _menuHandler;
        private readonly ISelectedService _selectedService;

        [ImportingConstructor]
        public App(IObjectsRepository objectsRepository,
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService pilotDialogService,
            IPilotServiceProvider serviceProvider,
            IFileProvider fileProvider)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;

            // BIM-сервисы есть только в Pilot-BIM; в Pilot-ICE вернется пусто -
            // BIM-функции мягко отключаются (IBimModelService.IsAvailable == false)
            var modelManager = serviceProvider.GetServices<IModelManager>().FirstOrDefault();
            var modelStorageProvider = serviceProvider.GetServices<IModelStorageProvider>().FirstOrDefault();

            var container = new ServiceContainer()
                .CreateContainer(objectsRepository,
                    tabServiceProvider,
                    pilotDialogService,
                    fileProvider,
                    modelManager,
                    modelStorageProvider);

            _menuHandler = container.GetInstance<IMenuCommandHandler>();
            _selectedService = container.GetInstance<ISelectedService>();

            serviceProvider.Register<IMenu<ModelContext>>(
                new ModelContextButton(BuildModelContextMenu, _menuHandler.ItemClick));
        }

        // Контекстное меню: обновить выделение и добавить пункт LookSelected
        private void BuildContextMenu(IMenuBuilder builder, MarshalByRefObject context)
        {
            _selectedService.UpdateSelected(context, MenuSource);
            _menuHandler.BuildLookSelectedItem(builder);
        }

        private void BuildContextMenu(IMenuBuilder builder, SignatureRequestsContext context)
        {
            _selectedService.UpdateSelected(context, MenuSource);
            _menuHandler.BuildLookSelectedItem(builder);
        }

        private void BuildModelContextMenu(IMenuBuilder builder, ModelContext context)
        {
            _selectedService.UpdateSelected(context, MenuSource);
            _menuHandler.BuildLookSelectedItem(builder);
        }

        // ===== Меню =====
        public void Build(IMenuBuilder builder, MainViewContext context) => _menuHandler.BuildMainMenu(builder);
        public void Build(IMenuBuilder builder, ObjectsViewContext context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, StorageContext context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, TasksViewContext2 context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, DocumentFilesContext context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, LinkedObjectsContext context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, LinkedTasksContext2 context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, DocumentAnnotationsListContext context) => BuildContextMenu(builder, context);
        public void Build(IMenuBuilder builder, SignatureRequestsContext context) => BuildContextMenu(builder, context);

        public void OnMenuItemClick(string name, MainViewContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, ObjectsViewContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, StorageContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, TasksViewContext2 context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, DocumentFilesContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, LinkedObjectsContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, LinkedTasksContext2 context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, DocumentAnnotationsListContext context) => _menuHandler.ItemClick(name);
        public void OnMenuItemClick(string name, SignatureRequestsContext context) => _menuHandler.ItemClick(name);

        // ===== Тулбары: пунктов не добавляем, используем как перехват смены выделения =====
        public void Build(IToolbarBuilder builder, MainViewContext context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, ObjectsViewContext context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, TasksViewContext2 context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, DocumentFilesContext context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, LinkedObjectsContext context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, LinkedTasksContext2 context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, DocumentAnnotationsListContext context) => _selectedService.UpdateSelected(context, ToolbarSource);
        public void Build(IToolbarBuilder builder, SignatureRequestsContext context) => _selectedService.UpdateSelected(context, ToolbarSource);

        public void OnToolbarItemClick(string name, MainViewContext context) { }
        public void OnToolbarItemClick(string name, ObjectsViewContext context) { }
        public void OnToolbarItemClick(string name, TasksViewContext2 context) { }
        public void OnToolbarItemClick(string name, DocumentFilesContext context) { }
        public void OnToolbarItemClick(string name, LinkedObjectsContext context) { }
        public void OnToolbarItemClick(string name, LinkedTasksContext2 context) { }
        public void OnToolbarItemClick(string name, DocumentAnnotationsListContext context) { }
        public void OnToolbarItemClick(string name, SignatureRequestsContext context) { }
    }
}
