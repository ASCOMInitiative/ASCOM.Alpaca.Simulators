using ASCOM.DeviceInterface;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Simulators.LocalServer.Drivers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Camera : BaseDriver, ASCOM.DeviceInterface.ICameraV3, IDisposable
    {
        public ASCOM.Common.DeviceInterfaces.ICameraV3 Device => (base.DeviceV2 as ASCOM.Common.DeviceInterfaces.ICameraV3);

        public short BinX { get => Device.BinX; set => Device.BinX = value; }
        public short BinY { get => Device.BinY; set => Device.BinY = value; }

        public CameraStates CameraState => (CameraStates)Device.CameraState;

        public int CameraXSize => Device.CameraXSize;

        public int CameraYSize => Device.CameraYSize;

        public bool CanAbortExposure => Device.CanAbortExposure;

        public bool CanAsymmetricBin => Device.CanAsymmetricBin;

        public bool CanGetCoolerPower => Device.CanGetCoolerPower;

        public bool CanPulseGuide => Device.CanPulseGuide;

        public bool CanSetCCDTemperature => Device.CanSetCCDTemperature;

        public bool CanStopExposure => Device.CanStopExposure;

        public double CCDTemperature => Device.CCDTemperature;

        public bool CoolerOn { get => Device.CoolerOn; set => Device.CoolerOn = value; }

        public double CoolerPower => Device.CoolerPower;

        public double ElectronsPerADU => Device.ElectronsPerADU;

        public double FullWellCapacity => Device.FullWellCapacity;

        public bool HasShutter => Device.HasShutter;

        public double HeatSinkTemperature => Device.HeatSinkTemperature;

        public object ImageArray => Device.ImageArray;

        public object ImageArrayVariant => Device.ImageArrayVariant;

        public bool ImageReady => Device.ImageReady;

        public bool IsPulseGuiding => Device.IsPulseGuiding;

        public double LastExposureDuration => Device.LastExposureDuration;

        public string LastExposureStartTime => Device.LastExposureStartTime;

        public int MaxADU => Device.MaxADU;

        public short MaxBinX => Device.MaxBinX;

        public short MaxBinY => Device.MaxBinY;

        public int NumX { get => Device.NumX; set => Device.NumX = value; }
        public int NumY { get => Device.NumY; set => Device.NumY = value; }

        public double PixelSizeX => Device.PixelSizeX;

        public double PixelSizeY => Device.PixelSizeY;

        public double SetCCDTemperature { get => Device.SetCCDTemperature; set => Device.SetCCDTemperature = value; }
        public int StartX { get => Device.StartX; set => Device.StartX = value; }
        public int StartY { get => Device.StartY; set => Device.StartY = value; }

        public short BayerOffsetX => Device.BayerOffsetX;

        public short BayerOffsetY => Device.BayerOffsetY;

        public bool CanFastReadout => Device.CanFastReadout;

        public double ExposureMax => Device.ExposureMax;

        public double ExposureMin => Device.ExposureMin;

        public double ExposureResolution => Device.ExposureResolution;

        public bool FastReadout { get => Device.FastReadout; set => Device.FastReadout = value; }
        public short Gain { get => Device.Gain; set => Device.Gain = value; }

        public short GainMax => Device.GainMax;

        public short GainMin => Device.GainMin;

        public ArrayList Gains => new ArrayList(Device.Gains.ToArray());

        public short PercentCompleted => Device.PercentCompleted;

        public short ReadoutMode { get => Device.ReadoutMode; set => Device.ReadoutMode = value; }

        public ArrayList ReadoutModes => new ArrayList(Device.ReadoutModes.ToArray());

        public string SensorName => Device.SensorName;

        public SensorType SensorType => (SensorType) Device.SensorType;

        public int Offset { get => Device.Offset; set => Device.Offset = value; }

        public int OffsetMax => Device.OffsetMax;

        public int OffsetMin => Device.OffsetMin;

        public ArrayList Offsets => new ArrayList(Device.Offsets.ToArray());

        public double SubExposureDuration { get => Device.SubExposureDuration; set => Device.SubExposureDuration = value; }

        public Camera()
        {
            base.GetDevice = () => ASCOM.Alpaca.DeviceManager.GetCamera(0);
        }

        public void AbortExposure()
        {
            Device.AbortExposure();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            Device.PulseGuide((ASCOM.Common.DeviceInterfaces.GuideDirection)Direction, Duration);
        }

        public void StartExposure(double Duration, bool Light)
        {
            Device.StartExposure(Duration, Light);
        }

        public void StopExposure()
        {
            Device.StopExposure();
        }
    }
}
