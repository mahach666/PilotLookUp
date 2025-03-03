using PilotLookUp.Enums;
using PilotLookUp.Objects;
using System;

namespace PilotLookUp.Interfaces
{
    internal interface IPageService
    {
        public IPage ActivePage { get; }
        public event Action<IPage> PageChanged;
        public void GoToPage(PagesName pageName);
        public void CreatePage(PagesName pageName, ObjectSet dataObj = null);
    }
}
