using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    public class AdvancedSearchService : IAdvancedSearchService
    {
        private const int SearchTimeoutMs = 15000;

        // ISearchService принадлежит клиенту (IDisposable) - не диспозим
        private readonly ISearchService _searchService;
        private readonly FieldCatalog _fieldCatalog;
        private IReadOnlyList<CategoryDescriptor> _categoriesCache;

        public AdvancedSearchService(ISearchService searchService, IObjectsRepository objectsRepository)
        {
            _searchService = searchService;
            _fieldCatalog = new FieldCatalog(objectsRepository);
        }

        public bool IsAvailable => _searchService != null;

        public IReadOnlyList<CategoryDescriptor> GetCategories()
        {
            // Кэш на время жизни плагина: состав типов/атрибутов меняется редко
            return _categoriesCache ??= _fieldCatalog.GetCategories();
        }

        public IQueryBuilder CreateBuilder()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("ISearchService недоступен в этом клиенте.");

            return _searchService.GetEmptyQueryBuilder();
        }

        public async Task<AdvancedSearchResult> RunAsync(IQueryBuilder builder, int maxResults)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("ISearchService недоступен в этом клиенте.");

            var observer = new SearchResultObserver();
            return await observer.WaitAsync(_searchService.Search(builder), SearchTimeoutMs, maxResults);
        }
    }
}
