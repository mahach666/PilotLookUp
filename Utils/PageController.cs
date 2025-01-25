using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.View.UserControls;
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
            _controlsHolder = new List<IControl>();
        }
        public IControl ActivePage { get; private set; }
        private List<IControl> _controlsHolder { get; }
        private LookUpModel _lookUpModel { get; }

        public void GoToPage(PagesName pageName)
        {
            if (ActivePage?.GetName() == pageName) { }
            else if (_controlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
            {
                ActivePage = _controlsHolder.FirstOrDefault(i => i.GetName() == pageName);
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
                    AddPage(_controlsHolder, new LookUpPage(new LookUpVM(_lookUpModel)));
                    GoToPage(pageName);
                    break;
            }
        }

        private void AddPage(List<IControl> list, IControl item)
        {
            list.RemoveAll(obj => obj.GetName() == item.GetName());
            list.Add(item);
        }
    }
}
