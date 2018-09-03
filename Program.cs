using System;
using System.Collections;
using System.Collections.Generic;
using stream_downloader;

namespace streamscraper
{
    class Program
    {
        private enum Args {
            ParserType = 0,
            Uri,
            SavePath
        }

        static void Main(string[] args)
        {
            // Register parsers
            ParserFactory.RegisterParser<RtlMostParser>("rtlmost");
            ParserFactory.RegisterParser<Tv2Parser>("tv2");
            ParserFactory.RegisterParser<MtvaParser>("mtva");
            
            if (args.Length > 0)
            {
                var parserType = args[(int)Args.ParserType];
                var parser = ParserFactory.GetParser(parserType);

                if(parser == null)
                {
                    throw new Exception("Unknown parser selected");
                }

                var uri = args[(int)Args.Uri];
                var savepath = args[(int)Args.SavePath];
                Console.WriteLine("Starting with args {0}", string.Join(", ", args));
                DoAsyncDownload(uri, savepath, parser);
            }
            else
            {
                PrintUsage();
            }

            Console.ReadKey();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: streamscraper <parsertype> <url> <savepath>");
        }
        
        private static async void DoAsyncDownload(string uri, string savepath, IParser parser)
        {
            var parsedUri = await parser.ParseAsync(uri);
            Console.WriteLine("Downloading {0}\n", (object)parsedUri);

            var downloader = new stream_downloader.Downloader();
            downloader.OnDurationInfo += duration => Console.WriteLine("Total duration={0}", (object)duration);
            downloader.OnProgress += (progress, time) =>
            {
                Console.Write("Time={0} Complete={1}%\t\r", time, progress);
            };
            downloader.OnDownloadComplete += () => Console.WriteLine("\r\nDownload complete!");
            downloader.DownloadStream(parsedUri, savepath);
        }
    }
}
