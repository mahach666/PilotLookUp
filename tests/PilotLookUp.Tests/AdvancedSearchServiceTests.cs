using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.ViewModel;
using PilotLookUp.ViewModel.Editors;
using PilotTestHost.Sdk;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Tests
{
    // Сервис продвинутого поиска и AdvancedSearchVM на живом клиенте
    public class AdvancedSearchServiceTests
    {
        private static IAdvancedSearchService CreateService(ITestContext ctx) =>
            new AdvancedSearchService(ctx.SearchService, ctx.Repository);

        private static AdvancedSearchVM CreateVM(ITestContext ctx, Action<Objects.ObjectSet> publish = null)
        {
            var clientEvents = new ClientEventService();
            var bim = new BimModelService(null, null, clientEvents, ctx.Repository);
            var repoService = new RepoService(ctx.Repository, bim);
            return new AdvancedSearchVM(CreateService(ctx), repoService, publish ?? (_ => { }));
        }

        private static void RequireSearchService(ITestContext ctx)
        {
            if (ctx.SearchService == null)
                throw new AssertFailedException("ISearchService недоступен в этом клиенте");
        }

        // Ожидание задачи на UI-потоке без его блокировки: вложенный цикл сообщений
        private static void PumpUntilCompleted(Task task, TimeSpan timeout)
        {
            var frame = new System.Windows.Threading.DispatcherFrame();

            task.ContinueWith(_ => frame.Continue = false, TaskScheduler.Default);

            var timer = new System.Windows.Threading.DispatcherTimer { Interval = timeout };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                frame.Continue = false;
            };
            timer.Start();

            System.Windows.Threading.Dispatcher.PushFrame(frame);
            timer.Stop();
        }

        [PilotFact]
        public void Clear_Leaves_One_Empty_Criterion(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var vm = CreateVM(ctx);

            vm.AddCriterionCommand.Execute(null);
            vm.AddCriterionCommand.Execute(null);
            Check.Equal(3, vm.Criteria.Count, "добавленные условия");

            vm.ClearCriteriaCommand.Execute(null);

            Check.Equal(1, vm.Criteria.Count, "после очистки остается одна пустая строка");
            Check.That(vm.Criteria[0].SelectedCategory != null, "строка сразу готова к вводу");
            Check.That(string.IsNullOrEmpty(vm.ResultsSummary), "итог прошлого поиска сброшен");
        }

        [PilotFact]
        public void Service_Is_Available_And_Caches_Categories(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var service = CreateService(ctx);

            Check.That(service.IsAvailable, "IsAvailable");
            Check.That(ReferenceEquals(service.GetCategories(), service.GetCategories()), "категории должны кэшироваться");
        }

        [PilotFact]
        public void Builder_Combines_Must_MustNot_And_AnyOf(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var service = CreateService(ctx);

            var builder = service.CreateBuilder()
                .Must(ObjectFields.AllText.Be("включаемое*"))
                .MustNot(ObjectFields.AllText.Be("исключаемое*"));
            builder.MustAnyOf(ObjectFields.AllText.Be("вариант1"), ObjectFields.AllText.Be("вариант2"));
            builder.MaxResults(10);

            var (lucene, _) = builder.Build();
            Check.Contains(lucene, "включаемое", "Must-фрагмент");
            Check.Contains(lucene, "исключаемое", "MustNot-фрагмент");
            Check.Contains(lucene, "вариант1", "AnyOf-фрагмент 1");
            Check.Contains(lucene, "вариант2", "AnyOf-фрагмент 2");
            ctx.Logger.Info("Lucene: " + lucene);
        }

        [PilotFact]
        public void RunAsync_Executes_Real_Search(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var service = CreateService(ctx);

            // Поиск по типу: берем любой реальный тип из базы
            var anyType = ctx.Repository.GetTypes().First();
            var builder = service.CreateBuilder()
                .Must(ObjectFields.TypeId.Exists())
                .MaxResults(3);

            // Результаты поиска SDK доставляет через UI-поток - блокировать его нельзя.
            // Ждем завершения, прокачивая очередь сообщений (DispatcherFrame)
            var task = service.RunAsync(builder, 3);
            PumpUntilCompleted(task, TimeSpan.FromSeconds(30));

            if (!task.IsCompleted)
                throw new AssertFailedException("поиск не завершился за 30 секунд");
            var result = task.GetAwaiter().GetResult();

            Check.NotNull(result, "результат поиска");
            Check.That(result.Ids.Count <= 3, "MaxResults соблюден");
            ctx.Logger.Info($"Найдено id: {result.Ids.Count}, всего: {result.Total}, таймаут: {result.TimedOut}, тип для справки: {anyType.Name}");
        }

        [PilotFact]
        public void VM_Builds_Live_Preview(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var vm = CreateVM(ctx);

            Check.Equal(1, vm.Criteria.Count, "стартовая строка");

            var row = vm.Criteria[0];
            ((TextValueVM)row.Editor).Text = "превью_маркер*";

            Check.Contains(vm.Preview, "превью_маркер", "превью должно перестроиться от ввода");
        }

        [PilotFact]
        public void VM_Negative_Operator_Puts_Minus_In_Preview(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var vm = CreateVM(ctx);

            var row = vm.Criteria[0];
            row.SelectedOperator = row.OperatorOptions.First(o => (CriterionOperator)o.Value == CriterionOperator.NotEquals);
            ((TextValueVM)row.Editor).Text = "минус_маркер";

            Check.Contains(vm.Preview, "минус_маркер", "значение в превью");
            Check.Contains(vm.Preview, "-", "MustNot должен давать исключающий фрагмент");
        }

        [PilotFact]
        public void VM_Raw_Lucene_Appends_To_Preview(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var vm = CreateVM(ctx);

            vm.RawLuceneQuery = "DObject\\.IsDeleted:True";
            Check.Contains(vm.Preview, "IsDeleted", "сырой Lucene попадает в превью");
        }

        [PilotFact]
        public void VM_Add_And_Remove_Criteria(ITestContext ctx)
        {
            RequireSearchService(ctx);
            var vm = CreateVM(ctx);

            vm.AddCriterionCommand.Execute(null);
            Check.Equal(2, vm.Criteria.Count, "добавление строки");

            vm.Criteria[1].RemoveCommand.Execute(null);
            Check.Equal(1, vm.Criteria.Count, "удаление строки");
        }
    }
}
