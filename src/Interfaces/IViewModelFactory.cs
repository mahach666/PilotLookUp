using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Interfaces
{
    public interface IViewModelFactory
    {
        MainVM CreateMainVM(StartViewInfo startInfo);
        
        // LookUpVM создается через фабрику
        LookUpVM CreateLookUpVM();
        LookUpVM CreateLookUpVM(ObjectSet objectSet);
        
        // SearchVM создается напрямую в PageService (циклическая зависимость)
        // SearchVM CreateSearchVM(IPageService pageService);
        
        // ViewModels с конкретными параметрами
        TaskTreeVM CreateTaskTreeVM(PilotObjectHelper pilotObjectHelper);
        AttrVM CreateAttrVM(PilotObjectHelper pilotObjectHelper);
        
        // ListItemVM для коллекций
        ListItemVM CreateListItemVM(PilotObjectHelper pilotObjectHelper);
        
        // SearchResVM создается в SearchVM
        SearchResVM CreateSearchResVM(IPageService pageService, PilotObjectHelper pilotObjectHelper);
    }
} 