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
        internal PageController(LookUpModel lookUpModel, PagesName startPage = PagesName.None)
        {
            _lookUpModel = lookUpModel;
            _controlsHolder = new List<IControl>();
            if (startPage != PagesName.None)
                GoToPage(startPage);
        }
        public IControl ActivePage { get; private set; }
        private List<IControl> _controlsHolder { get; }
        private LookUpModel _lookUpModel { get; }

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
                    //var pilotObjectMap = new PilotObjectMap(_lookUpModel._objectsRepository);
                    //var repo = new ObjectSet(null) { pilotObjectMap.Wrap(_lookUpModel._objectsRepository) };
                    //vm.SelectionDataObjects = repo;
                    var vm = _lookUpModel.GetDBVM();
                    AddPage(new LookUpPage(vm));
                    GoToPage(PagesName.LookUpPage);
                    break;
                case PagesName.SearchPage:
                    AddPage(new SearchPage(new SearchVM(_lookUpModel)));
                    GoToPage(pageName);
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
