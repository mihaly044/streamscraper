{% include_relative header.md %}

# Wiki
- [Prerequsities](#Prerequsities)
- [Building the project](#Building_the_project)
- [Implementing custom parsers](#Implementing_custom_parsers)

## Prerequisites

FFmpeg is required for the application to function and it needs to be downloaded/installed separately if running on Linux.

### FFmpeg on Windows

On Windows, download a x32 shared build from [ffmpeg's page](https://ffmpeg.zeranoe.com/builds/) and place ffmpeg.exe besides streamscraper.exe

**Note**: The current prebuilt versions of streamscraper already contains ffmpeg.

### FFmpeg on Unix-like systems

Install the appropriate package for your system. For example, on Debian, you'd say

```bash
sudo apt-get install -y ffmpeg
```


## Building the project
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

### Targeting other platforms

See [Microsoft's RID catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) for other runtime specifications.

# Implementing custom parsers
### Available parsers

Parsers are special classes that enables streamscraper to grab download links from streaming sites. There are three parsers have been already implemented:

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
./streamscraper download -p mycustomparser -u <url-to-parse> -o <save-path>
```
