using FW.BusinessLogic.Contracts;
using FW.Models;
using FW.Web.Clients.Interfaces;
namespace FW.Web.RpcClients.Interfaces
{
    public interface IIngredientsRpcClient : IBaseClient<IngredientVM,IngredientResponseVM>
    {
    }
}
