using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Services
{
    internal class PageController : IPageService
    {
        internal PageController(LookUpModel lookUpModel
            , PagesName startPage
            , Action<IPage> updateCurrentPage)
        {
            _updateCurrentPage = updateCurrentPage;
            _lookUpModel = lookUpModel;
            _controlsHolder = new List<IPage>();
            if (startPage != PagesName.None)
                GoToPage(startPage);
        }
        private IPage _activePage { get; set; }
        public IPage ActivePage => _activePage;

        private Action<IPage> _updateCurrentPage { get; }

        private List<IPage> _controlsHolder { get; }
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
        public void CreatePage(PagesName pageName, PilotObjectHelper dataObj = null)
        {
            switch (pageName)
            {
                case PagesName.LookUpPage:
                    var lookVM = dataObj == null
                        ? new LookUpVM(_lookUpModel)
                        : _lookUpModel.GetCastomLookUpVM(dataObj);
                    AddPage(lookVM);
                    GoToPage(pageName);
                    break;
                case PagesName.DBPage:
                    var vm = _lookUpModel.GetDBLookUpVM();
                    AddPage(vm);
                    GoToPage(PagesName.LookUpPage);
                    break;
                case PagesName.SearchPage:
                    AddPage(new SearchVM(_lookUpModel, this));
                    GoToPage(pageName);
                    break;
                case PagesName.TaskTree:
                    LookUpVM selectedItemVM = _controlsHolder.FirstOrDefault(it => it.GetName() == PagesName.LookUpPage) as LookUpVM;
                    PilotObjectHelper itemOne = selectedItemVM.DataObjectSelected.PilotObjectHelper;
                    AddPage(new TaskTreeVM(_lookUpModel, itemOne));
                    GoToPage(pageName);
                    break;
            }
        }

        private void AddPage(IPage item)
        {
            _controlsHolder.RemoveAll(obj => obj.GetName() == item.GetName());
            _controlsHolder.Add(item);
        }
    }
}
