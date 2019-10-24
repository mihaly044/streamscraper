using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace streamscraper.Parsers
{
    public partial class MtvaParser: IParser
    {
        public async Task<string> ParseAsync(string uri)
        {
            var html = await Downloader.DownloadString(uri);

            // Prepare parameters for request
            var prefix = "{\"token\":\"";
            var postfix = "}";
            var splitted = html.Split(new[] { prefix }, StringSplitOptions.None);
            var end = splitted[1].IndexOf("})");
            var mtvaJson = MtvaJson.FromJson(prefix + splitted[1].Substring(0, end) + postfix);
            
            // Prepare stream link
            var scrapeUrl = $"https://player.mediaklikk.hu/playernew/player.php?video={mtvaJson.Token}" +
                             "&noflash=yes&vastpreroll={mtvaJson.adVastPreroll}&osfamily=Ubuntu&osversion=null" +
                            $"browsername=Firefox&browserversion=61.0&title={mtvaJson.Title}" +
                            $"title={mtvaJson.Title}&series={mtvaJson.Series}&contentid={mtvaJson.ContentId}&embedded=0";
            var scrapeHtml = await Downloader.DownloadString(scrapeUrl);

            // Construct an object from json
            var prefix0 = "{";
            var prefix1 = "\"playlist\":";
            var postfix1 = "]}";

            var scrapeSplitted = scrapeHtml.Split(new[] { prefix1 }, StringSplitOptions.None );
            var scrapeEnd = scrapeSplitted[1].IndexOf(']');
            var mtvaPlaylist = MtvaPlaylist.FromJson(prefix0 + prefix1 + scrapeSplitted[1].Substring(0, scrapeEnd) + postfix1);
            
            return "http:" + mtvaPlaylist.playlist[0].File;
        }
    }

    public partial class MtvaJson
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("autostart")]
        public bool Autostart { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; }

        [JsonProperty("bgImage")]
        public string BgImage { get; set; }

        [JsonProperty("adVastPreroll")]
        public string AdVastPreroll { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("series")]
        public string Series { get; set; }

        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        [JsonProperty("embedded")]
        public bool Embedded { get; set; }
    }

    public partial class MtvaJson
    {
        public static MtvaJson FromJson(string json) => JsonConvert.DeserializeObject<MtvaJson>(json, Converter.Settings);
    }

    public class Playlist
    {
        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class MtvaPlaylist
    {
        public List<Playlist> playlist { get; set; }
    }

    public partial class MtvaPlaylist
    {
        public static MtvaPlaylist FromJson(string json) => JsonConvert.DeserializeObject<MtvaPlaylist>(json, Converter.Settings);
    }
}