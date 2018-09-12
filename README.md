
## streamscraper [![Build status](https://ci.appveyor.com/api/projects/status/ti9ndirsgqc0ks0u?svg=true)](https://ci.appveyor.com/project/mihaly044/streamscraper) ![License](https://img.shields.io/github/license/mihaly044/streamscraper.svg) [![GitHub issues](https://img.shields.io/github/issues/mihaly044/streamscraper.svg)](https://github.com/mihaly044/streamscraper/issues)

streamscraper is a cross-platform CLI tool for scraping m3u8 links and downloading movies from popular streaming sites.

## Looking for a graphical interface?
There is a GUI bundled with the latest version of streamscraper here: [streamscraper-gui](https://github.com/mihaly044/streamscraper-gui).

## Usage
### Launching without parameters

If you start streamscraper without parameters, it will ask you to input additonal data. Messages marked with `[INPUT]` means the program is waiting for an input.

### Launching with parameters

To view a list of available commands, use the --help switch

```bash
./streamscraper --help
```

This outputs:

```
  download       Sets the program to download mode
  listparsers    List all the available parsers
  help           Display more information on a specific command.
  version        Display version information.
```

### Downloading movies
#### Parameters
```
  -p, --parser      Required. Specifies which parser the program will use to 
                    obtain download links
  -u, --uri         Required. The website URL to download from
  -o, --output      Required. Location of the file to be saved
  -g, --guiserve    Optimize output for GUI programs
```

To download a movie from RTLMost!, you'd say:

```bash
./streamscraper download --parser rtlmost --uri https://path-to-rtlmost-video.hu/example --output video.mp4
```
Or the shorthand version:

```bash
./streamscraper download -p rtlmost -u https://path-to-rtlmost-video.hu/example -o video.mp4
```

