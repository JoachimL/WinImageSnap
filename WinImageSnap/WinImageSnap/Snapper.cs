using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public Snapper(Configuration config)
        {
            if (config.MaxSleepTime > 0)
                _maxSleepTime = config.MaxSleepTime;

            if (!string.IsNullOrWhiteSpace(config.RepositoryName))
                _repositoryName = config.RepositoryName;

            if (!string.IsNullOrWhiteSpace(config.OutputFolder))
                _outputFolder = config.OutputFolder;
        }

        internal void Snap()
        {
            var webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            FilterInfo webcamDevice = webcams[0];
            _cam = new VideoCaptureDevice(webcamDevice.MonikerString);
            _cam.NewFrame += CamOnSnapshotFrame;
            while (_bitmap == null)
            {
                _cam.Start();
                WaitForSnap();
                _cam.Stop();
            }
            SaveBitmap(_bitmap);
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

        private void SaveBitmap(Bitmap bitmap)
        {
            AddTextToBitmap(bitmap);
            if (!Directory.Exists(_outputFolder))
                Directory.CreateDirectory(_outputFolder);
            bitmap.Save(Path.Combine(_outputFolder, "snap_" + GetTimestamp() + ".png"), ImageFormat.Png);
        }

        private void AddTextToBitmap(Bitmap bitmap)
        {
            var font = new Font("Courier New", 20, FontStyle.Regular);
            var graphics = Graphics.FromImage(bitmap);

            if (!string.IsNullOrWhiteSpace(_repositoryName))
            {
                SizeF measuredSize = graphics.MeasureString(_repositoryName, font);
                graphics.DrawString(_repositoryName, font, Brushes.DarkOrange, bitmap.Width - measuredSize.Width, bitmap.Height - measuredSize.Height);
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
