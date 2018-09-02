## streamscraper
streamscraper is a continuation of [stream-downloader](https://github.com/mihaly044/stream-downloader).
While stream-downloader only provides the Downloader class to directly download videos from m3u8 streams, **streamscraper** also provides  implementations of the ``IParser`` interface to grab m3u8 stream links from popular streaming sites.

## Building the project
Build this project with dotnetcore 2.2 as follows:
```bash
dotnet -c Release --runtime linux-x64
```
Find the built executeables under < Project root >/bin/Release/netcoreapp2.2/linux-x64/**publish**
#### Targeting other platforms
See [Microsoft's RID catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) for other runtime specifications.
If you are targeting Windows, then use win-x86 instead of linux-x64.

## Prerequisites
FFmpeg is required for the application to function and it needs to be downloaded/installed separately.
### FFmpeg on Windows
On Windows, download a x32 shared build from [ffmpeg's page](https://ffmpeg.zeranoe.com/builds/) and place ffmpeg.exe besides rtlmost-downloader-cli.exe
### FFmpeg on Unix-like systems
Install the appropriate package for your system. For example, on Debian, you'd say
```
sudo apt-get install -y ffmpeg
```

## Usage
Call streamscraper as follows:
```bash
./streamscraper <parser-type> <url-to-parse> <save-path>
```
### Available parsers
Parsers are special classes that enables streamscraper to grab download links from streaming sites. There are two parsers already implemented:

 - RtlMostParser
 - Tv2Parser

For example, if you want to download from **rtlmost**, you'd say:
```bash
./streamscraper rtlmost https://path-to-rtlmost-video.hu/example video.mp4
```

### Using custom parsers
To implement your own parser for a custom site, first create a class that inherits from `IParser` and implement ``ParseAsync`` as follows:

```csharp
public async Task<string> ParseAsync(string uri)
{
    // Dowbnload HTML source of the page
    var html = await Downloader.DownloadString(uri);
    var parsedString = "";
    // Do something with the html variable and put it into parsedString
    return parsedString;
}

```

Register your parser with `ParserFactory` before you start using it. For example if your parser class name is MyCustomParser, then call:

```csharp
ParserFactory.RegisterParser<MyCustomParser>("mycustomparser");
```
After you have registered your parser class, build the project and call ``streamscraper`` as follows:
```bash
./streamscraper mycustomparser <url-to-parse> <save-path>
```

## Copyright disclaimer
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
