using PilotLookUp.Model;
using PilotLookUp.View;

namespace PilotLookUp.VM
{
    internal class LookUpVM
    {
        internal LookUpView _view;
        private LookUpModel lookUpModel;

        public LookUpVM(LookUpModel lookUpModel)
        {
            this.lookUpModel = lookUpModel;
        }
    }
}