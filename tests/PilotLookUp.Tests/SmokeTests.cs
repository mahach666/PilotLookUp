using Ascon.Pilot.SDK;
using PilotLookUp.Model.AdvancedSearch;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Smoke-тесты PilotLookUp, выполняются внутри клиента Pilot через PilotTestHost
    public class SmokeTests
    {
        [PilotFact]
        public void Repository_Available(ITestContext ctx)
        {
            if (ctx.Repository == null)
                throw new Exception("IObjectsRepository не передан в контекст");

            ctx.Logger.Info("База: " + ctx.Repository.GetDatabaseId());
            ctx.Logger.Info("Пользователь: " + ctx.Repository.GetCurrentPerson()?.DisplayName);
        }

        [PilotFact]
        public void Types_Not_Empty(ITestContext ctx)
        {
            var count = ctx.Repository.GetTypes().Count();
            ctx.Logger.Info("Типов в базе: " + count);

            if (count == 0)
                throw new Exception("В базе нет ни одного типа");
        }

        [PilotFact]
        public void FieldCatalog_Builds_Categories(ITestContext ctx)
        {
            var catalog = new FieldCatalog(ctx.Repository);
            var categories = catalog.GetCategories();

            if (categories.Count < 5)
                throw new Exception("Ожидалось не меньше 5 категорий, получено " + categories.Count);

            var attributeCategory = categories.FirstOrDefault(c => c.HasDetails);
            if (attributeCategory == null)
                throw new Exception("Нет категории с детализацией (Атрибут)");

            ctx.Logger.Info($"Категорий: {categories.Count}; детализаций в '{attributeCategory.DisplayName}': {attributeCategory.Details.Count}");
        }

        [PilotFact]
        public void SearchService_Builds_Query(ITestContext ctx)
        {
            if (ctx.SearchService == null)
            {
                ctx.Logger.Warn("ISearchService недоступен в этом клиенте — проверка пропущена");
                return;
            }

            var (lucene, _) = ctx.SearchService.GetObjectQueryBuilder()
                .Must(ObjectFields.AllText.Be("test*"))
                .MaxResults(1)
                .Build();

            if (string.IsNullOrWhiteSpace(lucene))
                throw new Exception("Построен пустой Lucene-запрос");

            ctx.Logger.Info("Lucene: " + lucene);
        }

        [PilotFact(Skip = "пример пропуска — можно удалить")]
        public void Skipped_Example()
        {
        }
    }
}
