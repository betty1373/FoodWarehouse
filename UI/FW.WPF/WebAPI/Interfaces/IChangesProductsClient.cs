using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FW.Domain;

namespace FW.WPF.WebAPI.Interfaces;

public interface IChangesProductsClient : IClientBase<ChangesProductResponseVM, ChangesProductVM>
{
    Task<IEnumerable<ChangesProductResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default);

    //Task<Product> CreateProductAsync(
    //    string Name,
    //    decimal Price,
    //    string Description,
    //    string CategoryName,
    //    CancellationToken Cancel = default);
}