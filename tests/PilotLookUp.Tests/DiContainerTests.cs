using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.ViewModel;
using PilotTestHost.Sdk;

namespace PilotLookUp.Tests
{
    // Полный DI-граф собирается на живых сервисах клиента (внутри Verify)
    public class DiContainerTests
    {
        [PilotFact]
        public void Container_Builds_And_Resolves_All_Services(ITestContext ctx)
        {
            var container = new ServiceContainer().CreateContainer(
                ctx.Repository, ctx.TabService, ctx.DialogService, ctx.FileProvider, ctx.SearchService);

            using (container)
            {
                Check.NotNull(container.GetInstance<IRepoService>(), "IRepoService");
                Check.NotNull(container.GetInstance<ICustomSearchService>(), "ICustomSearchService");
                Check.NotNull(container.GetInstance<ISelectedService>(), "ISelectedService");
                Check.NotNull(container.GetInstance<IMenuCommandHandler>(), "IMenuCommandHandler");
                Check.NotNull(container.GetInstance<IViewModelFactory>(), "IViewModelFactory");
                Check.NotNull(container.GetInstance<IPageServiceFactory>(), "IPageServiceFactory");

                // Синглтоны действительно одни на контейнер
                Check.That(ReferenceEquals(
                        container.GetInstance<IClientEventService>(),
                        container.GetInstance<IClientEventService>()),
                    "IClientEventService - синглтон");

                Check.That(!container.GetInstance<IBimModelService>().IsAvailable,
                    "в ICE BIM-сервис недоступен, но зарегистрирован");
                Check.That(container.GetInstance<IAdvancedSearchService>().IsAvailable,
                    "ISearchService передан - расширенный поиск доступен");
            }
        }

        [PilotFact]
        public void AdvancedSearch_Unavailable_Without_SearchService(ITestContext ctx)
        {
            var container = new ServiceContainer().CreateContainer(
                ctx.Repository, ctx.TabService, ctx.DialogService, ctx.FileProvider, searchService: null);

            using (container)
            {
                Check.That(!container.GetInstance<IAdvancedSearchService>().IsAvailable,
                    "без ISearchService режим недоступен (кнопка скрыта)");
            }
        }

        [PilotFact]
        public void PageService_Creates_Pages_And_Guards_Missing_Selection(ITestContext ctx)
        {
            var container = new ServiceContainer().CreateContainer(
                ctx.Repository, ctx.TabService, ctx.DialogService, ctx.FileProvider, ctx.SearchService);

            using (container)
            {
                var pageService = container.GetInstance<IPageServiceFactory>()
                    .CreatePageService(new StartViewInfo { PageName = PagesName.None });

                Check.That(pageService.ActivePage == null, "стартовая страница None - ничего не создано");

                pageService.GoToPage(PagesName.EventMonitor);
                Check.That(pageService.ActivePage is EventMonitorVM, "страница EventMonitor создана");

                // TaskTree/AttrPage требуют выбранного объекта на LookUp-странице
                pageService.GoToPage(PagesName.TaskTree);
                Check.That(pageService.ActivePage is EventMonitorVM,
                    "TaskTree без выбранного объекта не создается и не роняет сервис");

                (pageService as System.IDisposable)?.Dispose();
            }
        }
    }
}
