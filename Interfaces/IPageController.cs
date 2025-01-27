using PilotLookUp.Enums;
using PilotLookUp.Objects;
using System.Windows.Controls;

namespace PilotLookUp.Interfaces
{
    internal interface IPageController
    {
        public UserControl ActivePage { get; }
        public void GoToPage(PagesName pageName);
        public void CreatePage(PagesName pageName, PilotObjectHelper dataObj = null);
    }
}
