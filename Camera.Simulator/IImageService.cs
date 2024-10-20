using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading.Tasks;

namespace CameraSimulator
{
    internal interface IImageService
    {
        double RightAscension { get; set; }
        double Declination { get; set; }
        double Fov { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        double RotationAngle { get; set; }

        bool ImageReady { get; }

        /// <summary>
        /// Task to wait for asyncronous download
        /// </summary>
        Task DataReadyTask { get; }

        Image<Rgba32> GetImage();
        void StartRequest();
    }
}