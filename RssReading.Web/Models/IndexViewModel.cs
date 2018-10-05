using Microsoft.AspNetCore.Mvc.Rendering;
using RssReading.Domain.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssReading.Web.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<Source> sources)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Все" });
            items.AddRange(sources.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }));

            this.Sources = items;
        }

        public IList<SelectListItem> Sources { get; }

        public Guid? SourceId { get; set; }

        public string Sort { get; set; }  
        
        public RssFilterDto Filter { get; set; }
    }

    public class RssFilterDto
    {
        public Guid? SourceId { get; set; }

        public string Sort { get; set; }
    }
}
