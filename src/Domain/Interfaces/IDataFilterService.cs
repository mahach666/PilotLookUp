using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IDataFilterService
    {
        Task<List<ListItemVM>> FilterDataAsync(List<ListItemVM> data, string searchText);
        List<ListItemVM> FilterData(List<ListItemVM> data, string searchText);
    }
} 