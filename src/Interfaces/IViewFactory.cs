using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IViewFactory
    {
        void LookSelection(ObjectSet selected);
        void LookDB();
        void SearchPage();
    }
}
