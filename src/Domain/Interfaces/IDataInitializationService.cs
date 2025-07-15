using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IDataInitializationService
    {
        Task<List<ListItemVM>> InitializeDataFromRepositoryAsync();
        List<ListItemVM> InitializeDataFromRepository();
        bool ValidateData(List<ListItemVM> data);
    }
} 