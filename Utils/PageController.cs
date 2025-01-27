using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View.UserControls;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PilotLookUp.Utils
{
    internal class PageController : IPageController
    {
        internal PageController(LookUpModel lookUpModel, PagesName startPage, Action<UserControl> updateCurrentPage)
        {
            _updateCurrentPage = updateCurrentPage;
            _lookUpModel = lookUpModel;
            _controlsHolder = new List<IControl>();
            if (startPage != PagesName.None)
                GoToPage(startPage);
        }
        private IControl _activePage { get; set; }
        public  UserControl ActivePage => _activePage is UserControl control ? control : null;

        private readonly Action<UserControl> _updateCurrentPage;

        private List<IControl> _controlsHolder { get; }
        private LookUpModel _lookUpModel { get; }


        public void GoToPage(PagesName pageName)
        {
            if (_controlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
            {
                _activePage = _controlsHolder.FirstOrDefault(i => i.GetName() == pageName);
                _updateCurrentPage(ActivePage);
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
                    AddPage(new SearchPage(new SearchVM(_lookUpModel)));
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
