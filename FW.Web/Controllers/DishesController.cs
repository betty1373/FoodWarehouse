using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RpcClients.Interfaces;
using FW.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : EntityApiController<DishVM, DishResponseVM>
    {
        public DishesController(IDishesRpcClient client) : base(client) { }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByParentId()
        {
            var client = (IDishesRpcClient)Client;
            var response = await client.GetByParentId(UserId);

            if (response == null)
                return NoContent();
            return Ok(response);
        }
        [HttpPut("[action]/{id}:{warehauseId}:{numPortions}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cook(Guid id,Guid warehauseId, int numPortions)
        {
            if (id == Guid.Empty || numPortions == 0)
                return BadRequest();
            var client = (IDishesRpcClient)Client;
            var response = await client.Cook(id, warehauseId, numPortions);

            if (response.Status == StatusResult.NotFound)
                return NotFound(response.Title);

            return Ok(response);
        }
    }
 
}
