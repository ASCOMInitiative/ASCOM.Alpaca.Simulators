using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/switch/")]
    public class SwitchController : AlpacaController
    {
        public const string APIRoot = "api/v1/switch/";

        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetSwitch(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/maxswitch")]
        public ActionResult<IntResponse> MaxSwitch([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitch, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/canwrite")]
        public ActionResult<BoolResponse> CanWrite([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).CanWrite(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/getswitch")]
        public ActionResult<BoolResponse> GetSwitch([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitch(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/getswitchdescription")]
        public ActionResult<StringResponse> GetSwitchDescription([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchDescription(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/getswitchname")]
        public ActionResult<StringResponse> GetSwitchName([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchName(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/getswitchvalue")]
        public ActionResult<DoubleResponse> GetSwitchValue([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).GetSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/minswitchvalue")]
        public ActionResult<DoubleResponse> MinSwitchValue([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MinSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/maxswitchvalue")]
        public ActionResult<DoubleResponse> MaxSwitchValue([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).MaxSwitchValue(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/switchstep")]
        public ActionResult<DoubleResponse> SwitchStep([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [Required][DefaultValue(0)] short ID, [SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SwitchStep(ID), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/setswitch")]
        public ActionResult<Response> SetSwitch([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][Required][DefaultValue(0)] short ID, [FromForm][Required] bool State, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitch(ID, State), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/setswitchname")]
        public ActionResult<Response> SetSwitchName([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][Required][DefaultValue(0)] short ID, [Required][FromForm] string Name, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchName(ID, Name), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/setswitchvalue")]
        public ActionResult<Response> SetSwitchValue([DefaultValue(0)][SwaggerSchema(Strings.DeviceIDDescription)] int DeviceNumber,
 [FromForm][Required][DefaultValue(0)] short ID, [Required][FromForm] double Value, [FromForm][SwaggerSchema(Description = Strings.ClientIDDescription, Format = "uint32")][Range(0, 4294967295)]uint ClientID = 0, 
 [FromForm][SwaggerSchema(Strings.ClientTransactionIDDescription, Format = "uint32")][Range(0, 4294967295)] uint ClientTransactionID = 0
)
        {
            return ProcessRequest(() => DeviceManager.GetSwitch(DeviceNumber).SetSwitchValue(ID, Value), DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}