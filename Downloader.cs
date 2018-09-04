using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace streamscraper
{
    public class Downloader
    {
        private TimeSpan _duration;
        private int _progress;
        private Process _ffmpeg;
        private bool _started;

        public delegate void Progress(int progress, string time);
        public Progress OnProgress;

        public delegate void DurationInfo(string duration);
        public DurationInfo OnDurationInfo;

        public delegate void DownloadComplete();
        public DownloadComplete OnDownloadComplete;

        public Downloader()
        {
            Reset();
        }

        /// <summary>
        /// Resets the downloader
        /// </summary>
        private void Reset()
        {
            _progress = 0;
            _duration = TimeSpan.MinValue;
        }

        /// <summary>
        /// Indicates whether a download is in progress
        /// </summary>
        /// <returns></returns>
        public bool IsDownloading()
        {
            return _started;
        }

        /// <summary>
        /// Copy video from an M3U8 stream using ffmpeg
        /// </summary>
        /// <param name="url"></param>
        /// <param name="output"></param>
        public void DownloadStream(string url, string output)
        {
            Reset();
            _ffmpeg = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i {url} -c copy -bsf:a aac_adtstoasc {output}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _ffmpeg.ErrorDataReceived += POnErrorDataReceived;

            _started = true;
            _ffmpeg.Start();
            _ffmpeg.BeginOutputReadLine();
            _ffmpeg.BeginErrorReadLine();
            _ffmpeg.WaitForExit();
            _started = false;
            _ffmpeg.ErrorDataReceived -= POnErrorDataReceived;
        }

        /// <summary>
        /// Stop any running downloads
        /// </summary>
        /// <returns></returns>
        public bool Kill()
        {
            if (_started)
            {
                _ffmpeg.Kill();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when ffmpeg outputs data to its stderror output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataReceivedEventArgs"></param>
        private void POnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (_duration == TimeSpan.MinValue && dataReceivedEventArgs.Data.Contains("Duration:"))
            {
                var duration = dataReceivedEventArgs.Data.Substring(12, 8);
                _duration = TimeSpan.Parse(duration);

                OnDurationInfo?.Invoke(duration);
            }
            else if (dataReceivedEventArgs.Data != null && dataReceivedEventArgs.Data.Contains("time="))
            {
                var timeArr = dataReceivedEventArgs.Data.Split(new[] { "time=" }, StringSplitOptions.None);
                var timeStr = timeArr[1].Substring(0, 8);

                var time = TimeSpan.Parse(timeStr);

                var progress = (int)((time.TotalSeconds / _duration.TotalSeconds) * 100);
                if (progress != _progress)
                {
                    _progress = progress;

                    if (_progress >= 100)
                    {
                        OnDownloadComplete?.Invoke();
                    }
                    else
                    {
                        OnProgress?.Invoke(_progress, timeStr);
                    }
                }
            }
        }

        /// <summary>
        /// Download a string from an url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<string> DownloadString(string uri)
        {
            using (var wc = WebClientFactory.GetClient())
            {
                var content = await wc.DownloadStringTaskAsync(uri);
                return content;
            }
        }
    }
}
