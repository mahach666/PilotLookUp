using PilotLookUp.Enums;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelFactory
    {
        LookUpVM CreateLookUpVM(ObjectSet dataObjects = null);
        SearchVM CreateSearchVM();
        TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject);
        AttrVM CreateAttrVM(PilotObjectHelper selectedObject);
        MainVM CreateMainVM();
    }
} 