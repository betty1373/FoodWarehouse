using FW.WPF.WebAPI.Clients.Base;
using FW.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace FW.WPF.WebAPI.Interfaces;

public interface IProductsClient : IClientBase<ProductResponseVM, ProductVM>
{
    Task<IEnumerable<ProductResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default);
}