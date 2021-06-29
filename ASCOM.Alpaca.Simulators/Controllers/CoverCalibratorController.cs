using Alpaca;
using ASCOM.Alpaca.Responses;
using ASCOM.Standard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/covercalibrator/")]
    public class CoverCalibrator : AlpacaController
    {
        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetCoverCalibrator(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CoverState")]
        public ActionResult<IntResponse> CoverState([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CoverState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/CalibratorState")]
        public ActionResult<IntResponse> CalibratorState([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/Brightness")]
        public ActionResult<IntResponse> Brightness([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Brightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/MaxBrightness")]
        public ActionResult<IntResponse> MaxBrightness([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).MaxBrightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/OpenCover")]
        public ActionResult<Response> OpenCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).OpenCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/CloseCover")]
        public ActionResult<Response> CloseCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CloseCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/HaltCover")]
        public ActionResult<Response> HaltCover([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).HaltCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/CalibratorOn")]
        public ActionResult<Response> CalibratorOn([DefaultValue(0)] int DeviceNumber, [DefaultValue(0)][Required][FromForm] int Brightness, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOn(Brightness), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Brightness={Brightness}");
        }

        [HttpPut]
        [Route("{DeviceNumber}/CalibratorOff")]
        public ActionResult<Response> CalibratorOff([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOff(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}