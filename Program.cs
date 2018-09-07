using System;
using System.Runtime.InteropServices;

namespace streamscraper
{
    class Program
    {
        private enum Args {
            ParserType = 0,
            Uri,
            SavePath
        }

        private static Downloader _downloader;

        static void Main(string[] args)
        {
            ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "streamscraper starting up...\n");
            // Register parsers
            ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "Registering parsers...\n");
            ParserFactory.RegisterParser<RtlMostParser>("rtlmost");
            ParserFactory.RegisterParser<Tv2Parser>("tv2");
            ParserFactory.RegisterParser<MtvaParser>("mtva");
            ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "Total count of parsers is {0}\n", ParserFactory.GetAvailableParsers().Length);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConsoleKit.Message(ConsoleKit.MessageType.WARNING, "You are running Windows. To paste text, right click on the title" +
                                                                   "bar to bring up the context menu, and press paste.\n\n");
            }

            string parserType;
            IParser parser;
            string uri;
            string savepath;


            if (args.Length > 0)
            {
                parserType = args[(int)Args.ParserType];
                parser = ParserFactory.GetParser(parserType);
                uri = args[(int)Args.Uri];
                savepath = args[(int)Args.SavePath];

                if(parser == null)
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.ERROR, "Unknown parser selected\n");
                    return;
                }

                ConsoleKit.Message(ConsoleKit.MessageType.INFO , "Starting with args {0}\n", string.Join(", ", args));
                DoAsyncDownload(uri, savepath, parser);
            }
            else
            {
                var parsers = ParserFactory.GetAvailableParsers();
                if(parsers.Length > 0)
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.INPUT, "Parser [{0}]: ", string.Join(", ", parsers));
                    parserType = Console.ReadLine().ToLower();

                    parser = ParserFactory.GetParser(parserType);
                    if(parser == null)
                    {
                        ConsoleKit.Message(ConsoleKit.MessageType.ERROR, "Unknown parser selected\n");
                        return;
                    }

                    ConsoleKit.Message(ConsoleKit.MessageType.INPUT, "URL: ");
                    uri = Console.ReadLine();
 
                    ConsoleKit.Message(ConsoleKit.MessageType.INPUT, "Save to (*.mp4): ");
                    savepath = Console.ReadLine();

                    DoAsyncDownload(uri, savepath, parser);
                }
                else
                {
                    throw new Exception("No parsers available");
                }
            }

            var running = true;
            while (running)
            {
                Console.ReadKey();
                if (_downloader != null)
                    running = _downloader.IsDownloading();
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
            ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "Downloading {0}\n", (object)parsedUri);

             _downloader = new Downloader();
            _downloader.OnDurationInfo += duration => ConsoleKit.Message(ConsoleKit.MessageType.INFO, "Total duration={0}\n", (object)duration);

            _downloader.OnProgress += (progress, time) =>
            {
                Console.Write("Time={0} Complete={1}%\t\r", time, progress);
            };
            _downloader.OnDownloadComplete += () => ConsoleKit.Message(ConsoleKit.MessageType.INFO, 
                "Download complete!\n");
            _downloader.DownloadStream(parsedUri, savepath);
        }
    }
}
