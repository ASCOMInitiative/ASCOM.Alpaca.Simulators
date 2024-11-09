using ASCOM.Common;
using ASCOM.Simulators;
using Microsoft.AspNetCore.Builder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CameraSimulator
{
    internal class ImageServiceH2F : IImageService
    {
        // Public query Parameters
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public double Fov { get; set; } = 0.77;
        public double RotationAngle { get; set; } = 0.0;
        public double RightAscension { get; set; } = 83.6287;
        public double Declination { get; set; } = 22.0147;

        // Private parameters
        string baseUrl = "https://alasky.cds.unistra.fr/hips-image-services/hips2fits";
        string alternateUrl = "http://alaskybis.cds.unistra.fr/hips-image-services/hips2fits";
        private string Hips = "CDS/P/DSS2/color";
        private string Projection = "TAN";
        private string CoordSys = "icrs";

        // HttpClient is intended to be instantiated once per application, rather than per-use.
        private static readonly HttpClient httpClient = new HttpClient();
        private byte[] lastImage;
        private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();
        private IImageDecoder decoder = new JpegDecoder();

        public bool ImageReady { get; private set; }
        public Task DataReadyTask => _taskCompletionSource.Task;

        public Image<Rgba32> GetImage()
        {
            if (!ImageReady)
                return null;
            var image = Image.Load<Rgba32>(lastImage, decoder);
            ImageReady = false;
            lastImage = null;
            return image;
        }

        public void StartRequest()
        {
            ImageReady = false;
            if (_taskCompletionSource.Task.IsCompleted)
            {
                _taskCompletionSource = new TaskCompletionSource<bool>();
            }

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                var tr = new ASCOM.Tools.Transform();
                tr.SetApparent(RightAscension, Declination);

                var builder = new UriBuilder(baseUrl);
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["hips"] = Hips;
                query["width"] = Width.ToString(CultureInfo.InvariantCulture);
                query["height"] = Height.ToString(CultureInfo.InvariantCulture);
                query["fov"] = Fov.ToString(CultureInfo.InvariantCulture);
                query["projection"] = Projection;
                query["coordsys"] = CoordSys;
                query["rotation_angle"] = RotationAngle.ToString(CultureInfo.InvariantCulture);
                query["ra"] = (tr.RAJ2000 * 15).ToString("0.0000", CultureInfo.InvariantCulture);
                query["dec"] = tr.DecJ2000.ToString("0.0000", CultureInfo.InvariantCulture);
                query["format"] = "jpg";

                builder.Query = query.ToString();
                var requestUrl = builder.ToString();

                Log.LogMessage("ImageServiceH2F", "Hips2fits Web Api request sended");
                Log.log.LogDebug($"ImageServiceH2F - reques URL:{requestUrl}");

                var requestTask = httpClient.GetByteArrayAsync(requestUrl).ContinueWith(t =>
                 {
                     lastImage = t.Result;
                     ImageReady = true;
                     _taskCompletionSource.SetResult(true);
                     Log.LogMessage("ImageServiceH2F", "Hips2fits image donload complete.");
                 });
            }
            catch (Exception ex)
            {
                Log.LogMessage("ImageServiceH2F", $"Exception: {ex.GetType().Name} - {ex.Message}");
            }
        }

    }
}
