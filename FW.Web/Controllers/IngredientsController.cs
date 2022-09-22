using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RpcClients.Interfaces;
using FW.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : EntityApiController<IngredientVM, IngredientResponseVM>
    {
        public IngredientsController(IIngredientsRpcClient client) : base(client) { }
    }
 
}
