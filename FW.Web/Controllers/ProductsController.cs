using Microsoft.AspNetCore.Mvc;
using FW.BusinessLogic.Contracts;
using FW.Web.RequestClients.Interfaces;
using FW.Domain.Models;
using FW.Web.Controllers.Base;
using FW.Web.RpcClients.Interfaces;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : EntityApiController<ProductVM, ProductResponseVM>
    {
        public ProductsController(IProductsRequestClient client) : base(client) { }
        [HttpGet("[action]/{WarehouseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
     //   [HttpGet("warehause/{WarehauseId}")]
        public async Task<IActionResult> GetByParentId(Guid WarehouseId)
        {
            var client = (IProductsRequestClient)Client;
            var response = await client.GetByParentId(WarehouseId);

            if (!response.Any())
                return NoContent();
            return Ok(response);
        }
    }
  
}
