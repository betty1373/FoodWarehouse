using System.Threading;
using System.Threading.Tasks;

namespace FW.WPF.Identity.Interfaces;

public interface IClientIdentity<T> where T : class,IUserIdentity
{ 
    Task<string?> RequestPasswordTokenAsync(T model, CancellationToken Cancel = default);
}
