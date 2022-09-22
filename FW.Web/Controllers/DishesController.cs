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
    }
 
}
