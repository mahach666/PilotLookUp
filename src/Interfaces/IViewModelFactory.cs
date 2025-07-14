using PilotLookUp.Enums;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelFactory
    {
        LookUpVM CreateLookUpVM(ObjectSet dataObjects = null, IErrorHandlingService errorHandlingService = null);
        SearchVM CreateSearchVM(IErrorHandlingService errorHandlingService = null);
        TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null);
        AttrVM CreateAttrVM(PilotObjectHelper selectedObject, IErrorHandlingService errorHandlingService = null);
        MainVM CreateMainVM(IErrorHandlingService errorHandlingService = null);
        SearchResVM CreateSearchResVM(PilotObjectHelper pilotObjectHelper);
    }
} 