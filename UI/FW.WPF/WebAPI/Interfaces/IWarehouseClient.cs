using FW.WPF.WebAPI.Clients.Base;
using FW.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace FW.WPF.WebAPI.Interfaces;

public interface IWarehouseClient : IClientBase<WarehouseResponseVM, WarehouseVM >
{
    Task<WarehouseResponseVM?> GetByParentIdAsync(string token, CancellationToken Cancel = default);
}