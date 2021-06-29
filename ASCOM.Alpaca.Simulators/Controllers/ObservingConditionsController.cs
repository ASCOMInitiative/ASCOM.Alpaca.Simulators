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
    [Route("api/v1/observingconditions/")]
    public class ObservingConditionsController : AlpacaController
    {
        public new const string APIRoot = "api/v1/observingconditions/";

        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetObservingConditions(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/averageperiod")]
        public ActionResult<DoubleResponse> AveragePeriod([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).AveragePeriod, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/averageperiod")]
        public ActionResult<Response> AveragePeriod([DefaultValue(0)] int DeviceNumber, [Required][DefaultValue(1)][FromForm] double averageperiod, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetObservingConditions(DeviceNumber).AveragePeriod = averageperiod; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/cloudcover")]
        public ActionResult<DoubleResponse> CloudCover([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).CloudCover, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/dewpoint")]
        public ActionResult<DoubleResponse> DewPoint([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).DewPoint, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/humidity")]
        public ActionResult<DoubleResponse> Humidity([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).Humidity, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/pressure")]
        public ActionResult<DoubleResponse> Pressure([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).Pressure, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/rainrate")]
        public ActionResult<DoubleResponse> RainRate([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).RainRate, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/skybrightness")]
        public ActionResult<DoubleResponse> SkyBrightness([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).SkyBrightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/skyquality")]
        public ActionResult<DoubleResponse> SkyQuality([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).SkyQuality, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/skytemperature")]
        public ActionResult<DoubleResponse> SkyTemperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).SkyTemperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/starfwhm")]
        public ActionResult<DoubleResponse> StarFWHM([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).StarFWHM, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/temperature")]
        public ActionResult<DoubleResponse> Temperature([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).Temperature, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/winddirection")]
        public ActionResult<DoubleResponse> WindDirection([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).WindDirection, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/windgust")]
        public ActionResult<DoubleResponse> WindGust([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).WindGust, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/windspeed")]
        public ActionResult<DoubleResponse> WindSpeed([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).WindSpeed, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/refresh")]
        public ActionResult<Response> Refresh([DefaultValue(0)] int DeviceNumber, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).Refresh(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/sensordescription")]
        public ActionResult<StringResponse> SensorDescription([DefaultValue(0)] int DeviceNumber, string sensorname, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).SensorDescription(sensorname), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/timesincelastupdate")]
        public ActionResult<DoubleResponse> TimeSinceLastUpdate([DefaultValue(0)] int DeviceNumber, string sensorname, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetObservingConditions(DeviceNumber).TimeSinceLastUpdate(sensorname), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}