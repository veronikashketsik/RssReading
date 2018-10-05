using System.Collections.Generic;

namespace RssReading.ConsoleApp.Config
{
    public class RssSource
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class AppSettings
    {
        public string DbConnection { get; set; }
        public IEnumerable<RssSource> RssSources { get; set; }
    }
}