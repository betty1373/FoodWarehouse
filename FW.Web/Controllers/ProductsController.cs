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
    }
  
}
