using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RpcClients.Interfaces;
using FW.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : EntityApiController<WarehouseVM, WarehouseResponseVM>
    {
       
        public WarehousesController(IWarehousesRpcClient client) : base(client) { }
   
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByParentId()
        {
            var client = (IWarehousesRpcClient)Client;
            var response = await client.GetByParentId(UserId);

            if (response==null)
                return NoContent();
            return Ok(response);
        }
    }    
}
