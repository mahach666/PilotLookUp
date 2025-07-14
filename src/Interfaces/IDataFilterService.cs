using System.Collections.Generic;
using System.Threading.Tasks;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface IDataFilterService
    {
        Task<List<ListItemVM>> FilterDataAsync(List<ListItemVM> data, string searchText);
        List<ListItemVM> FilterData(List<ListItemVM> data, string searchText);
    }
} 