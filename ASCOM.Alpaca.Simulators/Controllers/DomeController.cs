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
    [Route("api/v1/dome/")]
    public class DomeController : AlpacaController
    {
        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetDome(DeviceNumber);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/altitude")]
        public ActionResult<DoubleResponse> Altitude([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).Altitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/athome")]
        public ActionResult<BoolResponse> AtHome([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).AtHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/atpark")]
        public ActionResult<BoolResponse> AtPark([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).AtPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/azimuth")]
        public ActionResult<DoubleResponse> Azimuth([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).Azimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/canfindhome")]
        public ActionResult<BoolResponse> CanFindHome([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanFindHome, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/canpark")]
        public ActionResult<BoolResponse> CanPark([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/cansetaltitude")]
        public ActionResult<BoolResponse> CanSetAltitude([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSetAltitude, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/cansetazimuth")]
        public ActionResult<BoolResponse> CanSetAzimuth([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSetAzimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/cansetpark")]
        public ActionResult<BoolResponse> CanSetPark([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSetPark, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/cansetshutter")]
        public ActionResult<BoolResponse> CanSetShutter([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSetShutter, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/canslave")]
        public ActionResult<BoolResponse> CanSlave([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSlave, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/cansyncazimuth")]
        public ActionResult<BoolResponse> CanSyncAzimuth([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CanSyncAzimuth, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/shutterstatus")]
        public ActionResult<IntResponse> ShutterStatus([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => (int)DeviceManager.GetDome(DeviceNumber).ShutterStatus, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/slaved")]
        public ActionResult<BoolResponse> Slaved([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).Slaved, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/slaved")]
        public ActionResult<Response> Slaved([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][FromForm] bool Slaved, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => { DeviceManager.GetDome(DeviceNumber).Slaved = Slaved; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/slewing")]
        public ActionResult<BoolResponse> Slewing([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).Slewing, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/abortslew")]
        public ActionResult<Response> AbortSlew([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).AbortSlew(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/closeshutter")]
        public ActionResult<Response> CloseShutter([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).CloseShutter(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/findhome")]
        public ActionResult<Response> FindHome([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).FindHome(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/openshutter")]
        public ActionResult<Response> OpenShutter([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).OpenShutter(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/park")]
        public ActionResult<Response> Park([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).Park(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/setpark")]
        public ActionResult<Response> SetPark([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).SetPark(), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/slewtoaltitude")]
        public ActionResult<Response> SlewToAltitude([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][FromForm] double Altitude, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).SlewToAltitude(Altitude), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/slewtoazimuth")]
        public ActionResult<Response> SlewToAzimuth([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][FromForm] double Azimuth, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).SlewToAzimuth(Azimuth), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("{DeviceNumber}/synctoazimuth")]
        public ActionResult<Response> SyncToAzimuth([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][FromForm] double Azimuth, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientID = 0,
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetDome(DeviceNumber).SyncToAzimuth(Azimuth), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}