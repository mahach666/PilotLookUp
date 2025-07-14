using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelProvider
    {
        LookUpVM CreateLookUpVM(ObjectSet dataObjects = null, IErrorHandlingService errorHandlingService = null);
        SearchVM CreateSearchVM(INavigationService navigationService);
        TaskTreeVM CreateTaskTreeVM(IPilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null);
        AttrVM CreateAttrVM(IPilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null);
        MainVM CreateMainVM(INavigationService navigationService, IViewModelFactory viewModelFactory, IErrorHandlingService errorHandlingService = null);
    }
} 