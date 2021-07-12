using Alpaca;
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Helpers;
using ASCOM.Standard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/camera/")]
    public class CameraController : AlpacaController
    {
        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetCamera(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/bayeroffsetx")]
        public ActionResult<IntResponse> BayerOffsetX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).BayerOffsetX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/bayeroffsety")]
        public ActionResult<IntResponse> BayerOffsetY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).BayerOffsetY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/binx")]
        public ActionResult<IntResponse> BinX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).BinX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/binx")]
        public ActionResult<Response> BinX([DefaultValue(0)] int DeviceNumber, [FromForm] short binx, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).BinX = binx; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/biny")]
        public ActionResult<IntResponse> BinY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).BinY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/biny")]
        public ActionResult<Response> BinY([DefaultValue(0)] int DeviceNumber, [FromForm] short biny, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).BinY = biny; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/camerastate")]
        public ActionResult<IntResponse> CameraState([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCamera(DeviceNumber).CameraState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cameraxsize")]
        public ActionResult<IntResponse> CameraXSize([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CameraXSize, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cameraysize")]
        public ActionResult<IntResponse> CameraYSize([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CameraYSize, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canabortexposure")]
        public ActionResult<BoolResponse> CanAbortExposure([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanAbortExposure, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canasymmetricbin")]
        public ActionResult<BoolResponse> CanAsymmetricBin([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanAsymmetricBin, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canfastreadout")]
        public ActionResult<BoolResponse> CanFastReadout([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanFastReadout, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cangetcoolerpower")]
        public ActionResult<BoolResponse> CanGetCoolerPower([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanGetCoolerPower, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canpulseguide")]
        public ActionResult<BoolResponse> CanPulseGuide([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanPulseGuide, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cansetccdtemperature")]
        public ActionResult<BoolResponse> CanSetCCDTemperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanSetCCDTemperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canstopexposure")]
        public ActionResult<BoolResponse> CanStopExposure([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CanStopExposure, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/ccdtemperature")]
        public ActionResult<DoubleResponse> CCDTemperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CCDTemperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cooleron")]
        public ActionResult<BoolResponse> CoolerOn([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CoolerOn, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/cooleron")]
        public ActionResult<Response> CoolerOn([DefaultValue(0)] int DeviceNumber, [FromForm] bool cooleron, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).CoolerOn = cooleron; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/coolerpower")]
        public ActionResult<DoubleResponse> CoolerPower([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).CoolerPower, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/electronsperadu")]
        public ActionResult<DoubleResponse> ElectronsPerADU([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ElectronsPerADU, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/exposuremax")]
        public ActionResult<DoubleResponse> ExposureMax([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ExposureMax, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/exposuremin")]
        public ActionResult<DoubleResponse> ExposureMin([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ExposureMin, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/exposureresolution")]
        public ActionResult<DoubleResponse> ExposureResolution([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ExposureResolution, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/fastreadout")]
        public ActionResult<BoolResponse> FastReadout([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).FastReadout, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/fastreadout")]
        public ActionResult<Response> FastReadout([DefaultValue(0)] int DeviceNumber, [FromForm] bool fastreadout, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).FastReadout = fastreadout; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/fullwellcapacity")]
        public ActionResult<DoubleResponse> FullWellCapacity([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).FullWellCapacity, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/gain")]
        public ActionResult<IntResponse> Gain([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).Gain, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/gain")]
        public ActionResult<Response> Gain([DefaultValue(0)] int DeviceNumber, [FromForm] short gain, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).Gain = gain; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/gainmax")]
        public ActionResult<IntResponse> GainMax([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).GainMax, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/gainmin")]
        public ActionResult<IntResponse> GainMin([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).GainMin, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/gains")]
        public ActionResult<StringListResponse> Gains([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).Gains, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/hasshutter")]
        public ActionResult<BoolResponse> HasShutter([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).HasShutter, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/heatsinktemperature")]
        public ActionResult<DoubleResponse> HeatSinkTemperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).HeatSinkTemperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/imagearray")]
        public ActionResult ImageArray([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                var rawresponse = string.Empty;

                if (DeviceManager.GetCamera(DeviceNumber).ImageArray is int[,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new IntArray2DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as int[,]));
                }
                else if (DeviceManager.GetCamera(DeviceNumber).ImageArray is int[,,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new IntArray3DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as int[,,]));
                }
                else if (DeviceManager.GetCamera(DeviceNumber).ImageArray is double[,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new DoubleArray2DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as double[,]));
                }
                else if (DeviceManager.GetCamera(DeviceNumber).ImageArray is double[,,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new DoubleArray3DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as double[,,]));
                }
                else if (DeviceManager.GetCamera(DeviceNumber).ImageArray is short[,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new ShortArray2DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as short[,]));
                }
                else if (DeviceManager.GetCamera(DeviceNumber).ImageArray is short[,,])
                {
                    rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new ShortArray3DResponse(ClientTransactionID, TransactionID, DeviceManager.GetCamera(DeviceNumber).ImageArray as short[,,]));
                }
                else
                {
                    throw new Exception("Failed to read ImageArray type from camera");
                }

                return Content(rawresponse, "application/json; charset=utf-8");
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<AxisRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        [HttpGet]
        [Route("{DeviceNumber}/imagearrayvariant")]
        public ActionResult ImageArrayVariant([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            var TransactionID = DeviceManager.ServerTransactionID;
            try
            {
                Logging.LogAPICall(HttpContext.Connection.RemoteIpAddress, HttpContext.Request.Path.ToString(), ClientID, ClientTransactionID, TransactionID);

                var rawresponse = string.Empty;

                Array raw_data = (Array)DeviceManager.GetCamera(DeviceNumber).ImageArrayVariant;

                var type = raw_data.GetValue(0, 0).GetType();

                if (type == typeof(int))
                {
                    if (raw_data.Rank == 2)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new IntArray2DResponse(ClientTransactionID, TransactionID, To2DArray<int>(raw_data)));
                    }
                    else if (raw_data.Rank == 3)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new IntArray3DResponse(ClientTransactionID, TransactionID, To3DArray<int>(raw_data)));
                    }
                    else
                    {
                        throw new Exception("Failed to read valid Rank from camera");
                    }
                }
                else if (type == typeof(short))
                {
                    if (raw_data.Rank == 2)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new ShortArray2DResponse(ClientTransactionID, TransactionID, To2DArray<short>(raw_data)));
                    }
                    else if (raw_data.Rank == 3)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new ShortArray3DResponse(ClientTransactionID, TransactionID, To3DArray<short>(raw_data)));
                    }
                    else
                    {
                        throw new Exception("Failed to read valid Rank from camera");
                    }
                }
                else if (type == typeof(double))
                {
                    if (raw_data.Rank == 2)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new DoubleArray2DResponse(ClientTransactionID, TransactionID, To2DArray<double>(raw_data)));
                    }
                    else if (raw_data.Rank == 3)
                    {
                        rawresponse = Newtonsoft.Json.JsonConvert.SerializeObject(new DoubleArray3DResponse(ClientTransactionID, TransactionID, To3DArray<double>(raw_data)));
                    }
                    else
                    {
                        throw new Exception("Failed to read valid Rank from camera");
                    }
                }
                else
                {
                    throw new Exception("Failed to read ImageArrayVariant type from camera");
                }

                return Content(rawresponse, "application/json; charset=utf-8");
            }
            catch (DeviceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ResponseHelpers.ExceptionResponseBuilder<AxisRatesResponse>(ex, ClientTransactionID, TransactionID));
            }
        }

        private static T[,] To2DArray<T>(Array raw)
        {
            T[,] result = new T[raw.GetLength(0), raw.GetLength(1)];

            for (int i = 0; i < raw.GetLength(0); ++i)
            {
                Array.Copy(raw, i * raw.GetLength(1), result, i * result.GetLength(1), raw.GetLength(1));
            }

            return result;
        }

        private static T[,,] To3DArray<T>(Array raw)
        {
            T[,,] result = new T[raw.GetLength(0), raw.GetLength(1), raw.GetLength(2)];

            for (int i = 0; i < raw.GetLength(0); ++i)
            {
                for (int j = 0; j < raw.GetLength(0); ++j)
                {
                    for (int k = 0; k < raw.GetLength(0); ++k)
                    {
                        result[i, j, k] = (T)raw.GetValue(i, j, k);
                    }
                }
            }

            return result;
        }

        [HttpGet]
        [Route("{DeviceNumber}/imageready")]
        public ActionResult<BoolResponse> ImageReady([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ImageReady, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/ispulseguiding")]
        public ActionResult<BoolResponse> IsPulseGuiding([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).IsPulseGuiding, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/lastexposureduration")]
        public ActionResult<DoubleResponse> LastExposureDuration([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).LastExposureDuration, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/lastexposurestarttime")]
        public ActionResult<StringResponse> LastExposureStartTime([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).LastExposureStartTime, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/maxadu")]
        public ActionResult<IntResponse> MaxADU([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).MaxADU, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/maxbinx")]
        public ActionResult<IntResponse> MaxBinX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).MaxBinX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/maxbiny")]
        public ActionResult<IntResponse> MaxBinY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).MaxBinY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/numx")]
        public ActionResult<IntResponse> NumX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).NumX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/numx")]
        public ActionResult<Response> NumX([DefaultValue(0)] int DeviceNumber, [FromForm] int numx, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).NumX = numx; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/numy")]
        public ActionResult<IntResponse> NumY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).NumY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/numy")]
        public ActionResult<Response> NumY([DefaultValue(0)] int DeviceNumber, [FromForm] int numy, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).NumY = numy; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/offset")]
        public ActionResult<IntResponse> Offset([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).Offset, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/offset")]
        public ActionResult<Response> Offset([DefaultValue(0)] int DeviceNumber, [FromForm] int offset, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).Offset = offset; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/offsetmax")]
        public ActionResult<IntResponse> OffsetMax([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).OffsetMax, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/offsetmin")]
        public ActionResult<IntResponse> OffsetMin([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).OffsetMin, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/offsets")]
        public ActionResult<StringListResponse> Offsets([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).Offsets, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/percentcompleted")]
        public ActionResult<IntResponse> PercentCompleted([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).PercentCompleted, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/pixelsizex")]
        public ActionResult<DoubleResponse> PixelSizeX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).PixelSizeX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/pixelsizey")]
        public ActionResult<DoubleResponse> PixelSizeY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).PixelSizeY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/readoutmode")]
        public ActionResult<IntResponse> ReadoutMode([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ReadoutMode, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/readoutmode")]
        public ActionResult<Response> ReadoutMode([DefaultValue(0)] int DeviceNumber, [FromForm] short readoutmode, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).ReadoutMode = readoutmode; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/readoutmodes")]
        public ActionResult<StringListResponse> ReadoutModes([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).ReadoutModes, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sensorname")]
        public ActionResult<StringResponse> SensorName([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).SensorName, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sensortype")]
        public ActionResult<IntResponse> SensorType([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCamera(DeviceNumber).SensorType, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/setccdtemperature")]
        public ActionResult<DoubleResponse> SetCCDTemperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).SetCCDTemperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/setccdtemperature")]
        public ActionResult<Response> SetCCDTemperature([DefaultValue(0)] int DeviceNumber, [FromForm] double setccdtemperature, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).SetCCDTemperature = setccdtemperature; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/startx")]
        public ActionResult<IntResponse> StartX([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).StartX, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/startx")]
        public ActionResult<Response> StartX([DefaultValue(0)] int DeviceNumber, [FromForm] int startx, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).StartX = startx; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/starty")]
        public ActionResult<IntResponse> StartY([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).StartY, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/starty")]
        public ActionResult<Response> StartY([DefaultValue(0)] int DeviceNumber, [FromForm] int starty, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).StartY = starty; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/subexposureduration")]
        public ActionResult<DoubleResponse> SubExposureDuration([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).SubExposureDuration, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/subexposureduration")]
        public ActionResult<Response> SubExposureDuration([DefaultValue(0)] int DeviceNumber, [FromForm] double SubExposureDuration, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetCamera(DeviceNumber).SubExposureDuration = SubExposureDuration; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/abortexposure")]
        public ActionResult<Response> AbortExposure([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).AbortExposure(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/pulseguide")]
        public ActionResult<Response> PulseGuide([DefaultValue(0)] int DeviceNumber, [FromForm] GuideDirection Direction, [FromForm] int Duration, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).PulseGuide(Direction, Duration), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/startexposure")]
        public ActionResult<Response> StartExposure([DefaultValue(0)] int DeviceNumber, [FromForm] double duration, [FromForm] bool light, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).StartExposure(duration, light), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/stopexposure")]
        public ActionResult<Response> StopExposure([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCamera(DeviceNumber).StopExposure(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}