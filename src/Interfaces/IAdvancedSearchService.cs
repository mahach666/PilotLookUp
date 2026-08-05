using Ascon.Pilot.SDK;
using PilotLookUp.Model.AdvancedSearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface IAdvancedSearchService
    {
        // false, если клиент не предоставил ISearchService - расширенный поиск скрыт
        bool IsAvailable { get; }

        IReadOnlyList<CategoryDescriptor> GetCategories();

        // Пустой билдер: поиск по всей базе, сужение - критериями
        IQueryBuilder CreateBuilder();

        Task<AdvancedSearchResult> RunAsync(IQueryBuilder builder);
    }
}
