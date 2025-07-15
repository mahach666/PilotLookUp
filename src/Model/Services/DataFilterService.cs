using PilotLookUp.Domain.Interfaces;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application = System.Windows.Application;

namespace PilotLookUp.Model.Services
{
    public class DataFilterService : IDataFilterService
    {
        public async Task<List<ListItemVM>> FilterDataAsync(List<ListItemVM> data, string searchText)
        {
            if (data == null)
                return new List<ListItemVM>();

            if (string.IsNullOrEmpty(searchText) || searchText.Length < 2)
            {
                return await Application.Current.Dispatcher.InvokeAsync(() => data);
            }

            var filtered = data
                .Where(i => i != null && 
                           (i.ObjName?.ToUpper().Contains(searchText.ToUpper()) == true ||
                            i.StrId?.ToUpper().Contains(searchText.ToUpper()) == true))
                .ToList();

            return await Application.Current.Dispatcher.InvokeAsync(() => filtered);
        }

        public List<ListItemVM> FilterData(List<ListItemVM> data, string searchText)
        {
            if (data == null)
                return new List<ListItemVM>();

            if (string.IsNullOrEmpty(searchText) || searchText.Length < 2)
            {
                return data;
            }

            return data
                .Where(i => i != null && 
                           (i.ObjName?.ToUpper().Contains(searchText.ToUpper()) == true ||
                            i.StrId?.ToUpper().Contains(searchText.ToUpper()) == true))
                .ToList();
        }
    }
} 