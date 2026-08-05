namespace PilotLookUp.Model.AdvancedSearch
{
    // Пункт выпадающего списка с человекочитаемым названием
    public sealed class ChoiceItem
    {
        public ChoiceItem(object value, string title)
        {
            Value = value;
            Title = title;
        }

        public object Value { get; }
        public string Title { get; }

        public override string ToString() => Title;
    }
}
