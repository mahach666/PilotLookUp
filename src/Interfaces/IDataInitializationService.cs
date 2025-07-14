using System.Collections.Generic;
using System.Threading.Tasks;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IDataInitializationService
    {
        Task<List<ListItemVM>> InitializeDataFromRepositoryAsync();
        List<ListItemVM> InitializeDataFromRepository();
        bool ValidateData(List<ListItemVM> data);
    }
} 