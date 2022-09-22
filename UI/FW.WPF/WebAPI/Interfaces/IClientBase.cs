using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FW.Domain;
namespace FW.WPF.WebAPI.Interfaces;

public interface IClientBase<T> where T : class,IEntity
{
  
    Task<IEnumerable<T>> GetAllAsync(string? token, CancellationToken Cancel = default);

    Task<IEnumerable<T>> GetAsync(int Skip, int Take, string? token, CancellationToken Cancel = default);

    Task<T?> GetByIdAsync(Guid Id, string? token, CancellationToken Cancel = default);

    Task<Guid> AddAsync(T Item, string? token, CancellationToken Cancel = default);

    Task<bool> UpdateAsync(T Item, string? token, CancellationToken Cancel = default);

    Task<T?> RemoveAsync(Guid Id, string? token, CancellationToken Cancel = default);
}
