using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.Services;
using PilotLookUp.View;
using PilotLookUp.ViewModel;


namespace PilotLookUp
{
    public static class ViewBuilder
    {
        public static void LookSeleсtion(ObjectSet selectedObjects, IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            if (!selectedObjects.IsLookable) return;

            var model = new LookUpModel(objectsRepository, tabServiceProvider);
            var pageController = new PageController(model, PagesName.LookUpPage, selectedObjects);
            var vm = new MainVM(pageController);
            var view = new MainView(vm);

            view.Show();
        }

        public static void LookDB(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            var pilotObjectMap = new PilotObjectMap(objectsRepository);
            var repo = new ObjectSet(null) { pilotObjectMap.Wrap(objectsRepository) };

            var model = new LookUpModel(objectsRepository, tabServiceProvider);
            var pageController = new PageController(model, PagesName.DBPage);
            var vm = new MainVM(pageController);
            var view = new MainView(vm);

            view.Show();
        }

        public static void SearchPage(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            var emptySet = new ObjectSet(null);
            var model = new LookUpModel(objectsRepository, tabServiceProvider);
            var pageController = new PageController(model, PagesName.SearchPage);
            var vm = new MainVM(pageController);
            var view = new MainView(vm);

            view.Show();
        }
    }
}
