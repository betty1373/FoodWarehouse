using FW.BusinessLogic.Contracts;
using FW.Web.Clients.Interfaces;
using FW.Models;
namespace FW.Web.RequestClients.Interfaces
{
    public interface IProductsRequestClient: IBaseClient<ProductVM,ProductResponseVM>
    {
        public Task<IEnumerable<ProductResponseVM>> GetByParentId(Guid prentId);
    }
}
