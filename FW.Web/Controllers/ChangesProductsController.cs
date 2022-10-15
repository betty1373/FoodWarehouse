using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RequestClients.Interfaces;
using FW.Web.RpcClients.Interfaces;
using FW.Models;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangesProductsController : EntityApiController<ChangesProductVM, ChangesProductResponseVM>
    {
        public ChangesProductsController(IChangesProductsRpcClient client) : base(client) { }
        [HttpGet("[action]/{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       
        public async Task<IActionResult> GetByParentId(Guid ProductId)
        {
            var client = (IChangesProductsRpcClient)Client;
            var response = await client.GetByParentId(ProductId);

            if (!response.Any())
                return NoContent();
            return Ok(response);
        }
    }
   
}

