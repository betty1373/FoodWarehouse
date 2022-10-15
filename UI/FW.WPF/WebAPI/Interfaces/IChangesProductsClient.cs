using FW.WPF.WebAPI.Clients.Base;
using FW.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace FW.WPF.WebAPI.Interfaces;

public interface IChangesProductsClient : IClientBase<ChangesProductResponseVM, ChangesProductVM>
{
    Task<IEnumerable<ChangesProductResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default);
}