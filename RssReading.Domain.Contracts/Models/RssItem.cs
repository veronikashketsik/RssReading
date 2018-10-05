using System;

namespace RssReading.Domain.Contracts.Models
{
    public class RssItem
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Source Source { get; set; }
    }
}
