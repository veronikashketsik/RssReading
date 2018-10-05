using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RssReading.Domain.Contracts.Models;
using RssReading.Infrastructure;

namespace RssReading.Domain.Data
{
    public interface IRssRepository
    {
        Task<IEnumerable<RssItem>> GetAllBySourceIdAsync(Guid id);
        Task<Source> GetSourceByUrlAsync(string sourceLink);
        Task CreateSourceAsync(Source modelSource);
        Task SaveChangesAsync();
        Task CreateRssItemBatchAsync(Guid sourceId, IEnumerable<RssItem> modelRssItems);
        Task<RssItem> GetLastRssItemBySourceAsync(Guid sourceId);
        Task<IEnumerable<Source>> GetAllSourcesAsync();
        Task<PaginatedResponse<RssItem>> QueryPageAsync(RssFilter filter, Paging paging, Sorting sorting);
    }
}