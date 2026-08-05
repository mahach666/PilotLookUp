using System;
using System.Collections.Generic;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Снимок значения из редактора строки критерия
    public sealed class CriterionValue
    {
        public string Text { get; set; }
        public string TextTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool BoolValue { get; set; }
        public IReadOnlyList<LookupItem> SelectedLookupItems { get; set; } = Array.Empty<LookupItem>();
    }
}
