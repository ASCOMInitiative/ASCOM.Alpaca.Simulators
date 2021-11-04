using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

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
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/coverstate")]
        public ActionResult<IntResponse> CoverState([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CoverState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/calibratorstate")]
        public ActionResult<IntResponse> CalibratorState([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => (int)DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorState, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/brightness")]
        public ActionResult<IntResponse> Brightness([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).Brightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/maxbrightness")]
        public ActionResult<IntResponse> MaxBrightness([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).MaxBrightness, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/opencover")]
        public ActionResult<Response> OpenCover([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).OpenCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/closecover")]
        public ActionResult<Response> CloseCover([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CloseCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/haltcover")]
        public ActionResult<Response> HaltCover([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).HaltCover(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/calibratoron")]
        public ActionResult<Response> CalibratorOn([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [DefaultValue(0)][Required][FromForm] int Brightness, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOn(Brightness), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID, $"Brightness={Brightness}");
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/calibratoroff")]
        public ActionResult<Response> CalibratorOff([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetCoverCalibrator(DeviceNumber).CalibratorOff(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}