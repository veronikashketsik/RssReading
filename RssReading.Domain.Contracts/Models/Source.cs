using System;
using System.Collections.Generic;

namespace RssReading.Domain.Contracts.Models
{
    public class Source
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}