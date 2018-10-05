using System;
using System.ComponentModel.DataAnnotations;

namespace RssReading.Data.Models
{
    public class RssItemData
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Guid SourceId { get; set; }
        public SourceData Source { get; set; }
    }
}