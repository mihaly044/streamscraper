using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace streamscraper.Parsers
{
    public class Tv2Parser: IParser
    {
        public async Task<string> ParseAsync(string uri)
        {
            var html = await Downloader.DownloadString(uri);
            var splitted = html.Split(new[] {"var jsonUrl = \"//"}, StringSplitOptions.None);
            var jsonUrl = "http://" + splitted[1].Substring(0, splitted[1].IndexOf(';')-1);

            var jsonResponse = await Downloader.DownloadString(jsonUrl);
            var tv2Response = Tv2Response.FromJson(jsonResponse);

            return "http:" + tv2Response.Bitrates.Hls;
        }
    }

    public partial class Tv2Response
    {
        [JsonProperty("bitrates")]
        public Bitrates Bitrates { get; set; }

        [JsonProperty("backupBitRates")]
        public BackupBitrates BackupBitrates { get; set; }

        [JsonProperty("mp4Labels")]
        public List<string> Mp4Labels { get; set; }

        [JsonProperty("bitrateType")]
        public string BitrateType { get; set; }

        [JsonProperty("sourceType")]
        public string SourceType { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("servers")]
        public List<string> Servers { get; set; }
    }

    public class Bitrates
    {
        [JsonProperty("hls")]
        public string Hls { get; set; }
    }

    public class BackupBitrates
    {
        [JsonProperty("hls")]
        public string Hls { get; set; }
    }

    public partial class Tv2Response
    {
        public static Tv2Response FromJson(string json) => JsonConvert.DeserializeObject<Tv2Response>(json, Converter.Settings);
    }

    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}