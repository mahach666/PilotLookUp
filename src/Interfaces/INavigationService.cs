using PilotLookUp.Enums;
using PilotLookUp.Objects;
using System;

namespace PilotLookUp.Interfaces
{
    public interface INavigationService
    {
        IPage ActivePage { get; }
        event Action<IPage> PageChanged;
        
        void NavigateTo(PagesName pageName);
        void NavigateToLookUp(ObjectSet dataObjects = null);
        void NavigateToSearch();
        void NavigateToTaskTree(IPilotObjectHelper selectedObject);
        void NavigateToAttr(IPilotObjectHelper selectedObject);
    }
} 