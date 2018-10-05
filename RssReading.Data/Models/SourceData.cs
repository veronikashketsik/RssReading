using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RssReading.Data.Models
{
    public class SourceData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public ICollection<RssItemData> RssItems { get; set; }
    }
}