using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RpcClients.Interfaces;
using FW.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : EntityApiController<RecipeVM, RecipeResponseVM>
    {
        public RecipesController(IRecipesRpcClient client) : base(client) { }
        [HttpGet("[action]/{DishId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      //  [HttpGet("dish/{DishId}")]
        public async Task<IActionResult> GetByParentId(Guid DishId)
        {
            var client = (IRecipesRpcClient) Client;
            var response = await client.GetByParentId(DishId);

            if (!response.Any())
                return NoContent();
            return Ok(response);
        }
    }
  
}
