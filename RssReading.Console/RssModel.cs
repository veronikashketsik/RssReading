using System.Collections.Generic;
using RssReading.Domain.Contracts.Models;

namespace RssReading.ConsoleApp
{
    public class RssModel
    {
        public Source Source { get; set; }
        public IEnumerable<RssItem> RssItems { get; set; }
    }
}