{% include_relative header.md %}
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