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

            LookUpView view = new LookUpView(new LookUpVM(new LookUpModel(dataObjects, objectsRepository)));
            view.Show();
        }
    }
}
