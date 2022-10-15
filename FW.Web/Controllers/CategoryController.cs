using Microsoft.AspNetCore.Mvc;
using FW.Web.RequestClients.Interfaces;
using FW.Models;
using FW.Web.Controllers.Base;

namespace FW.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : EntityApiController<CategoryVM, CategoryResponseVM>
    {
        public CategoryController(ICategoriesRequestClient client) : base(client) { }
    }   
}