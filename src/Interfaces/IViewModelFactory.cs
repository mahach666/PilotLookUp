using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using PilotLookUp.Contracts;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelFactory
    {
        MainVM CreateMainVM(StartViewInfo startInfo);
        LookUpVM CreateLookUpVM();
        
        // SearchVM не создается через фабрику - создается в PageService.CreatePage
        // SearchVM CreateSearchVM();
        
        // ViewModels ниже не создаются через фабрику, так как требуют конкретные параметры:
        // TaskTreeVM - создается с конкретным PilotObjectHelper в PageService
        // AttrVM - создается с конкретным PilotObjectHelper в PageService
        // SearchResVM - создается с конкретными параметрами в SearchVM
        // ListItemVM - создается с конкретным PilotObjectHelper в PageService
    }
} 