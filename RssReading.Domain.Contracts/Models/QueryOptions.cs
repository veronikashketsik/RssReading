using System;
using System.Linq.Expressions;
using RssReading.Infrastructure;

namespace RssReading.Domain.Contracts.Models
{
    public class QueryOptions
    {
        public static QueryOptions CreateDefaultQueryOptions(string sorting)
        {
            if (!Sorting.TryParse(sorting, out var sort))
            {
                throw new ArgumentException("incorrect sorting string", nameof(sorting));
            }

            return new QueryOptions(new RssFilter(), new Paging(), sort);
        }

        public Sorting Sorting { get; set; }
        public Paging Paging { get; set; }
        public RssFilter Filter { get; set; }

        public QueryOptions(RssFilter filter, Paging paging, Sorting sorting)
        {
            Sorting = sorting ?? new Sorting();
            Paging = paging ?? new Paging();
            Filter = filter;
        }
    }

    public class RssFilter
    {
        public Guid? SourceId { get; set; }
    }
}