using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly List<IPage> _pages;
        private IPage _activePage;

        public NavigationService(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            _pages = new List<IPage>();
        }

        public IPage ActivePage => _activePage;

        public event Action<IPage> PageChanged;

        public void NavigateTo(PagesName pageName)
        {
            var existingPage = _pages.FirstOrDefault(p => p.GetName() == pageName);
            if (existingPage != null)
            {
                SetActivePage(existingPage);
            }
            else
            {
                switch (pageName)
                {
                    case PagesName.LookUpPage:
                    case PagesName.DBPage:
                        NavigateToLookUp();
                        break;
                    case PagesName.SearchPage:
                        NavigateToSearch();
                        break;
                    default:
                        throw new ArgumentException($"Unknown page type: {pageName}");
                }
            }
        }

        public void NavigateToLookUp(ObjectSet dataObjects = null)
        {
            var lookUpVM = _viewModelFactory.CreateLookUpVM(dataObjects);
            
            if (dataObjects == null)
            {
                lookUpVM.InitializeDataIfNeeded();
            }
            
            AddPage(lookUpVM);
            SetActivePage(lookUpVM);
        }

        public void NavigateToSearch()
        {
            var searchVM = _viewModelFactory.CreateSearchVM(this);
            AddPage(searchVM);
            SetActivePage(searchVM);
        }

        public void NavigateToTaskTree(IPilotObjectHelper selectedObject)
        {
            var taskTreeVM = _viewModelFactory.CreateTaskTreeVM(selectedObject);
            AddPage(taskTreeVM);
            SetActivePage(taskTreeVM);
        }

        public void NavigateToAttr(IPilotObjectHelper selectedObject)
        {
            var attrVM = _viewModelFactory.CreateAttrVM(selectedObject);
            AddPage(attrVM);
            SetActivePage(attrVM);
        }

        private void AddPage(IPage page)
        {
            _pages.RemoveAll(p => p.GetName() == page.GetName());
            _pages.Add(page);
        }

        private void SetActivePage(IPage page)
        {
            _activePage = page;
            PageChanged?.Invoke(_activePage);
        }
    }
} 