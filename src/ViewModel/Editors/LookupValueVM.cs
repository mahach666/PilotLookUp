using PilotLookUp.Model.AdvancedSearch;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.ViewModel.Editors
{
    // Значение из справочника (люди, типы, оргединицы, состояния);
    // выбор - через SearchableDropDown с поиском
    public sealed class LookupValueVM : EditorVMBase
    {
        public LookupValueVM(IEnumerable<LookupItem> items)
        {
            Items = items?.ToList() ?? new List<LookupItem>();
        }

        public IReadOnlyList<LookupItem> Items { get; }

        private LookupItem _selectedItem;
        public LookupItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public IReadOnlyList<LookupItem> GetSelectedLookupItems() =>
            SelectedItem == null ? new List<LookupItem>() : new List<LookupItem> { SelectedItem };
    }
}
