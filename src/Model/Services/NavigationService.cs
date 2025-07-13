using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IViewModelProvider _viewModelProvider;
        private readonly ISearchViewModelCreator _searchViewModelCreator;
        private readonly List<IPage> _pages;
        private IPage _activePage;

        public NavigationService(IViewModelProvider viewModelProvider, ISearchViewModelCreator searchViewModelCreator)
        {
            _viewModelProvider = viewModelProvider;
            _searchViewModelCreator = searchViewModelCreator;
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
                // Создаем новую страницу в зависимости от типа
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
            var lookUpVM = _viewModelProvider.CreateLookUpVM(dataObjects);
            
            // Если данные не были переданы, инициализируем из репозитория
            if (dataObjects == null)
            {
                lookUpVM.InitializeDataIfNeeded();
            }
            
            AddPage(lookUpVM);
            SetActivePage(lookUpVM);
        }

        public void NavigateToSearch()
        {
            var searchVM = _searchViewModelCreator.CreateSearchVM(this);
            AddPage(searchVM);
            SetActivePage(searchVM);
        }

        public void NavigateToTaskTree(PilotObjectHelper selectedObject)
        {
            var taskTreeVM = _viewModelProvider.CreateTaskTreeVM(selectedObject);
            AddPage(taskTreeVM);
            SetActivePage(taskTreeVM);
        }

        public void NavigateToAttr(PilotObjectHelper selectedObject)
        {
            var attrVM = _viewModelProvider.CreateAttrVM(selectedObject);
            AddPage(attrVM);
            SetActivePage(attrVM);
        }

        private void AddPage(IPage page)
        {
            // Удаляем существующую страницу того же типа
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