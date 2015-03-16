using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WinImageSnap
{
    class Snapper
    {
        private bool _snapped;
        private VideoCaptureDevice _cam;
        private Bitmap _bitmap;
        private const int SleepTime = 100;
        private readonly int _maxSleepTime = 2;
        private readonly string _repositoryName = "";
        private readonly string _outputFolder = "C:\\temp";
        private readonly bool _verbose;
        private readonly string[] _cameraNames;
        private readonly Options _options;

        public Snapper(Options config)
        {
            _options = config;
            _verbose = config.Verbose;
            if (config.MaxWaitTime > 0)
                _maxSleepTime = config.MaxWaitTime;

            if (!string.IsNullOrWhiteSpace(config.RepositoryName)) 
                _repositoryName = config.RepositoryName;

            if (!string.IsNullOrWhiteSpace(config.OutputFolder))
                _outputFolder = config.OutputFolder;

            if (!string.IsNullOrWhiteSpace(config.CameraNames))
                _cameraNames = config.CameraNames.Split(new[] { '|' });

        }

        public void ListCameras()
        {
            foreach (var camera in new FilterInfoCollection(FilterCategory.VideoInputDevice).Cast<FilterInfo>())
                Console.WriteLine(camera.Name);
        }

        internal void Snap()
        {
            var webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            SetCamera(webcams);
            _cam.NewFrame += CamOnSnapshotFrame;
            while (_bitmap == null)
            {
                _cam.Start();
                WaitForSnap();
                _cam.Stop();
            }
            CreateSnap(_bitmap);
        }

        private void SetCamera(FilterInfoCollection webcams)
        {
            FilterInfo device = null;
            if (_cameraNames != null)
            {
                foreach (var preferredCam in _cameraNames)
                {
                    foreach (var webCam in webcams)
                    {
                        if (((FilterInfo)webCam).Name == preferredCam)
                            device = (FilterInfo)webCam;
                        if (device != null)
                            break;
                    }
                    if (device != null)
                        break;
                }
            }
            if (device == null)
                device = webcams[0];
            _cam = new VideoCaptureDevice(device.MonikerString);
        }

        private void WaitForSnap()
        {
            var sleptFor = 0;
            while (!_snapped && sleptFor < _maxSleepTime * 1000)
            {
                Thread.Sleep(SleepTime);
                sleptFor += SleepTime;
            }
        }

        void CamOnSnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            _bitmap = (Bitmap)eventArgs.Frame.Clone();
            _snapped = true;
        }

        private void CreateSnap(Bitmap bitmap)
        {
            AddTextToBitmap(bitmap);
            CreateDirectoryIfNecessary();
            LogOutputDirectory();
            bitmap.Save(Path.Combine(_outputFolder, "gitsnap_" + GetTimestamp() + ".png"), ImageFormat.Png);
        }

        private void CreateDirectoryIfNecessary()
        {
            if (!Directory.Exists(_outputFolder))
                Directory.CreateDirectory(_outputFolder);
        }

        private void LogOutputDirectory()
        {
            if (_verbose)
            {
                var directory = new DirectoryInfo(_outputFolder);
                Console.WriteLine("Putting snaps in {0}", directory.FullName);
            }
        }

        private void AddTextToBitmap(Bitmap bitmap)
        {
            var font = new Font("Courier New", 20, FontStyle.Regular);
            var graphics = Graphics.FromImage(bitmap);

            var imageText = "";
            if (!string.IsNullOrWhiteSpace(_repositoryName))
                imageText = _repositoryName;
            if (!string.IsNullOrWhiteSpace(_options.Branch))
                imageText = string.Concat(imageText, " (", _options.Branch, ")");

            if (!string.IsNullOrWhiteSpace(imageText))
            {
                SizeF measuredSize = graphics.MeasureString(imageText, font);
                graphics.DrawString(imageText, font, Brushes.DarkOrange, bitmap.Width - measuredSize.Width,
                    bitmap.Height - measuredSize.Height);
            }

            var timestamp = DateTime.Now.ToString("G");
            graphics.DrawString(timestamp, font, Brushes.DarkOrange, 0, 0);
        }

        private string GetTimestamp()
        {
            return DateTime.Now.ToString("s").Replace(":", "").Replace('T', '_');
        }
    }
}
