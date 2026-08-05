namespace PilotLookUp.Model.AdvancedSearch
{
    // Элемент справочника для выбора значения (человек, тип, оргединица, состояние)
    public sealed class LookupItem
    {
        public LookupItem(object key, string title)
        {
            Key = key;
            Title = title;
        }

        public object Key { get; }
        public string Title { get; }

        public override string ToString() => Title;
    }
}
