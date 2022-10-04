using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace FW.WPF.WebAPI.Interfaces;

public interface IDishesClient : IClientBase<DishResponseVM, DishVM>
{
    Task<IEnumerable<DishResponseVM>> GetByParentIdAsync(string token, CancellationToken Cancel = default);

    //Task<Product> CreateProductAsync(
    //    string Name,
    //    decimal Price,
    //    string Description,
    //    string CategoryName,
    //    CancellationToken Cancel = default);
}