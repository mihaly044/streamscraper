using System;
using System.Runtime.InteropServices;
using CommandLine;

namespace streamscraper
{
    internal class Program
    {
        public class Options
        {
            [Option('g', "guiserve", Required = false, HelpText = "Optimize output for GUI programs")]
            public bool GuiServe { get; set; }

            [Option('p', "parser", Required = true, HelpText = "Specifies which parser the program will use to obtain download links")]
            public string Parser { get; set; }

            [Option('u', "uri", Required = true, HelpText = "The website URL to download from")]
            public string Uri { get; set; }

            [Option('o', "output", Required = true, HelpText = "Location of the file to be saved")]
            public string Output { get; set; }
        }

        private static bool _guiServe;
        private static Downloader _downloader;

        static void Main(string[] args)
        {
            ParserFactory.RegisterParser<RtlMostParser>("rtlmost");
            ParserFactory.RegisterParser<Tv2Parser>("tv2");
            ParserFactory.RegisterParser<MtvaParser>("mtva");

            var parserType = "";
            IParser parser;
            var uri = "";
            var savepath = "";

            if (args.Length > 0)
            {
                var parsed = true;
                Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    parserType = o.Parser.Trim();
                    uri = o.Uri.Trim();
                    savepath = o.Output.Trim();
                    _guiServe = o.GuiServe;

                }).WithNotParsed(o =>
                {
                    parsed = false;
                });

                if (!parsed)
                    return;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.WARNING, "You appear to be running Windows. To paste text, right click on the title" +
                                                                       "bar to bring up the context menu, and press paste.\n\n");
                }

                parser = ParserFactory.GetParser(parserType);

                if (parser == null)
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
                if (parsers.Length > 0)
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.INPUT, "Parser [{0}]: ", string.Join(", ", parsers));
                    parserType = Console.ReadLine().ToLower();

                    parser = ParserFactory.GetParser(parserType);
                    if (parser == null)
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
                Console.Read();
                if (_downloader != null)
                    running = _downloader.IsDownloading();
            }
        }
        
        private static async void DoAsyncDownload(string uri, string savepath, IParser parser)
        {
            var parsedUri = await parser.ParseAsync(uri);
            ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "Downloading {0}\n", (object)parsedUri);

             _downloader = new Downloader();
            _downloader.OnDurationInfo += duration =>
            {
                if (!_guiServe)
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.INFO, "Total duration={0}\n", (object) duration);
                }
                else
                {
                    Console.WriteLine("DURATION_{0}", duration);
                }
            };


            var spinner = new[]{ '|', '/', '-', '\\' };
            var spinnerI = 0;

            _downloader.OnProgress += (progress, time) =>
            {
                if (!_guiServe)
                {
                    if (spinnerI > spinner.Length - 1)
                        spinnerI = 0;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"\t{spinner[spinnerI++]} ");
                    Console.ResetColor();

                    Console.Write("Time={0} Complete={1}%\t\r", time, progress);
                    if (progress == 100)
                        Console.Write("\n");
                }
                else
                {
                    Console.WriteLine("PROGRESS_{0}", progress);
                }
            };

            _downloader.OnDownloadComplete += () =>
            {
                if (!_guiServe)
                {
                    ConsoleKit.Message(ConsoleKit.MessageType.INFO,
                        "Download complete!\n");
                }
                else
                {
                    Console.WriteLine("COMPLETE_");
                }
            };
            _downloader.DownloadStream(parsedUri, savepath);
        }
    }
}
