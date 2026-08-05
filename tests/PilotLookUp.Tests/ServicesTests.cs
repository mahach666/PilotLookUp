using Ascon.Pilot.SDK;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Сервисный слой на живом клиенте
    public class ServicesTests
    {
        private static RepoService CreateRepoService(ITestContext ctx)
        {
            var clientEvents = new ClientEventService();
            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            return new RepoService(ctx.Repository, bim);
        }

        [PilotFact]
        public void RepoService_GetObjInfo_Reflects_Root_Object(ITestContext ctx)
        {
            var repoService = CreateRepoService(ctx);
            var rootHelper = new PilotObjectMap(ctx.Repository).Wrap(TestData.GetRoot(ctx));

            var info = TestPump.Await(repoService.GetObjInfo(rootHelper), 60);

            Check.That(info.Count > 5, $"рефлексия объекта должна дать строки (получено {info.Count})");
            Check.That(info.Any(set => set.SenderMemberName == "Id"), "должна быть строка Id");
        }

        [PilotFact]
        public void RepoService_GetWrapedObjs_Resolves_Real_Id(ITestContext ctx)
        {
            var repoService = CreateRepoService(ctx);
            var rootId = SystemObjectIds.RootObjectId;

            var set = TestPump.Await(repoService.GetWrapedObjs(new[] { rootId }));

            Check.Equal(1, set.Count, "один id - один объект");
            Check.That(set[0] is DataObjectHelper, "id объекта БД разворачивается в DataObjectHelper");
        }

        [PilotFact]
        public void SearchService_Finds_By_Guid_And_Int(ITestContext ctx)
        {
            var clientEvents = new ClientEventService();
            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            var searchService = new SearchService(ctx.Repository, bim);

            var byGuid = TestPump.Await(searchService.GetObjByString(SystemObjectIds.RootObjectId.ToString()));
            Check.That(byGuid != null && byGuid.Count > 0, "поиск по GUID корня");

            var personId = ctx.Repository.GetCurrentPerson().Id;
            var byInt = TestPump.Await(searchService.GetObjByString(personId.ToString()));
            Check.That(byInt != null && byInt.Count > 0, "поиск по int (человек/оргединица/тип)");

            var garbage = TestPump.Await(searchService.GetObjByString("просто текст"));
            Check.That(garbage == null, "мусорный запрос дает null");
        }

        [PilotFact]
        public void DataObjectService_Returns_Attributes_For_Real_Object(ITestContext ctx)
        {
            var service = new DataObjectService();
            var rootHelper = new PilotObjectMap(ctx.Repository).Wrap(TestData.GetRoot(ctx)) as DataObjectHelper;

            var attrs = service.GetAttrDTOs(rootHelper).ToList();
            ctx.Logger.Info($"Атрибутов у корня: {attrs.Count}");

            // Не-объект дает пустой список, а не NRE
            var empty = service.GetAttrDTOs(null).ToList();
            Check.Equal(0, empty.Count, "null-хелпер дает пустой список");
        }

        [PilotFact]
        public void TabService_GoTo_Null_Does_Not_Crash(ITestContext ctx)
        {
            new TabService(ctx.TabService).GoTo(null);
        }

        [PilotFact]
        public void ClientEventService_Delivers_Events()
        {
            var service = new ClientEventService();
            object received = null;
            service.EventRaised += r => received = r;

            var record = new MenuItemClickRecord("клик");
            service.Raise(record);

            Check.That(ReferenceEquals(record, received), "событие должно дойти подписчику как есть");
        }

        [PilotFact]
        public void BimModelService_Degrades_Gracefully_Without_Bim(ITestContext ctx)
        {
            var service = new BimModelService(null, null, new ClientEventService(), ctx.Repository);

            Check.That(!service.IsAvailable, "в ICE BIM недоступен");
            Check.That(service.GetStorage(Guid.NewGuid()) == null, "хранилище недоступно");
            Check.That(service.TryGetViewer(Guid.NewGuid()) == null, "вьюер не отслеживается");
            Check.That(service.TryLoadElementView(new ModelElementIdStub(Guid.NewGuid(), Guid.NewGuid())) == null,
                "резолв по интерфейсу дает null");

            // Guid-перегрузка возвращает диагностику вместо null
            var diagnostic = service.TryLoadElementView(Guid.NewGuid(), Guid.NewGuid());
            Check.NotNull(diagnostic, "диагностический результат");
            Check.Contains(diagnostic.LoadError, "недоступны", "причина отказа в LoadError");
        }
    }
}
