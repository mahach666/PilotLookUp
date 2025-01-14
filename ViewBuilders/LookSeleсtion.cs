using Ascon.Pilot.SDK;
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
            var lookUpModel = new LookUpModel(dataObjects, objectsRepository);
            var lookUpVM = new LookUpVM(lookUpModel);
            var view = new LookUpView(lookUpVM);
            view.Show();
        }
    }
}
