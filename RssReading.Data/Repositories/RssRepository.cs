using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RssReading.Data.MapperProfiles;
using RssReading.Data.Models;
using RssReading.Domain.Contracts.Models;
using RssReading.Domain.Data;
using RssReading.Infrastructure;

namespace RssReading.Data.Repositories
{
    public class RssRepository: IRssRepository
    {
        private readonly RssContext _context;

        public RssRepository(RssContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RssItem>> GetAllBySourceIdAsync(Guid id)
        {
           var itemDatas = await _context.RssItemDatas
               .Include(x => x.Source)
               .Where(x => x.SourceId == id)
               .ToListAsync();

            return Mapper.Map<IEnumerable<RssItem>>(itemDatas);
        }

        public async Task<Source> GetSourceByUrlAsync(string sourceLink)
        {
            var sourceData = await _context.SourceDatas.SingleOrDefaultAsync(x => x.Link == sourceLink);
            return Mapper.Map<Source>(sourceData);
        }

        public Task CreateSourceAsync(Source modelSource)
        {
            return _context.SourceDatas.AddAsync(Mapper.Map<SourceData>(modelSource));
        }

        public Task SaveChangesAsync()
        {
           return _context.SaveChangesAsync();
        }

        public Task CreateRssItemBatchAsync(Guid sourceId, IEnumerable<RssItem> modelRssItems)
        {
            var itemDatas = Mapper.Map<IEnumerable<RssItemData>>(modelRssItems);
            foreach (var item in itemDatas)
            {
                item.SourceId = sourceId;
            }

            return _context.RssItemDatas.AddRangeAsync(itemDatas);
        }

        public async Task<RssItem> GetLastRssItemBySourceAsync(Guid sourceId)
        {
            var rssItemData = await _context.RssItemDatas.OrderByDescending(x => x.PublishDate).FirstOrDefaultAsync();
            return Mapper.Map<RssItem>(rssItemData);
        }

        public async Task<IEnumerable<Source>> GetAllSourcesAsync()
        {
            var sourceDatas = await _context.SourceDatas.ToListAsync();

            return Mapper.Map<IEnumerable<Source>>(sourceDatas);
        }

        public async Task<PaginatedResponse<RssItem>> QueryPageAsync(RssFilter filter,
            Paging paging, Sorting sorting)
        {
            var dbSet = _context.RssItemDatas.Include(x => x.Source);
            var query = filter == null 
                ? dbSet 
                : dbSet.Where(GetExpression(filter));
            var datas = await SortAndPageAsync(query, paging, sorting);

            return Mapper.Map<PaginatedResponse<RssItem>>(datas);
        }

        private async Task<PaginatedResponse<RssItemData>> SortAndPageAsync(IQueryable<RssItemData> queryable,
            Paging paging, Sorting sorting)
        {
            return await queryable
                .Sort(sorting)
                .PageAsync(paging);
        }

        public Expression<Func<RssItemData, bool>> GetExpression(RssFilter filter)
        {
            if (filter.SourceId.HasValue)
            {
                return x => x.SourceId == filter.SourceId;
            }

            return x => true;
        }
    }

}