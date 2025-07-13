using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelProvider
    {
        LookUpVM CreateLookUpVM(ObjectSet dataObjects = null);
        SearchVM CreateSearchVM(INavigationService navigationService);
        TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject);
        AttrVM CreateAttrVM(PilotObjectHelper selectedObject);
        MainVM CreateMainVM(INavigationService navigationService, IViewModelFactory viewModelFactory);
    }
} 