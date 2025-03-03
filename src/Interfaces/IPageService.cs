using PilotLookUp.Enums;
using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    internal interface IPageService
    {
        public IPage ActivePage { get; }
        public void GoToPage(PagesName pageName);
        public void CreatePage(PagesName pageName, PilotObjectHelper dataObj = null);
    }
}
