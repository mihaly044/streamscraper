{% include_relative header.md %}

## What is streamscraper?
streamscraper is a cross platform CLI application for grabbing m3u8 stream links from popular streaming websites. It uses [ffmpeg](https://github.com/FFmpeg/FFmpeg) to download movies from the acquired links.

## GUI support
streamscraper also as a convenient graphical interface. Download pre-built binaries from the official [streamscraper-gui](https://github.com/mihaly044/streamscraper-gui/releases/tag/v.0.2-rev902b52_b7f7988a-beta) repo.

## Installation
- Download the latest release from here: [https://github.com/mihaly044/streamscraper/releases/latest](https://github.com/mihaly044/streamscraper/releases/latest)


## Usage
Start streamscraper with the --help switch to view a list of available commands:
```bash
streamscraper --help
````
This outputs the following message:
```
download       Sets the program to download mode
listparsers    List all the available parsers
help           Display more information on a specific command.
version        Display version information.
```
### Downloading content
#### Parameters
```
-p, --parser      Required. Specifies which parser the program will use to 
                  obtain download links
-u, --uri         Required. The website URL to download from
-o, --output      Required. Location of the file to be saved
-g, --guiserve    Optimize output for GUI programs
```

To download a movie from rtlmost, you would say:

```
./streamscraper download -p rtlmost -u https://path-to-rtlmost-video.hu/example -o video.mp4
```

## Disclaimer
Downloading copyrighted content usually requires prior written consent of the author(s). You are only permitted to use this software to download material if you own the aftermentioned written constent and/or the laws of the country you live in permints to do so.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.