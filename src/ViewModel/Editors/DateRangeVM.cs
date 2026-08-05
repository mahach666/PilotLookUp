using PilotLookUp.Model.AdvancedSearch;
using System;
using System.Collections.Generic;

namespace PilotLookUp.ViewModel.Editors
{
    public sealed class DateRangeVM : EditorVMBase
    {
        public IReadOnlyList<DatePreset> Presets { get; } = DatePreset.All;

        private DatePreset _selectedPreset = DatePreset.All[0];
        public DatePreset SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                _selectedPreset = value;
                OnPropertyChanged();

                var range = value?.GetRange();
                if (range != null)
                {
                    From = range.Value.From;
                    To = range.Value.To;
                }
            }
        }

        private DateTime? _from;
        public DateTime? From
        {
            get => _from;
            set { _from = value; OnPropertyChanged(); }
        }

        private DateTime? _to;
        public DateTime? To
        {
            get => _to;
            set { _to = value; OnPropertyChanged(); }
        }
    }
}
