using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FW.Web.RpcClients.Interfaces
{
    public interface IRecipesRpcClient : IBaseClient<RecipeVM, RecipeResponseVM>
    {
        public Task<IEnumerable<RecipeResponseVM>> GetByParentId(Guid ParentId);
    }
}
