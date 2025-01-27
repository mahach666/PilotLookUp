using PilotLookUp.Enums;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View.UserControls;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    internal interface IPageController
    {
        public void GoToPage(PagesName pageName);
        public void CreatePage(PagesName pageName, PilotObjectHelper dataObj = null);
    }
}
