using Ascon.Pilot.SDK;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotTestHost.Sdk;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Перехват выделения через реальный ObjectsViewContext (публичный ctor SDK)
    public class SelectedServiceTests
    {
        private static (SelectedService Service, List<object> Raised) Create(ITestContext ctx)
        {
            var clientEvents = new ClientEventService();
            var raised = new List<object>();
            clientEvents.EventRaised += r => raised.Add(r);

            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            return (new SelectedService(ctx.Repository, clientEvents, bim), raised);
        }

        [PilotFact]
        public void Selection_Raises_Record_With_Ids_And_Flags(ITestContext ctx)
        {
            var (service, raised) = Create(ctx);
            var root = TestData.GetRoot(ctx);

            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");

            var record = raised.OfType<SelectionChangedRecord>().Single();
            Check.Equal("Menu", record.Source, "источник");
            Check.Equal("ObjectsViewContext", record.ContextName, "имя контекста");
            Check.Equal(false, record.IsContext, "флаг IsContext из контекста");
            Check.Equal(1, record.SelectedCount, "число выделенных");
            Check.Equal(root.Id, record.SelectedObjectIds.Single(), "id выделенного объекта");
            Check.NotNull(service.Selected, "Selected заполнен");
            Check.Equal(1, service.Selected.Count, "Selected содержит объект");
        }

        [PilotFact]
        public void Shortcuts_Are_Merged_And_Counted(ITestContext ctx)
        {
            var (service, raised) = Create(ctx);
            var root = TestData.GetRoot(ctx);

            service.UpdateSelected(new ObjectsViewContext(new[] { root }, true, new[] { root }), "Toolbar");

            var record = raised.OfType<SelectionChangedRecord>().Single();
            Check.Equal(1, record.ShortcutsCount, "число ярлыков");
            Check.Equal(2, record.SelectedCount, "объекты + ярлыки вместе");
        }

        [PilotFact]
        public void Same_Selection_From_Same_Source_Is_Deduplicated(ITestContext ctx)
        {
            var (service, raised) = Create(ctx);
            var root = TestData.GetRoot(ctx);

            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");
            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");

            Check.Equal(1, raised.OfType<SelectionChangedRecord>().Count(), "повтор из того же источника гасится");
        }

        [PilotFact]
        public void Same_Selection_From_Other_Source_Is_Not_Deduplicated(ITestContext ctx)
        {
            var (service, raised) = Create(ctx);
            var root = TestData.GetRoot(ctx);

            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");
            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Toolbar");

            Check.Equal(2, raised.OfType<SelectionChangedRecord>().Count(),
                "источник входит в ключ дедупликации");
        }

        [PilotFact]
        public void Empty_Selection_Raises_Once_And_Resets_Dedup(ITestContext ctx)
        {
            var (service, raised) = Create(ctx);
            var root = TestData.GetRoot(ctx);

            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");
            service.UpdateSelected(new ObjectsViewContext(new IDataObject[0], false, null), "Menu");
            service.UpdateSelected(new ObjectsViewContext(new IDataObject[0], false, null), "Menu");

            var records = raised.OfType<SelectionChangedRecord>().ToList();
            Check.Equal(2, records.Count, "сброс выделения дает ровно одно событие");
            Check.Equal(0, records[1].SelectedCount, "запись о сбросе с нулем объектов");

            // После сброса то же выделение снова генерирует событие
            service.UpdateSelected(new ObjectsViewContext(new[] { root }, false, null), "Menu");
            Check.Equal(3, raised.OfType<SelectionChangedRecord>().Count(),
                "дедупликация сброшена пустым выделением");
        }
    }
}
