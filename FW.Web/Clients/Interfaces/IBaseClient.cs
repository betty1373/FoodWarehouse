using FW.ResponseStatus;

namespace FW.Web.Clients.Interfaces
{
    public interface IBaseClient<T, K>
        where T : class
        where K : class
    {
        public Task<K> Get(Guid id);
        public Task<IEnumerable<K>> GetPage(int Skip, int Take);
        public Task<ResponseStatusResult> Create(T model, Guid userId);
        public Task<ResponseStatusResult> Update(Guid id, T model, Guid userId);
        public Task<ResponseStatusResult> Delete(Guid id);
        public Task<IEnumerable<K>> GetAll();
        public Task<int> Count();
    }
}
