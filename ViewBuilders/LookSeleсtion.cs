using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using System.Collections.Generic;


namespace PilotLookUp.ViewBuilders
{
    public class LookSeleсtion
    {
        public LookSeleсtion(ObjectSet dataObjects, IObjectsRepository objectsRepository)
        {
            if (!dataObjects.IsLookable) return;

            var model = new LookUpModel(dataObjects, objectsRepository);
            var vm = new MainVM(model, PagesName.LookUpPage);
            LookUpView view = new LookUpView(vm);
            view.Show();
        }
    }
}
