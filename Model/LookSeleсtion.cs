using Ascon.Pilot.SDK;
using PilotLookUp.View;
using PilotLookUp.VM;
using System.Collections.Generic;


namespace PilotLookUp.Model
{
    internal class LookSeleсtion
    {
        private List<PilotTypsHelper> _dataObject { get; }
        private IObjectsRepository _objectsRepository { get; }

        internal LookSeleсtion(List<PilotTypsHelper> dataObject, IObjectsRepository objectsRepository)
        {
            _dataObject = dataObject;
            _objectsRepository = objectsRepository;
            LookUpView view = new LookUpView(new LookUpVM(new LookUpModel(_dataObject, _objectsRepository)));
            view.Show();
        }
    }
}
