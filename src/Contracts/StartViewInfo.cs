using PilotLookUp.Enums;
using PilotLookUp.Objects;

namespace PilotLookUp.Contracts
{
    public class StartViewInfo
    {
        public PagesName PageName{ get; set; }
        public ObjectSet SelectedObject { get; set; }
    }
}
