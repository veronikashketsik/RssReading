using Microsoft.Extensions.Logging;
using RssReading.Domain.Contracts.Models;
using RssReading.Domain.Data;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RssReading.ConsoleApp
{
    public class RssDatabaseFiller : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IRssRepository _repository;

        public RssDatabaseFiller(ILoggerFactory loggerFactory, IRssRepository repository)
        {
            _logger = loggerFactory.CreateLogger<RssDatabaseFiller>();
            _repository = repository;
            _httpClient = new HttpClient();
        }       

        public async Task FillDbAsync(string sourceUrl, string defaultSourceName = "")
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
            {
                throw new ArgumentException($"{nameof(sourceUrl)} can't be null or empty");
            }

            var response = await _httpClient.GetAsync(sourceUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while getting response from {sourceUrl}. HttpResponseCode: {statusCode}",
                    sourceUrl,
                    response.StatusCode);

                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            XDocument xmlDoc = XDocument.Parse(content.Trim());
            var model = RetrieveFromXml(xmlDoc, defaultSourceName);
            await FillDatabaseAsync(model);
        }
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task FillDatabaseAsync(RssModel model)
        {
            var source = await _repository.GetSourceByUrlAsync(model.Source.Link);
            if (source == null)
            {
                model.Source.Id = Guid.NewGuid();
                await _repository.CreateSourceAsync(model.Source);
                await _repository.CreateRssItemBatchAsync(model.Source.Id, model.RssItems);
                await _repository.SaveChangesAsync();
                _logger.LogInformation("Источник: {0} ({1}); Прочитано: {2} новостей; Сохранено: {3} новостей",
                    model.Source.Name,
                    model.Source.Link,
                    model.RssItems.Count(),
                    model.RssItems.Count());

                return;
            }
            //get last rssItem for source
            var lastRssItem = await _repository.GetLastRssItemBySourceAsync(source.Id);
            if (lastRssItem == null)
            {
                await _repository.CreateRssItemBatchAsync(source.Id, model.RssItems);
                await _repository.SaveChangesAsync();
                _logger.LogInformation("Источник: {0} ({1}); Прочитано: {2} новостей; Сохранено: {3} новостей",
                    model.Source.Name,
                    model.Source.Link,
                    model.RssItems.Count(),
                    model.RssItems.Count());

                return;
            }

            var newRssItems = model.RssItems.Where(x => x.PublishDate > lastRssItem.PublishDate);
            await _repository.CreateRssItemBatchAsync(source.Id, newRssItems);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Источник: {0} ({1}); Прочитано: {2} новостей; Сохранено: {3} новостей",
                model.Source.Name,
                model.Source.Link,                
                model.RssItems.Count(),
                newRssItems.Count());
        }

        private RssModel RetrieveFromXml(XDocument xmlDoc, string defaultSourceName = "")
        {
            var source = new Source
            {
                Name = !string.IsNullOrWhiteSpace(xmlDoc.Element("channel")?.Element("title")?.Value)
                ? xmlDoc.Element("rss")?.Element("channel")?.Element("title")?.Value
                : defaultSourceName,
                Link = xmlDoc.Element("rss")?.Element("channel")?.Element("link")?.Value
            };

            var rssItems = xmlDoc.Descendants("item")
                .Where(x => DateTime.TryParse(x.Element("pubDate")?.Value, out _))
                .Select(x =>
                    new RssItem
                    {
                        Title = x.Element("title")?.Value,
                        Link = x.Element("link")?.Value,
                        Description = x.Element("description")?.Value,
                        PublishDate = DateTime.Parse(x.Element("pubDate")?.Value),
                    }
                );

            return new RssModel
            {
                Source = source,
                RssItems = rssItems
            };
        }
    }
}
