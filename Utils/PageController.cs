using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.View;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Utils
{
    internal class PageController
    {
        internal PageController(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
            ControlsHolder = new List<IControl>();
        }
        public IControl ActivePage { get; private set; }
        private List<IControl> ControlsHolder { get; }
        private LookUpModel _lookUpModel { get; }

        public void GoToPage(PagesName pageName)
        {
            if (ActivePage?.GetName() == pageName) { }
            else if (ControlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
            {
                ActivePage = ControlsHolder.FirstOrDefault(i => i.GetName() == pageName);
            }
            else
            {
                CreatePage(pageName);
            }
        }
        public void CreatePage(PagesName pageName)
        {
            switch (pageName)
            {
                case PagesName.LookUpPage:
                    ActivePage = new LookUpPage(new LookUpVM(_lookUpModel));
                    break;
            }
        }
    }
}
