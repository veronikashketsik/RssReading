using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RssReading.Domain.Contracts.Models;

namespace RssReading.Web.Models
{
    public class FilterRssItemsViewModel
    {
        public IEnumerable<RssItem> RssItems { get; set; }

        public int PageNumber { get; set; }

        public int TotalPageCount { get; set; }
    }
}
