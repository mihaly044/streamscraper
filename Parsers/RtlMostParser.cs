using System;
using System.Linq;
using System.Threading.Tasks;
using stream_downloader;
namespace streamscraper
{
    public class RtlMostParser: IParser
    {
        public async Task<string> ParseAsync(string uri)
        {
            var html = await Downloader.DownloadString(uri);
            var splitted = html.Split(new[] { "\"full_physical_path\":\"" }, StringSplitOptions.None);

            var foundMedia = (from chunk in splitted
            where chunk.StartsWith("http")
            select chunk.Split('"')[0].Replace("\\u002F", "/")).ToList();

            foreach (var url in foundMedia)
            {
                if (url.Contains("_unpnp.ism/"))
                return url;
            }

            return "";
        }
    }
}