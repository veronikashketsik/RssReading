using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RssReading.Infrastructure
{
    public static class PagingExtensions
    {
        public static async Task<PaginatedResponse<T>> PageAsync<T>(this IQueryable<T> query, Paging paging = null)
        {
            if (paging == null)
            {
                paging = Paging.Default;
            }

            if (!(query.Expression.Type == typeof(IOrderedQueryable<T>)))
            {
                query = query.OrderBy(x => true);
            }

            var paginatedQuery = query.Skip((paging.Page - 1) * paging.PageSize).Take(paging.PageSize) as IOrderedQueryable<T>;

            var results = await paginatedQuery.ToListAsync();

            var response = new PaginatedResponse<T>()
            {
                Data = results,
                Paging = new PaginatedResponsePagingInfo()
                {
                    Limit = paging.PageSize,
                    Page = paging.Page,
                    TotalResults = query.Count(),
                    Returned = results.Count()
                }
            };

            return response;
        }

    }
    public class Paging
    {
        public static readonly Paging Default = new Paging { Page = DefaultPage, PageSize = DefaultPageSize };

        private const int DefaultPageSize = 10;
        private const int DefaultPage = 1;

        public Paging()
        {
            PageSize = DefaultPageSize;
            Page = DefaultPage;
        }

        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public PaginatedResponsePagingInfo Paging { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class PaginatedResponsePagingInfo
    {
        public int TotalResults { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }

        public int Returned { get; set; }

        public int TotalPages
        {
            get
            {
                if (Limit == 0)
                    return 0;

                var pages = Convert.ToDecimal(TotalResults) / Convert.ToDecimal(Limit);
                return Convert.ToInt32(Math.Ceiling(pages));
            }
        }
    }
}