using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RssReading.Domain.Contracts.Models;
using RssReading.Infrastructure;

namespace RssReading.Domain.Contracts
{
    public interface IRssService
    {
        Task<IEnumerable<RssItem>> GetAllRssItemsBySourceIdAsync(Guid id);
        Task<IEnumerable<Source>> GetAllSourcesAsync();
        Task<PaginatedResponse<RssItem>> QueryPageAsync(QueryOptions queryOptions);
    }
}
