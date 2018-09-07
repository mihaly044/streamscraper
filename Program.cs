using System;
using System.Collections;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            // Register parsers
            ParserFactory.RegisterParser<RtlMostParser>("rtlmost");
            ParserFactory.RegisterParser<Tv2Parser>("tv2");
            ParserFactory.RegisterParser<MtvaParser>("mtva");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Notice: ");
                Console.ResetColor();
                Console.Write("You are running Windows. To paste text, right click on the title bar to bring up" +
                              "the context menu, and press paste.\n");
            }

            string parserType;
            IParser parser;
            var uri = "";
            var savepath = "";


            if (args.Length > 0)
            {
                parserType = args[(int)Args.ParserType];
                parser = ParserFactory.GetParser(parserType);
                uri = args[(int)Args.Uri];
                savepath = args[(int)Args.SavePath];

                if(parser == null)
                {
                    throw new Exception("Unknown parser selected");
                }

                Console.WriteLine("Starting with args {0}", string.Join(", ", args));
                DoAsyncDownload(uri, savepath, parser);
            }
            else
            {
                var parsers = ParserFactory.GetAvailableParsers();
                if(parsers.Length > 0)
                {
                    ClrOut("@ ");
                    Console.Write("Parser [{0}] : ", string.Join(", ", parsers));
                    parserType = Console.ReadLine().ToLower();

                    parser = ParserFactory.GetParser(parserType);
                    if(parser == null)
                    {
                        throw new Exception("Unknown parser selected");
                    }

                    ClrOut("@ ");
                    Console.Write("URL : ");
                    uri = Console.ReadLine();
                    ClrOut("@ ");
                    Console.Write("Save to (*.mp4) : ");
                    savepath = Console.ReadLine();

                    DoAsyncDownload(uri, savepath, parser);
                }
                else
                {
                    throw new Exception("No parsers available");
                }
            }

            Console.ReadKey();
        }

        private static void ClrOut(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(msg);
            Console.ResetColor();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: streamscraper <parsertype> <url> <savepath>");
        }
        
        private static async void DoAsyncDownload(string uri, string savepath, IParser parser)
        {
            var parsedUri = await parser.ParseAsync(uri);
            Console.WriteLine("Downloading {0}\n", (object)parsedUri);

            var downloader = new Downloader();
            downloader.OnDurationInfo += duration => Console.WriteLine("Total duration={0}", (object)duration);
            downloader.OnProgress += (progress, time) =>
            {
                Console.Write("Time={0} Complete={1}%\t\r", time, progress);
            };
            downloader.OnDownloadComplete += () => ClrOut("\r\nDownload complete!");
            downloader.DownloadStream(parsedUri, savepath);
        }
    }
}
