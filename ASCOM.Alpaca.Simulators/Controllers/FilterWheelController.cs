using Alpaca;
using ASCOM.Common.Alpaca;
using ASCOM.Common.DeviceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASCOM.Alpaca.Simulators
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/v1/filterwheel/")]
    public class FilterWheelController : AlpacaController
    {
        [NonAction]
        public override IAscomDevice GetDevice(int DeviceNumber)
        {
            return DeviceManager.GetFilterWheel(DeviceNumber);
        }

        [HttpGet]
        [Route("{DeviceNumber}/focusoffsets")]
        public ActionResult<IntListResponse> FocusOffsets([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetFilterWheel(DeviceNumber).FocusOffsets, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/names")]
        public ActionResult<StringListResponse> Names([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetFilterWheel(DeviceNumber).Names, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpGet]
        [Route("{DeviceNumber}/position")]
        public ActionResult<IntResponse> Position([DefaultValue(0)] int DeviceNumber, uint ClientID = 0, uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => DeviceManager.GetFilterWheel(DeviceNumber).Position, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }

        [HttpPut]
        [Route("{DeviceNumber}/position")]
        public ActionResult<Response> Position([DefaultValue(0)] int DeviceNumber, [Required][FromForm] short Position, [FromForm] uint ClientID = 0, [FromForm] uint ClientTransactionID = 0)
        {
            return ProcessRequest(() => { DeviceManager.GetFilterWheel(DeviceNumber).Position = Position; }, DeviceManager.ServerTransactionID, ClientID, ClientTransactionID);
        }
    }
}