using Ascon.Pilot.SDK;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Лента событий: категории, наполнение, фильтры, отписка
    public class EventMonitorTests
    {
        [PilotFact]
        public void Notification_Kinds_Map_To_Categories()
        {
            Check.Equal("Objects", EventMonitorItemVM.GetNotificationCategory(NotificationKind.ObjectCreated), "Object*");
            Check.Equal("Tasks", EventMonitorItemVM.GetNotificationCategory(NotificationKind.TaskCreated), "Task*");
            Check.Equal("Storage", EventMonitorItemVM.GetNotificationCategory(NotificationKind.StorageObjectCreated), "Storage*");
        }

        [PilotFact]
        public void Client_Records_Produce_Items_With_Right_Category(ITestContext ctx)
        {
            var menuItem = new EventMonitorItemVM(new MenuItemClickRecord("LookDB"), ctx.Repository);
            Check.Equal("Client", menuItem.Category, "категория клика меню");
            Check.Equal("MenuItemClick", menuItem.Kind, "вид события");
            Check.Equal("LookDB", menuItem.Title, "заголовок - имя пункта");
            Check.That(menuItem.Helper != null && menuItem.Helper.IsLookable, "запись снупается");

            var selection = new SelectionChangedRecord("Menu", "ObjectsViewContext", false, new[] { Guid.NewGuid() }, 0, 1);
            var selectionItem = new EventMonitorItemVM(selection, ctx.Repository);
            Check.Equal("Client", selectionItem.Category, "категория выделения");
            Check.Contains(selectionItem.Title, "выделено 1", "заголовок с количеством");

            var bim = new BimSelectionChangedRecord(Guid.NewGuid(), Guid.NewGuid(), null);
            var bimItem = new EventMonitorItemVM(bim, ctx.Repository);
            Check.Equal("Bim", bimItem.Category, "категория BIM-события");
        }

        [PilotFact]
        public void VM_Receives_Filters_And_Clears_Events(ITestContext ctx)
        {
            var clientEvents = new ClientEventService();
            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            var repoService = new RepoService(ctx.Repository, bim);

            var vm = new EventMonitorVM(ctx.Repository, repoService, null, clientEvents);
            try
            {
                clientEvents.Raise(new MenuItemClickRecord("Search"));
                clientEvents.Raise(new SelectionErrorRecord("тестовая ошибка"));
                TestPump.DoEvents();

                Check.Equal(2, vm.Events.Count, "оба события попали в ленту");

                // Фильтр по тексту
                vm.TextFilter = "Search";
                Check.Equal(1, vm.EventsView.Cast<object>().Count(), "текстовый фильтр");

                vm.TextFilter = null;
                vm.SelectedCategory = "Bim";
                Check.Equal(0, vm.EventsView.Cast<object>().Count(), "фильтр по категории отсекает Client");

                vm.SelectedCategory = "Все";
                vm.ClearCommand.Execute(null);
                Check.Equal(0, vm.Events.Count, "очистка ленты");
            }
            finally
            {
                vm.Dispose();
            }
        }

        [PilotFact]
        public void Dispose_Unsubscribes_From_Client_Events(ITestContext ctx)
        {
            var clientEvents = new ClientEventService();
            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            var repoService = new RepoService(ctx.Repository, bim);

            var vm = new EventMonitorVM(ctx.Repository, repoService, null, clientEvents);
            vm.Dispose();

            clientEvents.Raise(new MenuItemClickRecord("после dispose"));
            TestPump.DoEvents();

            Check.Equal(0, vm.Events.Count, "после Dispose события не приходят");
        }
    }
}
