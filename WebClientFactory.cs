using System.Net;
using System.Text;

namespace streamscraper
{
    internal static class WebClientFactory
    {
        public static WebClient GetClient()
        {
            return new WebClient
            {
                Proxy = null,
                Encoding = Encoding.UTF8,
                Headers =
                {
                    ["Cache-Control"] = "max-age=0",
                    ["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                    ["User-agent"] = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0",
                    ["Accept-Language"] = "hu-HU,hu;q=0.8"
                }
            };
        }
    }
}
