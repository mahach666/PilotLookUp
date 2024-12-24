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
        private List<PilotObjectHelper> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }

        public LookSeleсtion(ObjectSet dataObjects, IObjectsRepository objectsRepository)
        {
            if (!dataObjects.IsLookable) return;

            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
            LookUpView view = new LookUpView(new LookUpVM(new LookUpModel(_dataObjects, _objectsRepository)));
            view.Show();
        }
    }
}
