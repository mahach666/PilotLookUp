using PilotLookUp.Enums;
using PilotLookUp.Core.Objects;

namespace PilotLookUp.Contracts
{
    public class StartViewInfo
    {
        public PagesName PageName{ get; set; }
        public ObjectSet SelectedObject { get; set; }
    }
}
