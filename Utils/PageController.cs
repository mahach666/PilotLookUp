using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View.UserControls;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Utils
{
    internal class PageController
    {
        internal PageController(LookUpModel lookUpModel, MainVM mainVM, PagesName startPage = PagesName.None)
        {
            _lookUpModel = lookUpModel;
            _mainVM = mainVM;
            _controlsHolder = new List<IControl>();
            if (startPage != PagesName.None)
                GoToPage(startPage);
        }
        public IControl ActivePage { get; private set; }
        private List<IControl> _controlsHolder { get; }
        private LookUpModel _lookUpModel { get; }
        private MainVM _mainVM { get; }


        public void GoToPage(PagesName pageName)
        {
            if (_controlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
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
                    AddPage(new LookUpPage(new LookUpVM(_lookUpModel)));
                    GoToPage(pageName);
                    break;
                case PagesName.DBPage:
                    var vm = _lookUpModel.GetDBLookUpVM();
                    AddPage(new LookUpPage(vm));
                    GoToPage(PagesName.LookUpPage);
                    break;
                case PagesName.SearchPage:
                    AddPage(new SearchPage(new SearchVM(_lookUpModel, _mainVM)));
                    GoToPage(pageName);
                    break;
            }
        }

        public void CreatePage(PagesName pageName, PilotObjectHelper dataObj)
        {
            switch (pageName)
            {
                case PagesName.LookUpPage:
                    var vm = _lookUpModel.GetCastomLookUpVM(dataObj);
                    AddPage(new LookUpPage(vm));
                    GoToPage(PagesName.LookUpPage);
                    break;
            }
        }

        private void AddPage(IControl item)
        {
            _controlsHolder.RemoveAll(obj => obj.GetName() == item.GetName());
            _controlsHolder.Add(item);
        }
    }
}
