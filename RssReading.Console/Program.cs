using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssReading.ConsoleApp;
using RssReading.ConsoleApp.Config;
using RssReading.Data.Repositories;
using RssReading.Domain.Data;
using Serilog;
using AutoMapper;
using RssReading.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RssReading.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = GetSettings();

            Mapper.Initialize(config => config.AddProfiles(typeof(RssRepository).Assembly));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Information()
                .MinimumLevel
                .Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .WriteTo
                .Console()
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder=> builder.AddSerilog())
                .AddSingleton<IRssRepository, RssRepository>()
                .AddSingleton<RssDatabaseFiller>()
                .AddDbContext<RssContext>(options => options.UseSqlServer(settings.DbConnection))
                .BuildServiceProvider();

            FillDatabase(serviceProvider.GetService<RssDatabaseFiller>(), settings.RssSources);

            Log.Logger.Information("Rss data synchronizing finished.");
            System.Console.ReadLine();
        }

        private static AppSettings GetSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            var section = config.GetSection("AppSettings");
            return section.Get<AppSettings>();
        }

        private static void FillDatabase(RssDatabaseFiller dbFiller, IEnumerable<RssSource> sources)
        {
            foreach (var source in sources)
            {
                dbFiller.FillDbAsync(source.Link, source.Name).GetAwaiter().GetResult();
            }
        }
    }
}
