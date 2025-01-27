using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View;
using PilotLookUp.ViewModel;


namespace PilotLookUp.ViewBuilders
{
    public class LookSeleсtion
    {
        public LookSeleсtion(ObjectSet dataObjects, IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            if (!dataObjects.IsLookable) return;

            var model = new LookUpModel(dataObjects, objectsRepository, tabServiceProvider);
            var vm = new MainVM(model, PagesName.LookUpPage);
            var view = new MainView(vm);

            view.Show();
        }

        public LookSeleсtion(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            var pilotObjectMap = new PilotObjectMap(objectsRepository);
            var repo = new ObjectSet(null) { pilotObjectMap.Wrap(objectsRepository) };

            var model = new LookUpModel(repo, objectsRepository, tabServiceProvider);
            var vm = new MainVM(model, PagesName.LookUpPage);
            var view = new MainView(vm);

            view.Show();
        }
    }
}
