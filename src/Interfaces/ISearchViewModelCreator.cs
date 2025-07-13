using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface ISearchViewModelCreator
    {
        SearchVM CreateSearchVM(INavigationService navigationService);
    }
} 