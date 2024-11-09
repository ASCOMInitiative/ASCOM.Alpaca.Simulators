using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using System.Threading.Tasks;

namespace CameraSimulator
{
    internal class ImageServiceFile : IImageService
    {
        public double RightAscension { get; set; }
        public double Declination { get; set; }
        public double Fov { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double RotationAngle { get; set; }

        public bool ImageReady { get; set; }

        public Task DataReadyTask => Task.CompletedTask;

        private IImageDecoder decoder = new JpegDecoder();
        Image<Rgba32> image;

        public ImageServiceFile(string filepath)
        {
           image= Image.Load<Rgba32>(filepath);
        }

        public Image<Rgba32> GetImage()
        {
            return image;
        }

        public void StartRequest()
        {
            // nothing to do            
        }
    }
}