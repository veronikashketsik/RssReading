using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RssReading.Domain.Contracts;
using RssReading.Domain.Contracts.Models;
using RssReading.Domain.Data;
using RssReading.Infrastructure;

namespace RssReading.Domain
{
    public class RssService : IRssService
    {
        private readonly IRssRepository _rssRepository;

        public RssService(IRssRepository rssItemRepository)
        {
            _rssRepository = rssItemRepository;
        }

        public async Task<IEnumerable<RssItem>> GetAllRssItemsBySourceIdAsync(Guid id)
        {
            return await _rssRepository.GetAllBySourceIdAsync(id);
        }

        public async Task<IEnumerable<Source>> GetAllSourcesAsync()
        {
            return await _rssRepository.GetAllSourcesAsync();
        }

        public async Task<PaginatedResponse<RssItem>> QueryPageAsync(QueryOptions queryOptions)
        {
            return await _rssRepository.QueryPageAsync(
                queryOptions.Filter,
                queryOptions.Paging,
                queryOptions.Sorting);
        }
    }
}