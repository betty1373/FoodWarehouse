using FW.BusinessLogic.Contracts;
using FW.Models;
using FW.Web.Clients.Interfaces;
namespace FW.Web.RpcClients.Interfaces
{
    public interface IChangesProductsRpcClient: IBaseClient<ChangesProductVM, ChangesProductResponseVM>
    {
        public Task<IEnumerable<ChangesProductResponseVM>> GetByParentId(Guid ParentId);
    }
}
