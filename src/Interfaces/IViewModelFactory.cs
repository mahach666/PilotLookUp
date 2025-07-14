using PilotLookUp.Enums;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelFactory
    {
        LookUpVM CreateLookUpVM(ObjectSet dataObjects = null);
        SearchVM CreateSearchVM(INavigationService navigationService);
        TaskTreeVM CreateTaskTreeVM(IPilotObjectHelper selectedObject);
        AttrVM CreateAttrVM(IPilotObjectHelper selectedObject);
        MainVM CreateMainVM(INavigationService navigationService);
        SearchResVM CreateSearchResVM(INavigationService navigationService, IPilotObjectHelper pilotObjectHelper);
    }
} 