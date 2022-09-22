using FW.BusinessLogic.Contracts;
using FW.Web.Controllers.Base;
using FW.Web.RequestClients.Interfaces;
using FW.Web.RpcClients.Interfaces;
using FW.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangesProductsController : EntityApiController<ChangesProductVM, ChangesProductResponseVM>
    {
        public ChangesProductsController(IChangesProductsRpcClient client) : base(client) { }
    }
   
}

