using System;
using System.Runtime.InteropServices;
using CommandLine;

namespace streamscraper
{
    internal class Program
    {

    [Verb("download",  HelpText = "Sets the program to download mode")]
        public class DownloadSubOptions
        {
            [Option('p', "parser", Required = true, HelpText = "Specifies which parser the program will use to obtain download links")]
            public string Parser { get; set; }

            [Option('u', "uri", Required = true, HelpText = "The website URL to download from")]
            public string Uri { get; set; }

            [Option('o', "output", Required = true, HelpText = "Location of the file to be saved")]
            public string Output { get; set; }

            [Option('g', "guiserve", Required = false, HelpText = "Optimize output for GUI programs")]
            public bool GuiServe { get; set; }
        }

        [Verb("listparsers", HelpText = "List all the available parsers")]
        public class ListParsersSubOptions {
            
        }

        private static bool _guiServe;
        private static Downloader _downloader;

        static void Main(string[] args)
        {
            ParserFactory.RegisterParser<RtlMostParser>("rtlmost");
            ParserFactory.RegisterParser<Tv2Parser>("tv2");
            ParserFactory.RegisterParser<MtvaParser>("mtva");

            if (args.Length > 0)
            {
                Parser.Default.ParseArguments<DownloadSubOptions, ListParsersSubOptions>(args)
                .WithParsed<DownloadSubOptions>(opts => {
                    _guiServe = opts.GuiServe;
                    var parser = ParserFactory.GetParser(opts.Parser.Trim());
                    if (parser == null)
                    {
                        if(!_guiServe)
                        {
                            ConsoleKit.Message(ConsoleKit.MessageType.ERROR, "Unknown parser selected\n");
                        }
                    }
                    else
                    {
                        DoAsyncDownload(opts.Uri.Trim(), opts.Output.Trim(), parser);
                        Wait();
                    }
                })
                .WithParsed<ListParsersSubOptions>(opts => {
                    Console.WriteLine(string.Join(", ", ParserFactory.GetAvailableParsers()));
                });
            }
            else
            {
                var parserType = "";
                IParser parser;
                var uri = "";
                var savepath = "";
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
                    Wait();
                }
                else
                {
                    throw new Exception("No parsers available");
                }
            }

        }
        
        private static void Wait()
        {
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
            if(!_guiServe)
            {
                ConsoleKit.Message(ConsoleKit.MessageType.DEBUG, "Downloading {0}\n", (object)parsedUri);
            }
            else
            {
                Console.WriteLine("DOWNLOADING_{0}", parsedUri);
            }

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
