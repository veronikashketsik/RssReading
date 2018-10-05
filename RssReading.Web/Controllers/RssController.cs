using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RssReading.Domain.Contracts;
using RssReading.Domain.Contracts.Models;
using RssReading.Infrastructure;
using RssReading.Web.Models;

namespace RssReading.Web.Controllers
{
    public class RssController : Controller
    {
        private const string defaultSorting = "PublishDate:desc";

        private readonly IRssService _service;

        public RssController(IRssService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sources = await _service.GetAllSourcesAsync();
            var defaultOptions = QueryOptions.CreateDefaultQueryOptions(defaultSorting);
            var paginatedData = await _service.QueryPageAsync(defaultOptions);
            var viewModel = new FilterRssItemsViewModel
            {
                RssItems = paginatedData.Data,
                PageNumber = paginatedData.Paging.Page,
                TotalPageCount = paginatedData.Paging.TotalPages
            };

            ViewBag.FilterRss = viewModel;

            return View(new IndexViewModel(sources));
        }

        [HttpPost]
        public async Task<IActionResult> FilterRssItems([FromForm] RssFilterDto filter, [FromQuery] Paging paging = null)
        {
            var domainFilter = Mapper.Map<RssFilter>(filter);
            if (!Sorting.TryParse(filter.Sort, out var sort))
            {
                this.BadRequest($"Incorrect sorting specified");
            }

            var options = new QueryOptions(domainFilter, paging ?? Paging.Default, sort ?? new Sorting { SortFields = new List<SortField>(){new SortField{FieldName = "PublishDate", Direction = SortDirection.Descending}}});
            var paginatedData = await _service.QueryPageAsync(options);

            var viewModel = new FilterRssItemsViewModel
               {
                   RssItems = paginatedData.Data,
                   PageNumber = paginatedData.Paging.Page,
                   TotalPageCount = paginatedData.Paging.TotalPages
               };

            return PartialView(viewModel);
        }
    }
}