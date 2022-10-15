using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.Web.Clients.Interfaces;
namespace FW.Web.RpcClients.Interfaces
{
    public interface IDishesRpcClient : IBaseClient<DishVM, DishResponseVM>
    {
        public Task<IEnumerable<DishResponseVM>> GetByParentId(Guid ParentId);
        public Task<ResponseStatusResult> Cook(Guid id, Guid userId, int numPortions);
    }
}
