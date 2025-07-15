using PilotLookUp.Domain.Entities;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Domain.Interfaces
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