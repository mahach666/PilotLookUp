using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class PageService : IPageService
    {
        private IRepoService _repoService { get; }
        private ICustomSearchService _searchService { get; }
        private ITabService _tabService { get; }
        private IViewModelFactory _viewModelFactory { get; }

        public PageService(
             StartViewInfo startViewInfo,
             IRepoService repoService,
             ICustomSearchService searchService,
             ITabService tabService, 
             IViewModelFactory viewModelFactory)
        {
            _repoService = repoService;
            _searchService = searchService;
            _tabService = tabService;
            _viewModelFactory = viewModelFactory;
            _controlsHolder = new List<IPage>();

            if (startViewInfo.PageName != PagesName.None)
                CreatePage(startViewInfo.PageName, startViewInfo.SelectedObject);
        }

        public event Action<IPage> PageChanged;

        private IPage _activePage { get; set; }
        public IPage ActivePage { get { return _activePage; } }

        private List<IPage> _controlsHolder { get; }



        public void GoToPage(PagesName pageName)
        {
            if (_controlsHolder.FirstOrDefault(i => i.GetName() == pageName) != null)
            {
                _activePage = _controlsHolder.FirstOrDefault(i => i.GetName() == pageName);
                PageChanged?.Invoke(_activePage);
            }
            else
            {
                CreatePage(pageName);
            }
        }
        public void CreatePage(PagesName pageName, ObjectSet dataObj = null)
        {
            LookUpVM selectedItemVM;
            PilotObjectHelper itemOne;

            switch (pageName)
            {
                case PagesName.LookUpPage:
                    var lookVM = dataObj == null
                        ? _viewModelFactory.CreateLookUpVM()
                        : _viewModelFactory.CreateLookUpVM(dataObj);
                    AddPage(lookVM);
                    GoToPage(pageName);
                    break;
                case PagesName.DBPage:
                    var vm = GetDBLookUpVM();
                    AddPage(vm);
                    GoToPage(PagesName.LookUpPage);
                    break;
                case PagesName.SearchPage:
                    AddPage(new SearchVM(this, _searchService, _tabService, _viewModelFactory));
                    GoToPage(pageName);
                    break;
                case PagesName.TaskTree:
                    selectedItemVM = _controlsHolder.FirstOrDefault(it => it.GetName() == PagesName.LookUpPage) as LookUpVM;
                    itemOne = selectedItemVM.DataObjectSelected.PilotObjectHelper;
                    AddPage(_viewModelFactory.CreateTaskTreeVM(itemOne));
                    GoToPage(pageName);
                    break;
                case PagesName.AttrPage:
                    selectedItemVM = _controlsHolder.FirstOrDefault(it => it.GetName() == PagesName.LookUpPage) as LookUpVM;
                    itemOne = selectedItemVM.DataObjectSelected.PilotObjectHelper;
                    AddPage(_viewModelFactory.CreateAttrVM(itemOne));
                    GoToPage(pageName);
                    break;
            }
        }

        private void AddPage(IPage item)
        {
            _controlsHolder.RemoveAll(obj => obj.GetName() == item.GetName());
            _controlsHolder.Add(item);
        }

        private LookUpVM GetDBLookUpVM()
        {
            var repo = _repoService.GetWrapedRepo();
            return _viewModelFactory.CreateLookUpVM(repo);
        }
    }
}
