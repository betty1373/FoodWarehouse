using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FW.Domain;
namespace FW.WPF.WebAPI.Interfaces;

public interface IClientBase<T,K> where T : class, IEntity
                                  where K: class{
  
    Task<IEnumerable<T>> GetAllAsync(string? token, CancellationToken Cancel = default);

    Task<IEnumerable<T>> GetAsync(int Skip, int Take, string? token, CancellationToken Cancel = default);

    Task<T?> GetByIdAsync(Guid Id, string? token, CancellationToken Cancel = default);

    Task<Guid> AddAsync(K Item, string? token, CancellationToken Cancel = default);

    Task<bool> UpdateAsync(K Item, string? token, CancellationToken Cancel = default);

    Task<Guid?> RemoveAsync(Guid Id, string? token, CancellationToken Cancel = default);
}
