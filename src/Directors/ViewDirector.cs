using Ascon.Pilot.SDK;
using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using SimpleInjector;


namespace PilotLookUp
{
    public static class ViewDirector
    {
        public static void LookSelection(
            ObjectSet selectedObjects
            , IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            if (!selectedObjects.IsLookable) return;

            var startInfo = new StartViewInfo()
            {
                PageName = PagesName.LookUpPage,
                SelectedObject = selectedObjects
            };

            ShowView(objectsRepository, tabServiceProvider, startInfo);
        }

        public static void LookDB(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            var startInfo = new StartViewInfo()
            { PageName = PagesName.DBPage };

            ShowView(objectsRepository, tabServiceProvider, startInfo);
        }

        public static void SearchPage(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider)
        {
            var startInfo = new StartViewInfo()
            { PageName = PagesName.SearchPage };

            ShowView(objectsRepository, tabServiceProvider, startInfo);
        }

        private static void ShowView(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , StartViewInfo startViewInfo)
        {
            var container = ConfigureContainer(objectsRepository, tabServiceProvider, startViewInfo);

            var window = container
         .GetInstance<MainView>();

            window.Show();
        }

        private static Container ConfigureContainer(
            IObjectsRepository objectsRepository
            , ITabServiceProvider tabServiceProvider
            , StartViewInfo startViewInfo)
        {
            var container = new Container();

            container.RegisterInstance(objectsRepository);
            container.RegisterInstance(tabServiceProvider);
            container.RegisterInstance(startViewInfo);

            container.Register<IRepoService, RepoService>();
            container.Register<ICastomSearchService, SearchService>();
            container.Register<ITabService, TabService>();
            container.Register<IWindowService, WindowService>();
            container.Register<ITreeItemService, TreeItemService>();
            container.Register<IPageService, PageService>();
            container.Register<MainVM>();
            container.Register<MainView>();

            return container;
        }
    }
}
