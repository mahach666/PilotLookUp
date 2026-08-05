namespace PilotLookUp.Model.AdvancedSearch
{
    // Элемент выпадашки с поиском: фильтр ищет по этому тексту, а не только
    // по отображаемой подписи (у атрибута это все его заголовки из разных типов)
    public interface ISearchableText
    {
        string SearchText { get; }
    }
}
