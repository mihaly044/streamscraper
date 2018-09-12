
## streamscraper [![Build status](https://ci.appveyor.com/api/projects/status/ti9ndirsgqc0ks0u?svg=true)](https://ci.appveyor.com/project/mihaly044/streamscraper) ![License](https://img.shields.io/github/license/mihaly044/streamscraper.svg) [![GitHub issues](https://img.shields.io/github/issues/mihaly044/streamscraper.svg)](https://github.com/mihaly044/streamscraper/issues)

streamscraper is a handy, cross platform CLI tool for downloading media from m3u8 streams.

It provides the Downloader class (which is essentially an ffmpeg wrapper class) to directly download from m3u8 streams.

In case you need to do additional parsing to fetch the download URL from some site, there is the ``IParser`` interface that you may use to write your custom parsers.

streamscraper already contains implementations of the ``IParser`` interface to grab m3u8 stream links from popular streaming sites.

## Looking for a GUI?

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

## Building the project


## Prebuilt binaries

If you do not wish to build the project yourself, you may download the prebuilt binary [v1.1.0-rc](https://github.com/mihaly044/streamscraper/releases/tag/v1.1.0-rc).

### Linux

On Linux systems, run quick-build:

```bash
chmod +x ./quick-build
./quick-build -v=version
```

*Specifying a version is optional.* The build script only uses the version parameter for naming packed tar.gz releases.

Find the built and packed executeables for each specified target platform under ``<Project root>/bin/Release/netcoreapp2.2/packed/``

  

### Windows
Build from Visual Studio 2017, or use the dotnet CLI tool as follows:

```
dotnet publish -c Release --runtime win-x86
```

Find the built and packed executeables for each specified target platform under ``<Project root>/bin/Release/netcoreapp2.2/win-x86/publish``

#### Targeting other platforms

See [Microsoft's RID catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) for other runtime specifications.


## Prerequisites

FFmpeg is required for the application to function and it needs to be downloaded/installed separately if running on Linux.

### FFmpeg on Windows

On Windows, download a x32 shared build from [ffmpeg's page](https://ffmpeg.zeranoe.com/builds/) and place ffmpeg.exe besides rtlmost-downloader-cli.exe

**Note**: The current prebuilt versions of streamscraper already contains ffmpeg.

### FFmpeg on Unix-like systems

Install the appropriate package for your system. For example, on Debian, you'd say

```bash
sudo apt-get install -y ffmpeg
```

### Available parsers

Parsers are special classes that enables streamscraper to grab download links from streaming sites. There are three parsers already implemented:

- RtlMostParser
- Tv2Parser
- MtvaParser

### Using custom parsers

To implement your own parser for a custom site, first create a class that inherits from `IParser` and implement ``ParseAsync`` as follows:

```csharp
public  async  Task<string> ParseAsync(string  uri)
{
   // Dowbnload HTML source of the page
   var  html  =  await  Downloader.DownloadString(uri);
   var  parsedString  =  "";
   // Do something with the html variable and put it into parsedString
   return  parsedString;
}
```
Register your parser with `ParserFactory` before you start using it. For example if your parser class name is MyCustomParser, then call:



```csharp
ParserFactory.RegisterParser<MyCustomParser>("mycustomparser");
```

After you have registered your parser class, build the project and call ``streamscraper`` as follows:

```bash
./streamscraper mycustomparser <url-to-parse>  <save-path>
```

## Disclaimer

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
