using PilotLookUp.Core.Objects;

namespace PilotLookUp.Core.Interfaces
{
    public interface IViewModelFactory
    {
        object CreateLookUpVM();
        object CreateSearchVM();
        object CreateTaskTreeVM(PilotObjectHelper pilotObjectHelper);
        object CreateAttrVM(PilotObjectHelper pilotObjectHelper);
        object CreateListItemVM(PilotObjectHelper pilotObjectHelper);
        object CreateSearchResVM(PilotObjectHelper pilotObjectHelper);
    }
} 