using FW.Domain.Models;
using FW.Web.Clients.Interfaces;
namespace FW.Web.RpcClients.Interfaces
{
    public interface IWarehousesRpcClient : IBaseClient<WarehouseVM, WarehouseResponseVM>
    {
        public Task<WarehouseResponseVM> GetByParentId(Guid ParentId);
    }
}
